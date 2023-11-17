using EnvDTE;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MermaidChart.API;
using MermaidChart.Utils;
using System.Net.Http;
using System.IO;
using Microsoft.VisualStudio.Threading;
using System.Diagnostics;
using MermaidChart.API.Models;
using System.Windows.Media.Media3D;
using Newtonsoft.Json.Linq;
using EnvDTE80;
using Community.VisualStudio.Toolkit;

namespace MermaidChart
{
    /// <summary>
    /// Interaction logic for MermaidControlsAdornment.xaml
    /// </summary>
    public partial class MermaidControlsAdornment : UserControl
    {
        private MermaidLink link;
        private Image view;
        private Image edit;

        public MermaidControlsAdornment(MermaidLink link)
        {
            InitializeComponent();
            this.link = link;

            view = (Image)FindName("ViewImage");
            edit = (Image)FindName("EditImage");

            edit.MouseLeftButtonUp += OnEditClicked;

            view.MouseLeftButtonUp += OnViewClicked;
        }

        private void OnEditClicked(object s, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start(URLProvider.EditUrl(link.DocumentId));
        }

        private void OnViewClicked(object s, MouseEventArgs e)
        {
            ThreadHelper.JoinableTaskFactory.RunAsync(async () => {
                await VS.StatusBar.ShowProgressAsync("Diagram downloading", 1, 3);

                var format = URLProvider.DiagramFormat.PNG;
                var theme = GetDiagramTheme();

                var apiClient = new APIClient();
                var document = await apiClient.GetDocumentAsync(link.DocumentId);

                var (viewUri, filename) = await document.Match(
                    async left => {
                        return (null, null);
                    },
                    async right => {
                        var url = URLProvider.ViewUrl(right, theme, format);
                        var filename = ToFilePath(right, format, theme);
                        return (url, filename);
                    }
                );

                if (viewUri == null || filename == null)
                {
                    await VS.StatusBar.ShowProgressAsync("Diagram downloading", 3, 3);
                    await VS.MessageBox.ShowErrorAsync("Diagram download failed");
                    return;
                }

                var filePath = Path.Combine(Path.GetTempPath(), filename);

                if(File.Exists(filePath))
                {
                    await VS.StatusBar.ShowProgressAsync("Diagram downloading", 3, 3);
                    await OpenDiagramAsync(filePath);
                    return;
                }

                var r = await DownloadDiagramAsync(apiClient, viewUri, filePath);

                await r.Match(
                    async left => await VS.MessageBox.ShowErrorAsync("Diagram download failed"),
                    async right => await OpenDiagramAsync(filePath)
                );
            });
        }

        private async Task OpenDiagramAsync(string path)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            (await VS.GetServiceAsync<DTE, DTE>()).ItemOperations.OpenFile(path);
        }

        private async Task<EitherE<bool>> DownloadDiagramAsync(APIClient client, string viewUri, string path)
        {
            await VS.StatusBar.ShowProgressAsync("Diagram downloading", 2, 3);

            var r = await client.DownloadFileAsync(viewUri, path);

            await VS.StatusBar.ShowProgressAsync("Diagram downloading", 3, 3);

            return r;
        }

        private static URLProvider.DiagramTheme GetDiagramTheme()
        {
            var contrastResult = ColorUtilities.CompareContrastWithBlackAndWhite(
                VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey)
                    .ToMediaColor()
            );

            switch (contrastResult)
            {
                case ContrastComparisonResult.ContrastHigherWithBlack:
                    return URLProvider.DiagramTheme.Light;

                default:
                    return URLProvider.DiagramTheme.Dark;
            }
        }

        private static string ToFilePath(MermaidDocument document, URLProvider.DiagramFormat format, URLProvider.DiagramTheme theme)
        {
            return $"MermaidChart_{document.DocumentId}_{document.UpdatedAt.ToString("MMddyyyyHHmmss")}_{theme.ToString().ToLower()}.{format.ToString().ToLower()}";
        }
    }
}
