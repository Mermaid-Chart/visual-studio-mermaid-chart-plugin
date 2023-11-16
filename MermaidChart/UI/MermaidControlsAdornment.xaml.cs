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
using System.Windows.Shapes;
using MermaidChart.API;
using MermaidChart.Utils;

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
        }
    }
}
