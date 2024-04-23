﻿using Spectre.Console;

namespace RadEndpoints.Cli.Helpers
{
    /// <summary>
    /// Theme is used to keep colors consistent and easy to change in the cli
    /// </summary>
    public record MarkupTheme(MarkupColor Primary, MarkupColor Secondary, MarkupColor Highlight, MarkupColor Error);
}