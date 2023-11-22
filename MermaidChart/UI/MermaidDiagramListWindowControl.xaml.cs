using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using System.Collections.ObjectModel;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using MessageBox = System.Windows.MessageBox;
using System.ComponentModel;
using System.Collections.Generic;
using MermaidChart.Utils;
using MermaidChart.API.Models;
using MermaidChart.API;
using System.Diagnostics;
using MermaidChart.Commands;
using System.Threading;

namespace MermaidChart.UI
{
    public partial class MermaidDiagramListWindowControl : UserControl
    {
        private CancellationTokenSource refreshCancellationSource = null;
        private APIClient client = new APIClient();
        public MermaidDiagramListWindowControl()
        {
            this.InitializeComponent();

            SettingsGeneralPage.Saved += OnSettingsChanged;
            DiagramListRefreshCommand.OnRefresh += OnRefreshCommand;
            
            RefreshList();
        }

        private void OnRefreshCommand()
        {
            RefreshList();
        }

        private void OnSettingsChanged(SettingsGeneralPage page)
        {
            RefreshList();
        }

        private void ListItemLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = (treeList.SelectedItem as MermaidListItem);
            if(item == null) return;

            switch(item.Type)
            {
                case MermaidListItemType.Project:
                    break;
                case MermaidListItemType.Document:
                    InsertDocumentLink(item.DocumentId);
                    break;
                case MermaidListItemType.Refresh:
                    RefreshList();
                    break;
                case MermaidListItemType.Settings:
                    NavigatePluginSettings();
                    break;
            }
        }

        private MermaidListItem RootNode()
        {
            return new MermaidListItem(){
                Type = MermaidListItemType.Root,
                Title = "Projects"
            };
        }

        private MermaidListItem BuildLoadingTree()
        {
            var root = RootNode();
            var loadingNode = new MermaidListItem()
            {
                Type = MermaidListItemType.Loading,
                Title = "Loading"
            };
            root.Children.Add(loadingNode);

            return root;
        }

        private MermaidListItem BuildFailedTree(Exception exception)
        {
            var root = RootNode();
            MermaidListItem errorNode;
            if (exception is UnauthorizedException)
            {
                errorNode = new MermaidListItem()
                {
                    Type = MermaidListItemType.Settings,
                    Title = "Failed: Unauthorized - Open Settings"
                };
            }else
            {
                errorNode = new MermaidListItem()
                {
                    Type = MermaidListItemType.Settings,
                    Title = "Failed. - Click to Refresh"
                };
            }
            root.Children.Add(errorNode);
            return root;
        }

        private MermaidListItem BuildTree(List<ProjectWithDocuments> data)
        {
            var root = RootNode();
            foreach(var project in data)
            {
                var projectRoot = new MermaidListItem()
                {
                    Type = MermaidListItemType.Project,
                    Title = project.Project.Title,
                };

                foreach(var document in project.Documents)
                {
                    var documentNode = new MermaidListItem()
                    {
                        Type = MermaidListItemType.Document,
                        Title = document.Title,
                        DocumentId = document.DocumentId,
                    };
                    projectRoot.Children.Add(documentNode);
                }

                root.Children.Add(projectRoot);
            }

            return root;
        }

        private void RefreshList()
        {
            SetListRoot(BuildLoadingTree());
           
            if(refreshCancellationSource != null) refreshCancellationSource.Cancel();
            refreshCancellationSource = new CancellationTokenSource();

            _ = ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                var data = await client.GetProjectsWithDocumentsAsync().WithCancellation(refreshCancellationSource.Token);

                var rootNode = data.Match(
                    left => BuildFailedTree(left),
                    right => BuildTree(right)
                );

                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                SetListRoot(rootNode);
            });
        }

        private void SetListRoot(MermaidListItem root)
        {
            var source = new List<MermaidListItem>
            {
                root
            };

            root.IsExpanded = true;

            treeList.ItemsSource = source;
        }

        private void NavigatePluginSettings()
        {
            throw new NotImplementedException();
        }

        private void InsertDocumentLink(string documentId)
        {
            _ = ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                DTE2 dte = await VS.GetServiceAsync<DTE, DTE2>();
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                var selectedText = (TextSelection)dte.ActiveDocument.Selection;
                var commenter = CommentUtils.GetCommentData(dte.ActiveDocument.Language);

                selectedText.Insert(commenter.GetCommented($"[MermaidChart: {documentId}]"));
            });
        }
    }

    enum MermaidListItemType {
        Root, Project, Document, Refresh, Settings, Loading
    }

    internal class MermaidListItem: TreeViewItemBase
    {
        public MermaidListItem()
        {
            this.Children = new ObservableCollection<MermaidListItem>();
        }
        public MermaidListItemType Type { get; set; }
        public string Title { get; set; }
        public string DocumentId { get; set; }

        public ObservableCollection<MermaidListItem> Children { get; set; }
    }


    public class TreeViewItemBase : INotifyPropertyChanged
    {
        private bool isSelected;
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                if (value != this.isSelected)
                {
                    this.isSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }

        private bool isExpanded;
        public bool IsExpanded
        {
            get { return this.isExpanded; }
            set
            {
                if (value != this.isExpanded)
                {
                    this.isExpanded = value;
                    NotifyPropertyChanged("IsExpanded");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}