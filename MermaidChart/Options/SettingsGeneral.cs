using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using static MermaidChart.Options.OptionsProvider;

namespace MermaidChart.Options
{
    internal partial class OptionsProvider
    {
        [ComVisible(true)]
        public class SettingsGeneralPageOptions : BaseOptionPage<SettingsGeneral> { }
    }

    [ComVisible(true)]
    public class GeneralOptionPage : UIElementDialogPage
    {
        protected override UIElement Child
        {
            get
            {
                MermaidChart.UI.SettingsGeneralPageControl page = new MermaidChart.UI.SettingsGeneralPageControl
                {
                    generalOptionsPage = this
                };
                page.Initialize();
                return page;
            }
        }
    }

    public class SettingsGeneral : BaseOptionModel<SettingsGeneral>
    {
        [Category(Constants.SettingsCategoryGeneral)]
        [DisplayName(Constants.SettingsBaseUrl)]
        [Description(Constants.SettingsBaseUrlDescription)]
        [DefaultValue(Constants.DefaultBaseUrl)]
        public string BaseUrl { get; set; } = Constants.DefaultBaseUrl;

        [Category(Constants.SettingsCategoryGeneral)]
        [DisplayName(Constants.SettingsAccessToken)]
        [Description(Constants.SettingsAccessTokenDescription)]
        public string AccessToken { get; set; } = "";
    }
}
