using RadEndpoints.Cli.Helpers;
using Spectre.Console.Cli;
using System.Text.Json;

namespace RadEndpoints.Cli.Commands.GenerateEndpoint
{
    public sealed class GenerateEndpointCommand : Command<GenerateEndpointSettings>
    {
        public override int Execute(CommandContext context, GenerateEndpointSettings settings)
        {            
            if(File.Exists(settings.ImportPath))
            {
                DoBulkImport(settings.ImportPath);
                AnsiConsole.WriteLine();
                return 0;
            }
            RadHelper.WriteTitle("Endpoint Generator");
            AnsiConsole.WriteLine();

            List<GenerateEndpointSettings> generatedFileSettings = [];
            do
            {
                RunWizard(settings);
                CreateEndpointFiles(settings);
                AnsiConsole.WriteLine();
                RadHelper.WriteTitle("Code Generated");
                RenderFileTable(settings);

                generatedFileSettings.Add(settings.Clone());

            } while (ShouldCreateAnother(settings));

            MaybeDumpSettings(generatedFileSettings);
            AnsiConsole.WriteLine();
            return 0;
        }

        private void DoBulkImport(string importPath) 
        {
            var json = File.ReadAllText(importPath);
            var importedSettings = JsonSerializer.Deserialize<List<GenerateEndpointSettings>>(json);

            if(importedSettings is null || importedSettings.Count == 0)
            {
                RadHelper.WriteTitle($"Import Cancelled - No endpoint definitions found in {importPath}");
                return;
            }
            RadHelper.WriteTitle($"Definitions Found in {importPath}");
            RenderFileTable([.. importedSettings]);

            var proceed = PromptHelper.Confirm("Proceed with generation?", true);

            if (!proceed)
            {
                RadHelper.WriteTitle("Import Cancelled");
                return;
            }
            foreach (var s in importedSettings)
            {
                CreateEndpointFiles(s);
            }
            AnsiConsole.WriteLine();
            RadHelper.WriteTitle("Code Generation Complete");
        }

        private void CreateEndpointFiles(GenerateEndpointSettings s)
        {
            EnsureEndpointDirectory(s.EndpointName);
            GenerateEndpoint(s);
            GenerateModels(s);

            if (s.WithMapper)
            {
                GenerateMapper(s);
            }             
        }

        private static void RunWizard(GenerateEndpointSettings s)
        {
            s.BaseNamepace = PromptHelper.DefaultValueTextPrompt("Base Namespace", 3, s.BaseNamepace);

            s.ResourceName = PromptHelper.DefaultValueTextPrompt("Resource Name", 3, s.ResourceName); 
            
            s.ResourceName = s.ResourceName.UpperFirstCharOnly();            
            
            s.Verb = PromptHelper.ChoicePrompt("HTTP Verb", ["Get", "Post", "Put", "Patch", "Delete"]);            

            s.Path = s.Verb switch
            {   
                "Post" => PromptHelper.DefaultValueTextPrompt("Path", 3, $"/{s.ResourceName.ToLower()}s"),
                _ => PromptHelper.DefaultValueTextPrompt("Path", 3, $"/{s.ResourceName.ToLower()}s/{{id}}"),
            };

            var defaultEndpointName = s.Verb switch
            {
                "Post" => $"Create{s.ResourceName}",
                "Put" => $"Update{s.ResourceName}",
                "Patch" => $"Patch{s.ResourceName}",
                "Delete" => $"Delete{s.ResourceName}",
                _ => s.Path.Contains("{id}")
                    ? $"Get{s.ResourceName}"
                    : $"GetAll{s.ResourceName}s"
            };

            s.EndpointName = PromptHelper.DefaultValueTextPrompt("Endpoint Name", 3, defaultEndpointName);

            s.Tag = PromptHelper.DefaultValueTextPrompt("OpenAPI Doc Tag", 3, s.ResourceName);

            var defaultDescription = s.Verb switch
            {   
                "Post" => $"Create a new {s.ResourceName}",
                "Put" => $"Update a {s.ResourceName} by ID",
                "Patch" => $"Patch a {s.ResourceName} by ID",
                "Delete" => $"Delete a {s.ResourceName} by ID",
                _ => s.Path.Contains("{id}")
                    ? $"Get {s.ResourceName} by ID"
                    : $"Get all {s.ResourceName}s"
            };

            s.Description = PromptHelper.DefaultValueTextPrompt("OpenAPI Doc Description", 3, defaultDescription);
            s.WithMapper = PromptHelper.Confirm("Generate Mapper?", true);
        }

