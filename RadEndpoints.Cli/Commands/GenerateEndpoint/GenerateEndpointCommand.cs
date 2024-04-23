using Newtonsoft.Json;
using RadEndpoints.Cli.Helpers;
using Spectre.Console.Cli;

namespace RadEndpoints.Cli.Commands.GenerateEndpoint
{
    public sealed class GenerateEndpointCommand : Command<GenerateEndpointSettings>
    {
        public override int Execute(CommandContext context, GenerateEndpointSettings settings)
        {
            RadHelper.WriteTitle("Endpoint Generator");
            AnsiConsole.WriteLine();

            List<GenerateEndpointSettings> generatedFileSettings = [];
            do 
            {
                RunWizard(settings);
                EnsureEndpointDirectory(settings.EndpointName);
                GenerateEndpoint(settings);
                GenerateModels(settings);

                if (settings.WithMapper)
                {
                    GenerateMapper(settings);
                }
                OutputResults(settings);
                generatedFileSettings.Add(settings.Clone());

            } while (ShouldCreateAnother(settings));

            MaybeDumpSettings(generatedFileSettings);
            
            return 0;
        }

        private static void MaybeDumpSettings(List<GenerateEndpointSettings> settings)
        {
            AnsiConsole.WriteLine();
            var shouldDump = PromptHelper.Confirm("Output JSON for later use?  You can later import a JSON file for bulk endpoint generation tasks.", false);

            if (!shouldDump) return;

            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            var path = Path.Combine($"{settings.First().ResourceName}Endpoints.json".GetAssemblyRootedPath());
            File.WriteAllText(path, json);
        }

        private static bool ShouldCreateAnother(GenerateEndpointSettings settings)
        {
            AnsiConsole.WriteLine();
            bool createAnother = PromptHelper.Confirm("Create another?", true);
            return createAnother;
        }

        private static void RunWizard(GenerateEndpointSettings settings)
        {
            settings.BaseNamepace = PromptHelper.DefaultValueTextPrompt("Base Namespace", 3, 
                string.IsNullOrEmpty(settings.BaseNamepace) 
                    ? "Your.Project.Namespace" 
                    : settings.BaseNamepace);

            settings.ResourceName = PromptHelper.DefaultValueTextPrompt("Resource Name", 3, 
                string.IsNullOrEmpty(settings.ResourceName) 
                    ? "Resource"
                    : settings.ResourceName);
            
            settings.ResourceName = settings.ResourceName.UpperFirstCharOnly();            
            
            settings.Verb = PromptHelper.ChoicePrompt("HTTP Verb", ["Get", "Post", "Put", "Patch", "Delete"]);            

            settings.Path = settings.Verb switch
            {   
                "Post" => PromptHelper.DefaultValueTextPrompt("Path", 3, $"/{settings.ResourceName.ToLower()}s"),
                "Put" => PromptHelper.DefaultValueTextPrompt("Path", 3, $"/{settings.ResourceName.ToLower()}s/{{id}}"),
                "Patch" => PromptHelper.DefaultValueTextPrompt("Path", 3, $"/{settings.ResourceName.ToLower()}s/{{id}}"),
                "Delete" => PromptHelper.DefaultValueTextPrompt("Path", 3, $"/{settings.ResourceName.ToLower()}s/{{id}}"),
                _ => PromptHelper.DefaultValueTextPrompt("Path", 3, $"/{settings.ResourceName.ToLower()}s/{{id}}"),
            };

            var defaultEndpointName = settings.Verb switch
            {
                "Post" => $"Create{settings.ResourceName}",
                "Put" => $"Update{settings.ResourceName}",
                "Patch" => $"Patch{settings.ResourceName}",
                "Delete" => $"Delete{settings.ResourceName}",
                _ => settings.Path.Contains("{id}")
                    ? $"Get{settings.ResourceName}"
                    : $"GetAll{settings.ResourceName}s"
            };

            settings.EndpointName = PromptHelper.DefaultValueTextPrompt("Endpoint Name", 3, defaultEndpointName);

            settings.Tag = PromptHelper.DefaultValueTextPrompt("OpenAPI Doc Tag", 3, settings.ResourceName);

            var defaultDescription = settings.Verb switch
            {   
                "Post" => $"Create a new {settings.ResourceName}",
                "Put" => $"Update a {settings.ResourceName} by ID",
                "Patch" => $"Patch a {settings.ResourceName} by ID",
                "Delete" => $"Delete a {settings.ResourceName} by ID",
                _ => settings.Path.Contains("{id}")
                    ? $"Get a {settings.ResourceName} by ID"
                    : $"Get all {settings.ResourceName}s"
            };

            settings.Description = PromptHelper.DefaultValueTextPrompt("OpenAPI Doc Description", 3, defaultDescription);
            settings.WithMapper = PromptHelper.Confirm("Generate Mapper?", true);
        }

        private static void OutputResults(GenerateEndpointSettings settings)
        {
            AnsiConsole.WriteLine();

            RadHelper.WriteTitle($"Files Generated");

            var table = new Table();

            table
                .AddColumn("File")
                .AddColumn("Classes")
                .AddRow(
                    RadHelper.AddHighlights($"{settings.EndpointName}Endpoint.cs"),
                    RadHelper.AddHighlights($"{settings.EndpointName}Endpoint"))
                .AddRow(
                    RadHelper.AddHighlights($"{settings.EndpointName}Models.cs"), 
                    RadHelper.AddHighlights($"{settings.EndpointName}Request * {settings.EndpointName}Response * {settings.EndpointName}RequestValidator"));

            if (settings.WithMapper)
            {                
                table.AddRow(
                    RadHelper.AddHighlights($"{settings.EndpointName}Mapper.cs"), 
                    RadHelper.AddHighlights($"{settings.EndpointName}Mapper"));                
            }
            table
                .BorderColor(RadHelper.Theme.Secondary.Color)
                .Border(TableBorder.Rounded);                

            AnsiConsole.Write(table);


            
        }

        private static void GenerateEndpoint(GenerateEndpointSettings settings)
        {
            var templatePath = @"Templates\Endpoint.txt".GetAssemblyRootedPath();

            var classString =  FileHelper
                .GetFileAsString(templatePath)
                .EscapeNonPlaceholderBraces()
                .FormatTemplate(settings.EndpointName, settings.BaseNamepace, settings.Path, settings.Verb, settings.Tag, settings.Description);

            var path = Path
                .Combine(settings.EndpointName, $"{settings.EndpointName}Endpoint.cs")
                .GetAssemblyRootedPath();

            File.WriteAllText(path, classString);
        }

        private static void GenerateModels(GenerateEndpointSettings settings)
        {
            var templatePath = @"Templates\Models.txt".GetAssemblyRootedPath();

            var classString =  FileHelper
                .GetFileAsString(templatePath)
                .EscapeNonPlaceholderBraces()
                .FormatTemplate(settings.EndpointName, settings.BaseNamepace);

            var path = Path
                .Combine(settings.EndpointName, $"{settings.EndpointName}Models.cs")
                .GetAssemblyRootedPath();

            File.WriteAllText(path, classString);
        }

        private static void GenerateMapper(GenerateEndpointSettings settings)
        {
            var templatePath = @"Templates\Mapper.txt".GetAssemblyRootedPath();

            var classString = FileHelper
                .GetFileAsString(templatePath)
                .EscapeNonPlaceholderBraces()
                .FormatTemplate(settings.EndpointName, settings.BaseNamepace);

            var path = Path
                .Combine(settings.EndpointName, $"{settings.EndpointName}Mapper.cs")
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
