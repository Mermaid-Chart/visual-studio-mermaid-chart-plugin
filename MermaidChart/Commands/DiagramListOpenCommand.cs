using System.ComponentModel.Design;
using MermaidChart.API;
using MermaidChart.EditorExtensions;

namespace MermaidChart.Commands
{
    [Command("512fc355-237a-4937-9805-cbdb25e11682", 4129)]
    internal sealed class DiagramListOpenCommand : BaseCommand<DiagramListOpenCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            var window = await VS.Windows.ShowToolWindowAsync(new Guid("da7a0845-9354-4cb7-aa80-2d2d9399fb44"));
            if (window == null)
            {
                throw new NotSupportedException("Cannot create tool window");
            }

        }
    }
}
