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
        public static CommentData GetCommentData(string language, string fileExt)
        {
            if(language.ToLower().Equals("plain text"))
            {
                switch(fileExt)
                {
                    case ".md":
                        language = "markdown";
                        break;
                    case ".yaml":
                    case ".yml":
                        language = "yaml";
                        break;
                    default:
                        break;
                }
            }

            switch (language.ToLower())
            {
                case "markdown":
                    return new CommentData("[//]: <>", "");
                case "xml":
                case "html":
                    return new CommentData("<!--", "-->");
                case "yaml":
                case "python":
                    return new CommentData("#", "");
                default:
                    return new CommentData("//", "");
            }
        }
    }
}
