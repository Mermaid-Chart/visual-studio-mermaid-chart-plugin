using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MermaidChart.UI
{
    [ComVisible(true)]
    public class OptionsPage: DialogPage
    {
        internal static string BaseUrl = "https://www.mermaidchart.com";
        private string accessToken = "";

        [Category("General")]
        [DisplayName("Base URL")]
        [Description("Mermaid Chart API base URL")]
        public string BaseUrlField
        {
            get { return cleanupUrl(BaseUrl); }
            set { BaseUrl = cleanupUrl(value); }
        }

        [Category("General")]
        [DisplayName("Access Token")]
        [Description("Mermaid Chart API access token")]
        public string AccessTokenField
        {
            get { return accessToken; }
            set { accessToken = value; }
        }


        private string cleanupUrl(string url)
        {
            var regEx = new Regex("/*$");
            return regEx.Replace(url, string.Empty);
        } 
    }
}
