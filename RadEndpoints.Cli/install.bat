dotnet tool uninstall -g RadEndpoints.Cli
dotnet pack
dotnet tool install --global --add-source .\nupkg RadEndpoints.Cli