# RadEndpoints CLI
_The RadEndpoints CLI tool will help scaffold endpoints very rapidly with a single command, guided wizard or in bulk with a JSON definition._

## Installing RadEndpoints CLI

#### Installing from NuGet
- **From the command line run:** `dotnet tool install -g RadEndpoints.Cli`


#### Installing from Source
- **From the /RadEndpoint.Cli folder run:** `install.bat` 
- Manual Instructions:
  - If installed, unintall it: `dotnet tool uninstall -g RadEndpoints.Cli`
  - Rebuild the CLI code and package it: `dotnet pack`
  - Install: `dotnet tool install --global --add-source .\nupkg RadEndpoints.Cli`

#### Uninstalling RadEndpoints CLI
- **From the /RadEndpoint.Cli folder run:** `uninstall.bat` 
- Manual Instructions:
  - `dotnet tool uninstall -g RadEndpoints.Cli`

### Generating Endpoints With Wizard

- Navigate to the project folder where you want to generate the endpoints

#### Generate Endpoint with Wizard

- `rad generate endpoint` to run the guided endpoint wizard
- You can save the endpoint definition to a .json file for later use

#### Generate Endpoints with JSON Definition File
- `rad generate endpoint -i <input file>` to run the endpoint generation tool with an endpoint definition json file        
- Use this format in the JSON file:
```json
[
  {
    "BaseNamepace": "Your.Project.Namespace.Endpoints",
    "ResourceName": "Resource",
    "Verb": "Get",
    "EndpointName": "GetResource",
    "Path": "/resources/{id}",
    "Tag": "Resource",
    "Description": "Get Resource by ID",
    "WithMapper": true,
    "ImportPath": null
  },
  ...add more endpoints here
]
```