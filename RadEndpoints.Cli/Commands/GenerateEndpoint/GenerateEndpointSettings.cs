using RadEndpoints.Cli.Helpers;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace RadEndpoints.Cli.Commands.GenerateEndpoint
{    
    public sealed class GenerateEndpointSettings : CommandSettings
    {
        public static class Defaults
        {
            public const string BaseNamespace = "Your.Project.Namespace.Endpoints";
            public const string ResourceName = "Resource";
            public const string Verb = "Get";
            public const string EndpointName = "GetResource";
            public const string Path = "/path/to/endpoint";
            public const string Entity = "Entity";
            public const string Tag = "Resource";
            public const string Description = "Add OpenApi Description Here";
        }

        [Description("The base namespace used for your generated classes.  The endpoint name will be added to this later.")]
        [CommandOption("-n|--namespace <NAMESPACE>")]
        [DefaultValue(Defaults.BaseNamespace)]
        public string BaseNamepace { get; set; } = string.Empty;

        [Description("The name of the resource or feature -- specify as singular! (Ex: User, Product, Order)")]
        [CommandOption("-r|--resource <RESOURCENAME>")]
        [DefaultValue(Defaults.ResourceName)]
        public string ResourceName { get; set; } = string.Empty;

        [Description("The http verb. Ex: Get, Post, Put, Patch, Delete")]
        [CommandOption("-v|--verb <VERB>")]
        [DefaultValue(Defaults.Verb)]
        public string Verb { get; set; } = string.Empty;

        [Description("The endpoint name.")]
        [CommandOption("-e|--endpoint <ENDPOINTNAME>")]
        [DefaultValue(Defaults.EndpointName)]
        public string EndpointName { get; set; } = string.Empty;

        [Description("The endpoint path. Ex: /user/{id}.")]
        [CommandOption("-p|--path <PATH>")]
        [DefaultValue(Defaults.Path)]
        public string Path { get; set; } = string.Empty;

        [Description("The domain/entity type to use in the mapper. Ex: UserEntity")]
        [CommandOption("--entity <ENTITYTYPE>")]
        [DefaultValue(Defaults.Entity)]
        public string Entity { get; set; } = string.Empty;

        [Description("The OpenApi tag to use Ex: \"Users\"")]
        [CommandOption("-t|--tag <TAG>")]
        [DefaultValue(Defaults.Tag)]
        public string Tag { get; set; } = string.Empty;

        [Description("The OpenApi description to use Ex: \"Get all users\"")]
        [CommandOption("-d|--desc <DESCRIPTION>")]
        [DefaultValue(Defaults.Description)]
        public string Description { get; set; } = string.Empty;

        [Description("Generate a mapping class")]
        [CommandOption("-m|--mapper")]
        [DefaultValue(true)]
        public bool WithMapper { get; set; }

        [JsonIgnore]
        [Description("Endpoint creation from file")]
        [CommandOption("-f|--file")]
        public string? ImportPath { get; set; }

        [JsonIgnore]
        [Description("Quick mode, skips all prompts and uses default values.  Reasonable defaults are calculated when verb and resource name are provided.")]        
        [CommandOption("-q|--quick")]
        [DefaultValue(false)]
        public bool QuickMode { get; set; }

        public override ValidationResult Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.Successful) return baseResult;
            
            Verb = Verb.UpperFirstCharOnly();
            EndpointName = Verb.UpperFirstCharOnly();
            ResourceName = ResourceName.UpperFirstCharOnly();
            Entity = Entity.UpperFirstCharOnly();
            Tag = Tag.UpperFirstCharOnly();

            return ValidateVerb();
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
            Entity = Entity,
            Tag = Tag,
            Description = Description,
            WithMapper = WithMapper
    };
}
}
