using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MermaidChart.API.Converters
{
    internal class MermaidAPIDateTimeConverter: IsoDateTimeConverter
    {
        public MermaidAPIDateTimeConverter() {
            //2023-11-17T10:01:33.579Z
            base.DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffZ";
        }
    }
}
