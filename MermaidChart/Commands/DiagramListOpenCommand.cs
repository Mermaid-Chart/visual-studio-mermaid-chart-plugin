using System.ComponentModel.Design;
using MermaidChart.API;
using MermaidChart.EditorExtensions;

namespace MermaidChart.Commands
{
    [Command(PackageGuids.MermaidChartCommandSetString, PackageIds.OpenDiagramList)]
    internal sealed class DiagramListOpenCommand : BaseCommand<DiagramListOpenCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            var window = await VS.Windows.ShowToolWindowAsync(MermaidDiagramListWindow.guid);
            if (window == null)
            {
                throw new NotSupportedException("Cannot create tool window");
            }

        }
    }
}
