using MermaidChart.API;
using MermaidChart.API.Models;
using System.Linq;

namespace MermaidChart.Commands
{
    [Command("512fc355-237a-4937-9805-cbdb25e11682", 0x0100)]
    internal sealed class TestAPICommand : BaseCommand<TestAPICommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            var client = new APIClient();
            var r = await client.GetProjectsAsync();
            r.Match(
                async left => await VS.MessageBox.ShowWarningAsync("TestAPICommand", $"Error"),
                async right => await VS.MessageBox.ShowWarningAsync("TestAPICommand", $"{(right[0] as MermaidProject).Id}")
            );
            
        }
    }
}
