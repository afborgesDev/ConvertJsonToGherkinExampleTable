using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace PasteJsonAsTable.Core.TableConverter
{
    public static class Converter
    {
        public static string Convert(Dictionary<string, object> json)
        {
            var headers = json.Select(x => x.Key)
                                        .Aggregate((previous, current) => $"{previous}|{current}|");

            //var fields = json.Select(x => x.Value.ToString())
            //                           .Aggregate((previous, current) => $"{previous}|{current}|").ToString();

            var fields = BuildFields(json);
            return $"|{headers}{Environment.NewLine}|{fields}";
        }

        private static string BuildFields(Dictionary<string, object> json) =>
            json.Select(x => ConvertJsonValueToString(x)).Aggregate((previous, current) => $"{previous}|{current}|").ToString();

        private static string ConvertJsonValueToString(KeyValuePair<string, object> jsonField)
        {
            if (!jsonField.Value.GetType().IsJArray())
                return jsonField.Value.ToString();

            return ReduceJArray(jsonField.Value);
        }

        private static string ReduceJArray(object jsonValue)
        {
            var array = (JArray)jsonValue;
            return array.Aggregate((previous, current) => $"{previous},{current}").ToString();
        }
    }
}
