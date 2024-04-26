dotnet tool uninstall -g RadEndpoints.Cli
dotnet pack
dotnet tool install --global --add-source "C:\dev\src\MinimalApiPoc\RadEndpoints.Cli\nupkg" RadEndpoints.Cli --version 1.0.0