using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MermaidChart
{
    internal class Constants
    {
        public const string DefaultBaseUrl = "https://www.mermaidchart.com";

        public const string DiagramListWindowCaption = "Mermaid Chart";

        public const string SettingsCategoryGeneral = "Category";
        public const string SettingsBaseUrl = "Base URL";
        public const string SettingsBaseUrlDescription = "Mermaid Chart API base URL";
        public const string SettingsAccessToken = "Access Token";
        public const string SettingsAccessTokenDescription = "Mermaid Chart API access token";
        public const string SettingsTokenHintRationale = "You can get token here: ";
        public const string SettingsTokenHintLink = $"{DefaultBaseUrl}/app/user/settings";

        public const string ProgressDiagramDownloading = "Diagram downloading";
        public const string ErrorDiagramDownloadingFailed = "Can't download file";
    }
}
