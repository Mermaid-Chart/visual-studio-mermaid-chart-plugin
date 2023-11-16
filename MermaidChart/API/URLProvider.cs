using Microsoft.VisualStudio.RpcContracts.Utilities;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MermaidChart.API.Models;
using MermaidChart.UI;

namespace MermaidChart.API
{
    internal static class URLProvider
    {
        internal enum DiagramTheme { Light, Dark }
        internal enum DiagramFormat { PNG, SVG, HTML }

        private static string baseUrl()
        {
            ThreadHelper.ThrowIfNotOnUIThread(); //TODO: Remove UI thread dependency
            var vsShellService = (ServiceProvider.GlobalProvider.GetService(typeof(IVsShell)) as IVsShell);
            if (vsShellService.IsPackageLoaded(PackageGuids.MermaidChart, out var plugin) == Microsoft.VisualStudio.VSConstants.S_OK) {
                var page = ((MermaidChartPackage)plugin).GetDialogPage(typeof(OptionsPage));
                return ((OptionsPage)page).BaseUrlField;
            }
            return OptionsPage.BaseUrl;
        }

        internal static string ViewUrl(MermaidDocument document, DiagramTheme theme, DiagramFormat format = DiagramFormat.PNG)
        {
            return $"{baseUrl()}/raw/{document.DocumentId}?version=v{document.Major}.{document.Minor}&theme={theme}&format={format}";
        }

        internal static string EditUrl(string documentId)
        {
            return $"{baseUrl()}/app/diagrams/{documentId}?ref=vs";
        }

        internal static string ProjectsUrl()
        {
            return $"{baseUrl()}/rest-api/projects";
        }

        internal static string DocumentsUrl(string projectId)
        {
            return $"{baseUrl()}/rest-api/projects/{projectId}/documents";
        }

        internal static string DocumentUrl(string documentId)
        {
            return $"{baseUrl()}/rest-api/documents/{documentId}";
        }
    }
}
