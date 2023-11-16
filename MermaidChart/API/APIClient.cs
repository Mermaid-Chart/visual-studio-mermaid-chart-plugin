using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MermaidChart.API.Models;
using MermaidChart.UI;
using Newtonsoft.Json;
using MermaidChart.Utils;

namespace MermaidChart.API
{
    internal class APIClient
    {
        private static string token()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var vsShellService = (ServiceProvider.GlobalProvider.GetService(typeof(IVsShell)) as IVsShell);
            if (vsShellService.IsPackageLoaded(PackageGuids.MermaidChart, out var plugin) == Microsoft.VisualStudio.VSConstants.S_OK)
            {
                var page = ((MermaidChartPackage)plugin).GetDialogPage(typeof(OptionsPage));
                var token = ((OptionsPage)page).AccessTokenField;
                if(token == null || String.IsNullOrEmpty(token))
                {
                    throw new Exception("token is null");
                }
                return token;
            }
            throw new Exception("token can't be loaded");
        }

        private static HttpClient client = new HttpClient(
            new HttpClientHandler()
            {
                Proxy = null,
                UseProxy = false
            }
        );

        internal async Task<EitherE<List<MermaidProject>>> GetProjectsAsync() 
        {
            return await GetAsync<List<MermaidProject>>(URLProvider.ProjectsUrl());
        }

        internal async Task<EitherE<MermaidDocument>> GetDocumentAsync(string documentId)
        {
            return await GetAsync<MermaidDocument>(URLProvider.DocumentUrl(documentId));
        }

        internal async Task<EitherE<List<MermaidDocument>>> GetDocumentsAsync(string projectId)
        {
            return await GetAsync<List<MermaidDocument>>(URLProvider.DocumentsUrl(projectId));
        }

        internal async Task<EitherE<string>> GetViewUrlAsync(string diagramId, URLProvider.DiagramTheme theme)
        {
            var document = await GetDocumentAsync(diagramId);
            return document.Match(
                left => new EitherE<string>(left),
                right => new EitherE<string>(URLProvider.ViewUrl(right, theme, URLProvider.DiagramFormat.PNG)) 
            );
        }

        private async Task<EitherE<T>> GetAsync<T>(string url)
        {
            try
            {

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token());
                var result = await client.SendAsync(request);
                return new EitherE<T>(JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync()));
            }catch (Exception ex)
            {
                return new EitherE<T>(ex);
            }
        }
    }
}
