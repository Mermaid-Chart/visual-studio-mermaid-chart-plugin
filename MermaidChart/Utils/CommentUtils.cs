using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MermaidChart.Utils
{
    internal class CommentData
    {
        public string Prefix { get; set; }
        public string Postfix { get; set; }

        public CommentData(string prefix, string postfix)
        {
            Prefix = prefix;
            Postfix = postfix;
        }

        public string GetCommented(string text)
        {
            return $"{Prefix} {text} {Postfix}";
        }
    }

    internal class CommentUtils
    {
        public static CommentData GetCommentData(string language)
        {
            switch (language.ToLower())
            {
                case "xml":
                    return new CommentData("<!--", "-->");
                case "python":
                    return new CommentData("#", "");
                default:
                    return new CommentData("//", "");
            }
        }
    }
}
