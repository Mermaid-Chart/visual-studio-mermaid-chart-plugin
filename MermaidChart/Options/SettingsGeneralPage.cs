using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace MermaidChart.UI
{
    internal partial class OptionsProvider
    {
        [ComVisible(true)]
        public class SettingsGeneralPageOptions : BaseOptionPage<SettingsGeneralPage> { }
    }

    public class SettingsGeneralPage : BaseOptionModel<SettingsGeneralPage>
    {
        private const string DefaultUrl = "https://www.mermaidchart.com";

        [Category("General")]
        [DisplayName("Base URL")]
        [Description("Mermaid Chart API base URL")]
        [DefaultValue(DefaultUrl)]
        public string BaseUrl { get; set; } = DefaultUrl;

        [Category("General")]
        [DisplayName("Access Token")]
        [Description("Mermaid Chart API access token")]
        public string AccessToken { get; set; } = "";


        private string cleanupUrl(string url)
        {
            var regEx = new Regex("/*$");
            return regEx.Replace(url, string.Empty);
        }
    }
}
