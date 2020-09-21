using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConvertJsonToGherkinExampleTable.Core.JsonParser
{
    internal static class ParseToDictionaryHelper
    {
        private const int NoItems = 0;

        public static bool TryParseToDictionary(string? payload, out Dictionary<string, object> json)
        {
            json = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(payload))
                return false;

            try
            {
                json = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload);
                return json.Count > NoItems;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}
