global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using MermaidChart.UI;
using MermaidChart.Commands;
using MermaidChart.EditorExtensions;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell.Interop;
using Constants = EnvDTE.Constants;
using Microsoft.VisualStudio;

namespace MermaidChart
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.MermaidChartString)]
    [ProvideOptionPage(typeof(OptionsProvider.SettingsGeneralPageOptions), "MermaidChart", "General", 0, 0, true, SupportsProfiles = true)]
    [ProvideToolWindow(typeof(MermaidDiagramListWindow), Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Right, MultiInstances = false, Window = WindowGuids.Toolbox)]
    [ProvideToolWindowVisibility(typeof(MermaidDiagramListWindow), UIContextGuids.SolutionExists)]
    public sealed class MermaidChartPackage : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await DiagramListRefreshCommand.InitializeAsync(this);
            await DiagramListOpenCommand.InitializeAsync(this);
        }
    }
}