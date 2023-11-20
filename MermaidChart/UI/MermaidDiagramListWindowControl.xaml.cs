using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using MessageBox = System.Windows.MessageBox;

namespace MermaidChart.UI
{
    public partial class MermaidDiagramListWindowControl : UserControl
    {
        public MermaidDiagramListWindowControl()
        {
            this.InitializeComponent();
        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            _ = ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                DTE2 dte = await VS.GetServiceAsync<DTE, DTE2>();
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                var selectedText = (TextSelection)dte.ActiveDocument.Selection;
                
                selectedText.Insert($"// {dte.ActiveDocument.Language}");
            });
        }
    }
}