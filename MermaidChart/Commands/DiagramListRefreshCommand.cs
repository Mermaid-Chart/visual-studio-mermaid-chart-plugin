using MermaidChart.API;
using MermaidChart.API.Models;
using System.Linq;

namespace MermaidChart.Commands
{
    [Command("512fc355-237a-4937-9805-cbdb25e11682", 0x0100)]
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
