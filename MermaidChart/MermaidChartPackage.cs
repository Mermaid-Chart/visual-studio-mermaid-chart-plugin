global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using MermaidChart.UI;
using System.Runtime.InteropServices;
using System.Threading;

namespace MermaidChart
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.MermaidChartString)]
    [ProvideOptionPage(typeof(OptionsProvider.SettingsGeneralPageOptions), "MermaidChart", "General", 0, 0, true, SupportsProfiles = true)]
    public sealed class MermaidChartPackage : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await TestAPICommand.InitializeAsync(this);
        }
    }
}