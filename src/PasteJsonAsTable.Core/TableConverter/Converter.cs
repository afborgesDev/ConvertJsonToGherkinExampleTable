using System;
using System.Collections.Generic;
using System.Linq;

namespace PasteJsonAsTable.Core.TableConverter
{
    public static class Converter
    {
        public static string Convert(Dictionary<string, object> json)
        {
            var headers = json.Select(x => x.Key)
                                        .Aggregate((previous, current) => $"{previous}|{current}|");

            var fields = json.Select(x => x.Value.ToString())
                                       .Aggregate((previous, current) => $"{previous}|{current}|").ToString();

            return $"|{headers}{Environment.NewLine}|{fields}";
        }
    }
}