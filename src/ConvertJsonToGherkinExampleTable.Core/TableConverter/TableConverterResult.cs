using System;
using System.Collections.Generic;
using System.Linq;

namespace ConvertJsonToGherkinExampleTable.Core.TableConverter
{
    internal class TableConverterResult
    {
        public string? Headers { get; set; }
        public string? Fields { get; set; }

        public static TableConverterResult FromHeaderAndFields(string header, string field) => new TableConverterResult { Headers = header, Fields = field };

        public static TableConverterResult FromHeaderAndListFields(string? header, IEnumerable<string?>? fields)
        {
            var result = new TableConverterResult { Headers = header };
            result.Fields = fields.Aggregate((previous, current) => $"{previous}{Environment.NewLine}{TableConvertionConstants.DefaultColumnSeparator}{current}");
            return result;
        }

        public override string? ToString() =>
            CleanCovertedText($"{TableConvertionConstants.DefaultColumnSeparator}{Headers}{Environment.NewLine}{TableConvertionConstants.DefaultColumnSeparator}{Fields}");

        private static string CleanCovertedText(string text)
        {
            var temp = text.Replace($"{TableConvertionConstants.DefaultColumnSeparator}{TableConvertionConstants.DefaultColumnSeparator}",
                                       TableConvertionConstants.DefaultColumnSeparator, StringComparison.InvariantCultureIgnoreCase);

            return temp.Replace($"{TableConvertionConstants.DefaultColumnSeparator}{TableConvertionConstants.DefaultColumnSeparator}",
                                   TableConvertionConstants.DefaultColumnSeparator, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
