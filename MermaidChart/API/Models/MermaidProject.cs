using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MermaidChart.API.Models
{
    internal class MermaidProject
    {
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("title")]
        public string Title;

        internal MermaidProject() { }

        internal MermaidProject(string id, string title)
        {
            this.Id = id;
            this.Title = title;
        }
    }
}
