using RadEndpoints.Cli.Commands.GenerateEndpoint;
using RadEndpoints.Cli.Fonts;
using RadEndpoints.Cli.Helpers;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;


RadHelper.RenderLogo("RadEndpoints", FontConstants.FontPath);

var app = new CommandApp();
app.SetDefaultCommand<GenerateEndpointCommand>();

app.Configure(config =>
{
    config.SetApplicationName("RadEndpoints.Cli");
    config.SetApplicationVersion("1.0.0");
    config.AddExample(["generate", "endpoint", "GetUser"]);
    config.AddExample(["generate", "endpoint", "-f", "<DEFINITION_FILE>.json"]);


    config.AddBranch("generate", generate =>
    {
        generate
            .AddCommand<GenerateEndpointCommand>("endpoint")
            .WithAlias("e")
            .WithDescription("Generate an endpoint, request, response, and mapper class.")
            .WithExample(["generate", "endpoint", "GetUser"]);

    })
    .WithAlias("g");
});

app.Run(args);