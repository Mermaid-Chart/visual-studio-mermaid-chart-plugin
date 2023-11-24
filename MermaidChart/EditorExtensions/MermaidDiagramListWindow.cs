using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using MermaidChart.UI;

namespace MermaidChart.EditorExtensions
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid(MermaidDiagramListWindow.guidString)]
    public class MermaidDiagramListWindow : ToolWindowPane
    {
        public const string guidString = "da7a0845-9354-4cb7-aa80-2d2d9399fb44";
        public static Guid guid = new Guid(guidString);
        public MermaidDiagramListWindow() : base(null)
        {
            this.Caption = Constants.DiagramListWindowCaption;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new MermaidDiagramListWindowControl();

            this.ToolBar = new System.ComponentModel.Design.CommandID(PackageGuids.DiagramListWindowCmdSet, PackageIds.DiagramListToolbar);
        }
    }
}
