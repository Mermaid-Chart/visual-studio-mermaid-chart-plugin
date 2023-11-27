using MermaidChart.API;
using MermaidChart.Options;
using System.Windows;
using System.Windows.Controls;
using static System.Windows.Forms.LinkLabel;

namespace MermaidChart.UI
{
    public partial class SettingsGeneralPageControl : UserControl
    {
        internal GeneralOptionPage generalOptionsPage;

        public SettingsGeneralPageControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            SettingsGeneral.Instance.Save();
            tokenInput.Text = SettingsGeneral.Instance.AccessToken;
            baseUrlInput.Text = SettingsGeneral.Instance.BaseUrl;
            hintText.Visibility = String.IsNullOrEmpty(tokenInput.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void baseUrlInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            SettingsGeneral.Instance.BaseUrl = baseUrlInput.Text;
            SettingsGeneral.Instance.Save();
        }

        private void tokenInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            SettingsGeneral.Instance.AccessToken = tokenInput.Text;
            hintText.Visibility = String.IsNullOrEmpty(tokenInput.Text) ? Visibility.Visible : Visibility.Collapsed;
            SettingsGeneral.Instance.Save();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(Constants.SettingsTokenHintLink);
        }
    }
}
