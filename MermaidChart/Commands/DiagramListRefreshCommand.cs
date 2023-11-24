using MermaidChart.API;
using MermaidChart.API.Models;
using System.Linq;

namespace MermaidChart.Commands
{
    [Command(PackageGuids.MermaidChartCommandSetString, PackageIds.RefreshDiagramList)]
    internal sealed class DiagramListRefreshCommand : BaseCommand<DiagramListRefreshCommand>
    {
        public delegate void DiagramListRefreshHandler();
        public static event DiagramListRefreshHandler OnRefresh;

        protected override Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            OnRefresh();
            return Task.CompletedTask;
        }
    }
}