        private static void RenderFileTable(params GenerateEndpointSettings[] multipleSettings)
        {
            var table = new Table()
                .AddColumn("File")
                .AddColumn("Classes");

            foreach (var s in multipleSettings)
            {
                table
                .AddRow(
                    RadHelper.AddHighlights($"{s.EndpointName}Endpoint.cs"),
                    RadHelper.AddHighlights($"{s.EndpointName}Endpoint"))
                .AddRow(
                    RadHelper.AddHighlights($"{s.EndpointName}Models.cs"),
                    RadHelper.AddHighlights($"{s.EndpointName}Request -- {s.EndpointName}Response -- {s.EndpointName}Validator"));

                if (s.WithMapper)
                {
                    table.AddRow(
                        RadHelper.AddHighlights($"{s.EndpointName}Mapper.cs"),
                        RadHelper.AddHighlights($"{s.EndpointName}Mapper"));
                }
            }
            
            table
                .BorderColor(RadHelper.Theme.Secondary.Color)
                .Border(TableBorder.Rounded);

            AnsiConsole.Write(table);
        }

        private static void GenerateEndpoint(GenerateEndpointSettings s)
        {
            var templatePath = @"Templates\Endpoint.txt".GetAssemblyRootedPath();

            var formattedCode =  FileHelper
                .GetFileAsString(templatePath)
                .EscapeNonPlaceholderBraces()
                .FormatTemplate(s.EndpointName, s.BaseNamepace, s.Path, s.Verb, s.Tag, s.Description);

            var outputPath = Path
                .Combine(s.EndpointName, $"{s.EndpointName}Endpoint.cs")
                .GetCwdRootedPath();

            File.WriteAllText(outputPath, formattedCode);
        }

        private static void GenerateModels(GenerateEndpointSettings s)
        {
            var templatePath = @"Templates\Models.txt".GetAssemblyRootedPath();

            var formattedCode =  FileHelper
                .GetFileAsString(templatePath)
                .EscapeNonPlaceholderBraces()
                .FormatTemplate(s.EndpointName, s.BaseNamepace);

            var outputPath = Path
                .Combine(s.EndpointName, $"{s.EndpointName}Models.cs")
                .GetCwdRootedPath();

            File.WriteAllText(outputPath, formattedCode);
        }

        private static void GenerateMapper(GenerateEndpointSettings s)
        {
            var templatePath = @"Templates\Mapper.txt".GetAssemblyRootedPath();

            string formattedCode = FileHelper
                .GetFileAsString(templatePath)
                .EscapeNonPlaceholderBraces()
                .FormatTemplate(s.EndpointName, s.BaseNamepace);

            var outputPath = Path
                .Combine(s.EndpointName, $"{s.EndpointName}Mapper.cs")
                .GetCwdRootedPath();

            File.WriteAllText(outputPath, formattedCode);
        }

        private void EnsureEndpointDirectory(string featureNamespace)
        {
            var path = Path.Combine(featureNamespace.GetCwdRootedPath());

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static bool ShouldCreateAnother(GenerateEndpointSettings settings)
        {
            AnsiConsole.WriteLine();
            bool createAnother = PromptHelper.Confirm("Create another?", true);
            return createAnother;
        }

        private static void MaybeDumpSettings(List<GenerateEndpointSettings> s)
        {            
            AnsiConsole.WriteLine();
            var shouldDump = PromptHelper.Confirm("Save endpoint definitions to disk for later import?", false);

            if (!shouldDump) return;

            var json = JsonSerializer.Serialize(s, new JsonSerializerOptions { WriteIndented = true });
            var fileName = $"{s.First().ResourceName}Endpoints.json";
            var path = Path.Combine(fileName.GetCwdRootedPath());

            File.WriteAllText(path, json);
            AnsiConsole.WriteLine(); 
            RadHelper.WriteTitle($"Settings saved to {fileName}");
        }
    }
}
