using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpIpp.Protocol.Extensions
{
    internal static class StringExtensions
    {
        public static string ConvertDashToCamelCase(this string input)
        {
            return string.Join("", input.Split('-').Select(x => x.First().ToString().ToUpper() + x.Substring(1).ToLower()));
        }

        public static string ConvertCamelCaseToDash(this string input)
        {
            return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) && (char.IsLower(input[i - 1]) || i < input.Length - 1 && char.IsLower(input[i + 1]))
                ? "-" + x
                : x.ToString())).ToLowerInvariant();
        }
    }
}