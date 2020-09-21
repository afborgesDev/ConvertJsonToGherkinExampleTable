using System;
using System.Collections.Generic;
using System.Linq;
using ConvertJsonToGherkinExampleTable.Core.Common;
using ConvertJsonToGherkinExampleTable.Core.TableConverter.TableCodeGenerator;

namespace ConvertJsonToGherkinExampleTable.Core.TableConverter
{
    internal class TableConverterResult
    {
        public string? Headers { get; set; }
        public string? Fields { get; set; }
        public string? GeneratedCode { get; set; }

        public static TableConverterResult FromHeaderAndFields(string header, string field, bool generateCode = false) => FromHeaderAndfieldsInternal(header, field, generateCode);

        public static TableConverterResult FromHeaderAndListFields(string? header, IEnumerable<string?>? fields, bool generateCode = false)
        {
            var result = FromHeaderAndfieldsInternal(header, default, generateCode);
            result.Fields = fields.Aggregate((previous, current) => $"{previous}{Environment.NewLine}{TableConvertionConstants.DefaultColumnSeparator}{current}");
            return result;
        }

        public override string? ToString()
        {
            var convertionResult = JoinHeaderFiled().RemoveSeparatorDuplicaitons();
            if (!string.IsNullOrEmpty(GeneratedCode))
                convertionResult += $"{Environment.NewLine}**CODE:** {GeneratedCode}";
            return convertionResult;
        }

        private static TableConverterResult FromHeaderAndfieldsInternal(string? header, string? field, bool generateCode)
        {
            var result = new TableConverterResult { Headers = header, Fields = field };
            if (generateCode)
                result.GeneratedCode = CodeGenerator.FromTableConverterResult(result);
            return result;
        }

        private string JoinHeaderFiled() =>
            $"{TableConvertionConstants.DefaultColumnSeparator}{Headers}{Environment.NewLine}{TableConvertionConstants.DefaultColumnSeparator}{Fields}";
    }
}
