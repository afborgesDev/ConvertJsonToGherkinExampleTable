using System.Collections.Generic;
using ConvertJsonToGherkinExampleTable.Core.TableConverter.Resolvers;

namespace ConvertJsonToGherkinExampleTable.Core.TableConverter
{
    internal static class Converter
    {
        public static TableConverterResult Convert(Dictionary<string, object> json, bool generateCode = false) =>
            TableConverterResult.FromHeaderAndFields(HeaderResolver.Resolve(json), FieldResolver.Resolve(json), generateCode);
    }
}
