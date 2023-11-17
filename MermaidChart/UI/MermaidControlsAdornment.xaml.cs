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

            
            ThreadHelper.ThrowIfNotOnUIThread();
            var dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            view = (Image)FindName("ViewImage");
            edit = (Image)FindName("EditImage");

            edit.MouseLeftButtonUp += (s, e) =>
            {
                System.Diagnostics.Process.Start(URLProvider.EditUrl(link.DocumentId));
            };

            view.MouseLeftButtonUp += (s, e) =>
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () => {
                    await VS.StatusBar.ShowProgressAsync("Diagram downloading", 1, 3);

                    var downloadingClient = new HttpClient(new HttpClientHandler()
                    {
                        Proxy = null,
                        UseProxy = false,
                    });

                    var apiClient = new APIClient();
                    var document = await apiClient.GetDocumentAsync(link.DocumentId);
                    var format = URLProvider.DiagramFormat.PNG;

                    var (viewUri, filename) = await document.Match(
                        async left => {
                            await VS.StatusBar.ShowProgressAsync("Diagram downloading", 3, 3);
                            await VS.MessageBox.ShowErrorAsync("Diagram download failed");
                            return (null, null);
                        },
                        async right => { 
                            var url = URLProvider.ViewUrl(right, URLProvider.DiagramTheme.Dark, format);
                            var filename = ToFilePath(right, format);
                            return (url, filename);
                        }
                    );

                    if(viewUri == null || filename == null) {
                        return;
                    }
                    
                    var filePath = Path.Combine(Path.GetTempPath(), filename);

                    await VS.StatusBar.ShowProgressAsync("Diagram downloading", 2, 3);

                    var r = await apiClient.DownloadFileAsync(viewUri, filePath);

                    await VS.StatusBar.ShowProgressAsync("Diagram downloading", 3, 3);

                    await r.Match(
                        async left => await VS.MessageBox.ShowErrorAsync("Diagram download failed"),
                        async right => { 
                            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                            (await VS.GetServiceAsync<DTE, DTE>()).ItemOperations.OpenFile(filePath);
                        }
                    );
                });
            };
        }

        private static string ToFilePath(MermaidDocument document, URLProvider.DiagramFormat format)
        {
            return $"MermaidChart_{document.DocumentId}_{document.UpdatedAt.ToString("MMddyyyyHHmmss")}.{format.ToString().ToLower()}";
        }
    }
}
