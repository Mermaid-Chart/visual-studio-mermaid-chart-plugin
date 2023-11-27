using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Threading;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.Generic;
using MermaidChart.Utils;
using MermaidChart.API.Models;
using MermaidChart.API;
using MermaidChart.Commands;
using System.Threading;
using MermaidChart.Options;
using Microsoft.VisualStudio.Shell.Interop;
using static Microsoft.VisualStudio.VSConstants;
using static MermaidChart.Options.OptionsProvider;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;

namespace MermaidChart.UI
{
    public partial class MermaidDiagramListWindowControl : UserControl
    {
        private CancellationTokenSource refreshCancellationSource = null;
        private APIClient client = new APIClient();
        private MermaidChartPackage package;
        public MermaidDiagramListWindowControl()
        {
            this.InitializeComponent();

            SettingsGeneral.Saved += OnSettingsChanged;
            DiagramListRefreshCommand.OnRefresh += OnRefreshCommand;
            
            RefreshList();

            _ = ThreadHelper.JoinableTaskFactory.RunAsync(async () => {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                var vsShell = (IVsShell)ServiceProvider.GlobalProvider.GetService(typeof(IVsShell));
                if (vsShell.IsPackageLoaded(PackageGuids.MermaidChart, out var myPackage) == S_OK)
                {
                    this.package = (MermaidChartPackage)myPackage;
                }
            });
        }

        private void OnRefreshCommand()
        {
            RefreshList();
        }

        private void OnSettingsChanged(SettingsGeneral page)
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
                    Title = "Failed: Unauthorized",
                    Action = " Open Settings"
                };
            }else
            {
                errorNode = new MermaidListItem()
                {
                    Type = MermaidListItemType.Settings,
                    Title = "Failed",
                    Action = " Click to Refresh"
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
            package.ShowOptionPage(typeof(GeneralOptionPage));
        }

        private void InsertDocumentLink(string documentId)
        {
            _ = ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                var docView = await VS.Documents.GetActiveDocumentViewAsync();
                if (docView?.TextView == null) return;
                var position = docView.TextView.Caret.Position.BufferPosition;
                

                DTE2 dte = await VS.GetServiceAsync<DTE, DTE2>();
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                var selection = (TextSelection)dte.ActiveDocument.Selection;
                var editorPoint = selection.ActivePoint.CreateEditPoint();
                var commenter = CommentUtils.GetCommentData(dte.ActiveDocument.Language, Path.GetExtension(dte.ActiveDocument.Name));
                var comment = commenter.GetCommented($"[MermaidChart: {documentId}]");
                if (String.IsNullOrWhiteSpace(editorPoint.GetLines(selection.CurrentLine, selection.CurrentLine + 1)))
                {
                    selection.Insert(comment);
                }
                else if (selection.CurrentLine == 1)
                {
                    Debug.WriteLine("Put at start of document with code nextLine");
                    docView.TextBuffer.Insert(0, $"{comment}\n");
                }
                else
                {
                    var previousLine = editorPoint.GetLines(selection.CurrentLine, selection.CurrentLine + 1);
                    var whitespaceRegex = new Regex(@"([\r\n\t\f\v \\]*)");
                    var previousLineWhitespaces = whitespaceRegex.Matches(previousLine)
                        .Cast<Match>()
                        .First()
                        ?.Groups[1]
                        ?.Value;
                    if(previousLineWhitespaces == null)
                    {
                        previousLineWhitespaces = "";
                    }

                    editorPoint.LineUp();
                    editorPoint.EndOfLine();
                    editorPoint.Insert($"\n{previousLineWhitespaces}{comment}");
                }
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
            this.Action = null;
        }
        public MermaidListItemType Type { get; set; }
        public string Title { get; set; }
        public string Action { get; set; }
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