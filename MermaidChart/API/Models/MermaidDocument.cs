using MermaidChart.API.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MermaidChart.API.Models
{
    internal class MermaidDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("major")]
        public int Major { get; set; }
        [JsonProperty("minor")]
        public int Minor { get; set; }
        [JsonProperty("documentId")]
        public string DocumentId { get; set; }
        [JsonConverter(typeof(MermaidAPIDateTimeConverter))]
        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; } //TODO: Fix JSON parsing

        internal MermaidDocument() { }

        internal MermaidDocument(string id, string title, int major, int minor, string documentId, DateTime updatedAt)
        {
            this.Id = id;
            this.Title = title;
            this.Major = major;
            this.Minor = minor;
            this.DocumentId = documentId;
            this.UpdatedAt = updatedAt;
        }
    }
}
