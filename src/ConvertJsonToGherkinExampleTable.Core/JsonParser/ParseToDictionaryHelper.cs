using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConvertJsonToGherkinExampleTable.Core.JsonParser
{
    internal static class ParseToDictionaryHelper
    {
        public static bool TryParseToDictionary(string payload, out Dictionary<string, object> json)
        {
            json = default;

            if (string.IsNullOrEmpty(payload))
                return false;

            try
            {
                json = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload);
                if (json.Count == 0)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
