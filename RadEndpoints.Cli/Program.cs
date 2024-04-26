using RadEndpoints.Cli.Commands.GenerateEndpoint;
using RadEndpoints.Cli.Fonts;
using RadEndpoints.Cli.Helpers;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;
using System.Net;


RadHelper.RenderLogo("RadEndpoints", FontConstants.FontPath);

var app = new CommandApp();

app.Configure(config =>
{  
    config.SetApplicationName("RadEndpoints.Cli");
    config.SetApplicationVersion("1.0.0");
    config.AddExample(["generate", "endpoint"]);
    config.AddExample(["g", "e"]);
    config.AddExample(["g", "e",  "--verb Post", "-r User", "-n MyProject.Endpoints", "-q"]);
    config.AddExample(["generate", "endpoint", "-f", "<DEFINITION_FILE>.json"]);


    config.AddBranch("generate", generate =>
    {
        generate
            .AddCommand<GenerateEndpointCommand>("endpoint")
            .WithAlias("e")
            .WithDescription("Generate an endpoint, request, response, and mapper class.")
            .WithExample(["generate", "endpoint"])
            .WithExample(["g", "e"])
            .WithExample(["g", "e", "--verb Post", "-r User", "-n MyProject.Endpoints", "-q"])
            .WithExample(["generate", "endpoint", "-f", "<DEFINITION_FILE>.json"]);

    })
    .WithAlias("g");
});

app.Run(args);