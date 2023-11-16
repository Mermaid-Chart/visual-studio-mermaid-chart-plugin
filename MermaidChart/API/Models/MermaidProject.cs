using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MermaidChart.API.Models
{
    internal class MermaidProject
    {
        string id;
        string title;

        internal MermaidProject(string id, string title)
        {
            this.id = id;
            this.title = title;
        }
    }
}
