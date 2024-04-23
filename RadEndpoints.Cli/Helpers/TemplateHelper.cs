using System.Text.RegularExpressions;

namespace RadEndpoints.Cli.Helpers
{
    internal static class TemplateHelper
    {
        public static string EscapeNonPlaceholderBraces(this string templateString) =>
            Regex.Replace(templateString, @"(?<!\{[0-9])\{(?![0-9]\})|(?<!\{[0-9])\}(?![0-9]\})", m => m.Value + m.Value);

        public static string FormatTemplate(this string templateString, params string[] templateParamters) =>
            string.Format(templateString, templateParamters);

    }
}
