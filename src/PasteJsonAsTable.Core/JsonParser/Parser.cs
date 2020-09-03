using Newtonsoft.Json;
using PasteJsonAsTable.Core.TableConverter;
using System;
using System.Collections.Generic;

namespace PasteJsonAsTable.Core.JsonParser
{
    public static class Parser
    {
        public const string CouldNotConvertJsonIntoTableMessage = "Could not convert the Json into a Table";

        public static string Parse(string payload)
        {
            if (string.IsNullOrEmpty(payload))
                return string.Empty;

            if (!TryParseIntoDynamicJson(payload, out var dynamicJson))
                return CouldNotConvertJsonIntoTableMessage;

            return Converter.Convert(dynamicJson);
        }

        private static bool TryParseIntoDynamicJson(string payload, out Dictionary<string, object> json)
        {
            json = default;
            try
            {
                json = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload);
                if (json.Count == 0)
                    return false;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}