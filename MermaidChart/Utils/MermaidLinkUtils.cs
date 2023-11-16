using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MermaidChart.Utils
{
    public class MermaidLink
    {
        public string DocumentId { get;  }
        public int Position { get; }
        public int Length { get; }

        public MermaidLink(string documentId, int position, int length)
        {
            DocumentId = documentId;
            Position = position;
            Length = length;
        }

        public String Key()
        {
            return DocumentId + ":" + Position + ":" + Length;
        }
    }

    internal static class MermaidLinkUtils
    {
        private static Regex regex = new Regex("\\[MermaidChart: ([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})]");

        public static IEnumerable<MermaidLink> FindLinks(string text)
        {
            foreach (var match in regex.Matches(text).Cast<Match>())
            {
                yield return new MermaidLink(match.Groups[1].Value, match.Index, match.Length);
            }
        }
    }
}
