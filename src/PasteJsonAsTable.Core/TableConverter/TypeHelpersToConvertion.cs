using System;
using Newtonsoft.Json.Linq;

namespace PasteJsonAsTable.Core.TableConverter
{
    public static class TypeHelpersToConvertion
    {
        public static bool IsJArray(this Type type) => type.IsAssignableFrom(typeof(JArray));
    }
}
