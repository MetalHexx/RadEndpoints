using Spectre.Console.Cli;
using System.ComponentModel;

namespace RadEndpoints.Cli.Commands.GenerateEndpoint
{
    public sealed class GenerateEndpointSettings : CommandSettings
    {
        [Description("The base namespace used for your generated classes.  The endpoint name will be added to this later.")]
        [CommandOption("--namespace <NAMESPACE>")]
        [DefaultValue("Your.Project.Namespace.Endpoints")]
        public string BaseNamepace { get; set; } = string.Empty;

        [Description("The name of the resource or feature -- specify as singular! (Ex: User, Product, Order)")]
        [CommandOption("--resource <RESOURCENAME>")]
        [DefaultValue("Resource")]
        public string ResourceName { get; set; } = string.Empty;

        [Description("The http verb. Ex: Get, Post, Put, Patch, Delete")]
        [CommandOption("--verb <VERB>")]
        [DefaultValue("Get")]
        public string Verb { get; set; } = string.Empty;

        [Description("The endpoint name")]
        [CommandOption("--endpoint <ENDPOINTNAME>")]
        [DefaultValue("New")]
        public string EndpointName { get; set; } = string.Empty;

        [Description("The endpoint path. Ex: /user/{id}.")]
        [CommandOption("--path <PATH>")]
        [DefaultValue("/path/to/endpoint")]
        public string Path { get; set; } = string.Empty;        

        [Description("The OpenApi tag to use Ex: \"Users\"")]
        [CommandOption("--tag <TAG>")]
        [DefaultValue("Add OpenApi Tag Here")]
        public string Tag { get; set; } = string.Empty;

        [Description("The OpenApi description to use Ex: \"Get all users\"")]
        [CommandOption("--desc <DESCRIPTION>")]
        [DefaultValue("Add OpenApi Description Here")]
        public string Description { get; set; } = string.Empty;

        [Description("Generate a mapping class")]
        [CommandOption("--mapper")]
        [DefaultValue(true)]
        public bool WithMapper { get; set; }

        [Description("Endpoint creation from file")]
        [CommandOption("-f|--file")]
        public string? ImportPath { get; set; }




        public override ValidationResult Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.Successful) return baseResult;
            
            Verb = EnsureBeginsWithUppercase(Verb);
            EndpointName = EnsureBeginsWithUppercase(EndpointName);
            ResourceName = EnsureBeginsWithUppercase(ResourceName);
            Tag = EnsureBeginsWithUppercase(Tag);

            return ValidateVerb();
        }

        private string EnsureBeginsWithUppercase(string value)
        {
            return value[..1].ToUpper() + value[1..];
        }

        private ValidationResult ValidateVerb() 
        {
            var converter = TypeDescriptor.GetConverter(typeof(SupportedVerbs));

            if (!converter.IsValid(Verb))
            {
                return ValidationResult.Error("Invalid HTTP Verb");
            }
            return ValidationResult.Success();
        }

        public GenerateEndpointSettings Clone() => new() {
            BaseNamepace = BaseNamepace,
            ResourceName = ResourceName,
            Verb = Verb,
            EndpointName = EndpointName,
            Path = Path,
            Tag = Tag,
            Description = Description,
            WithMapper = WithMapper
    };
}
}
