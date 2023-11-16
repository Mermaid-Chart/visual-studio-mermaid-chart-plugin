using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MColor = System.Windows.Media.Color;
using DColor = System.Drawing.Color;


namespace MermaidChart.Utils
{
    internal static class ThemeUtils
    {
        public static MColor ToMediaColor(this DColor color)
        {
            return MColor.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
