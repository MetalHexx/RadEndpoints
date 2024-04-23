using Newtonsoft.Json;
using RadEndpoints.Cli.Helpers;
using Spectre.Console.Cli;

namespace RadEndpoints.Cli.Commands.GenerateEndpoint
{
    public sealed class GenerateEndpointCommand : Command<GenerateEndpointSettings>
    {
        public override int Execute(CommandContext context, GenerateEndpointSettings settings)
        {            
            AnsiConsole.WriteLine();

            if(File.Exists(settings.ImportPath))
            {
                var json = File.ReadAllText(settings.ImportPath);
                var importedSettings = JsonConvert.DeserializeObject<List<GenerateEndpointSettings>>(json)!;

                RadHelper.WriteTitle("Files To Import");
                RenderFileTable([.. importedSettings]);

                var proceed = PromptHelper.Confirm("Proceed with import?", true);

                if (!proceed) 
                {
                    RadHelper.WriteHorizonalRule("Import Cancelled", Justify.Left);
                    return 0;
                }
                RadHelper.WriteHorizonalRule("Starting Import", Justify.Left);

                foreach (var s in importedSettings)
                {
                    CreateEndpointFiles(s);
                }
                AnsiConsole.WriteLine();
                RadHelper.WriteHorizonalRule("Import Completed", Justify.Left);
                return 0;
            }



            MultipleManualCreation(settings);

            return 0;
        }

        private void MultipleManualCreation(GenerateEndpointSettings settings)
        {
            RadHelper.WriteTitle("Endpoint Generator");

            List<GenerateEndpointSettings> generatedFileSettings = [];
            do
            {
                RunWizard(settings);
                CreateEndpointFiles(settings);
                
                generatedFileSettings.Add(settings.Clone());

            } while (ShouldCreateAnother(settings));

            MaybeDumpSettings(generatedFileSettings);
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
            OutputResults(s);
        }

        private static void MaybeDumpSettings(List<GenerateEndpointSettings> s)
        {
            AnsiConsole.WriteLine();
            var shouldDump = PromptHelper.Confirm("Output JSON for later use?  You can later import a JSON file for bulk endpoint generation tasks.", false);

            if (!shouldDump) return;

            var json = JsonConvert.SerializeObject(s, Formatting.Indented);
            var path = Path.Combine($"{s.First().ResourceName}Endpoints.json".GetAssemblyRootedPath());
            File.WriteAllText(path, json);
        }

        private static bool ShouldCreateAnother(GenerateEndpointSettings settings)
        {
            AnsiConsole.WriteLine();
            bool createAnother = PromptHelper.Confirm("Create another?", true);
            return createAnother;
        }

        private static void RunWizard(GenerateEndpointSettings s)
        {
            s.BaseNamepace = PromptHelper.DefaultValueTextPrompt("Base Namespace", 3, 
                string.IsNullOrEmpty(s.BaseNamepace) 
                    ? "Your.Project.Namespace" 
                    : s.BaseNamepace);

            s.ResourceName = PromptHelper.DefaultValueTextPrompt("Resource Name", 3, 
                string.IsNullOrEmpty(s.ResourceName) 
                    ? "Resource"
                    : s.ResourceName);
            
            s.ResourceName = s.ResourceName.UpperFirstCharOnly();            
            
            s.Verb = PromptHelper.ChoicePrompt("HTTP Verb", ["Get", "Post", "Put", "Patch", "Delete"]);            

            s.Path = s.Verb switch
            {   
                "Post" => PromptHelper.DefaultValueTextPrompt("Path", 3, $"/{s.ResourceName.ToLower()}s"),
                "Put" => PromptHelper.DefaultValueTextPrompt("Path", 3, $"/{s.ResourceName.ToLower()}s/{{id}}"),
                "Patch" => PromptHelper.DefaultValueTextPrompt("Path", 3, $"/{s.ResourceName.ToLower()}s/{{id}}"),
                "Delete" => PromptHelper.DefaultValueTextPrompt("Path", 3, $"/{s.ResourceName.ToLower()}s/{{id}}"),
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
                    ? $"Get a {s.ResourceName} by ID"
                    : $"Get all {s.ResourceName}s"
            };

            s.Description = PromptHelper.DefaultValueTextPrompt("OpenAPI Doc Description", 3, defaultDescription);
            s.WithMapper = PromptHelper.Confirm("Generate Mapper?", true);
        }

        private static void OutputResults(GenerateEndpointSettings s)
        {
            AnsiConsole.WriteLine();
            RadHelper.WriteTitle($"Files Generated");
            RenderFileTable(s);
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
                    RadHelper.AddHighlights($"{s.EndpointName}Request * {s.EndpointName}Response * {s.EndpointName}RequestValidator"));

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

            var classString =  FileHelper
                .GetFileAsString(templatePath)
                .EscapeNonPlaceholderBraces()
                .FormatTemplate(s.EndpointName, s.BaseNamepace, s.Path, s.Verb, s.Tag, s.Description);

            var path = Path
                .Combine(s.EndpointName, $"{s.EndpointName}Endpoint.cs")
                .GetAssemblyRootedPath();

            File.WriteAllText(path, classString);
        }

        private static void GenerateModels(GenerateEndpointSettings s)
        {
            var templatePath = @"Templates\Models.txt".GetAssemblyRootedPath();

            var classString =  FileHelper
                .GetFileAsString(templatePath)
                .EscapeNonPlaceholderBraces()
                .FormatTemplate(s.EndpointName, s.BaseNamepace);

            var path = Path
                .Combine(s.EndpointName, $"{s.EndpointName}Models.cs")
                .GetAssemblyRootedPath();

            File.WriteAllText(path, classString);
        }

        private static void GenerateMapper(GenerateEndpointSettings s)
        {
            var templatePath = @"Templates\Mapper.txt".GetAssemblyRootedPath();

            var classString = FileHelper
                .GetFileAsString(templatePath)
                .EscapeNonPlaceholderBraces()
                .FormatTemplate(s.EndpointName, s.BaseNamepace);

            var path = Path
                .Combine(s.EndpointName, $"{s.EndpointName}Mapper.cs")
                .GetAssemblyRootedPath();

            File.WriteAllText(path, classString);
        }

        private void EnsureEndpointDirectory(string featureNamespace)
        {
            var path = Path.Combine(featureNamespace.GetAssemblyRootedPath());

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
