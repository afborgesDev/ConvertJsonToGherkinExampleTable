using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PasteJsonAsTable.Core.TableConverter.Resolvers
{
    public static class JObjectResolver
    {
        public const string DefaultJoinSymbol = ".";

        public static string ResolveHeaders(string baseName, object item)
        {
            if (string.IsNullOrEmpty(baseName))
                throw new ArgumentNullException(nameof(baseName));

            ValidateEntry(item, out var token);
            var builder = new StringBuilder();
            foreach (JProperty interItem in token)
            {
                builder.Append(baseName)
                       .Append(DefaultJoinSymbol)
                       .Append(interItem.Name)
                       .Append(Converter.DefaultColumnSeparator);
            }

            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        private static void ValidateEntry(object item, out JToken token)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!(item is JToken convertedJToken))
                throw new NullReferenceException("Could not convert the item into JToken while resolving JObject");

            token = convertedJToken;
        }
    }
}
