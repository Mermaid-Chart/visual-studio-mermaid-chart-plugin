using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MermaidChart.API.Models
{
    internal class ProjectWithDocuments
    {
        internal MermaidProject Project { get; }
        internal List<MermaidDocument> Documents { get; }

        public ProjectWithDocuments(MermaidProject project, List<MermaidDocument> documents)
        {
            Project = project;
            Documents = documents;
        }
    }
}
