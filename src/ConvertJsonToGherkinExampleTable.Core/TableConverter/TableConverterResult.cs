using System;
using System.Collections.Generic;
using System.Linq;
using ConvertJsonToGherkinExampleTable.Core.TableConverter.TableCodeGenerator;

namespace ConvertJsonToGherkinExampleTable.Core.TableConverter
{
    internal class TableConverterResult
    {
        public string? Headers { get; set; }
        public string? Fields { get; set; }
        public string? GeneratedCode { get; set; }

        public static TableConverterResult FromHeaderAndFields(string header, string field, bool generateCode = false)
        {
            var result = new TableConverterResult { Headers = header, Fields = field };
            if (generateCode)
                result.GeneratedCode = CodeGenerator.FromTableConverterResult(result);

            return result;
        }

        public static TableConverterResult FromHeaderAndListFields(string? header, IEnumerable<string?>? fields, bool generateCode = false)
        {
            var result = new TableConverterResult { Headers = header };
            if (generateCode)
                result.GeneratedCode = CodeGenerator.FromTableConverterResult(result);

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
