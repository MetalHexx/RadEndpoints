using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadEndpoints.Cli.Helpers
{
    internal static class StringHelper
    {
        public static string UpperFirstCharOnly(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            input = input.ToLower();
            input = input[..1].ToUpper() + input[1..];
            return input;
        }
    }
}
