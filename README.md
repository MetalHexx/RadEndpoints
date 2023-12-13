# RadEndpoints
A lightweight API framework that embraces the power of Net Core Minimal APIs using a well-defined REPR Style (Request-Endpoint-Response) pattern.  The framework aims to sprinkle syntactical sugar over Minimal Apis to facilitate a consistent endpoint building / testing / delivery workflow. A fast and easy developer experience is the #1 goal of this project.

While the framework has very strong opinions on endpoint structure, all of the helper/conveniences are virtually optional. Full minimal api functionality is preserved for more uncommon edge case scenarios or custom use cases.  As of now, this code is for experimental and educational purposes only. 

### Features:
#### Endpoint Base Classes with common conveniences
- HttpContext
- ILogger<EndpointName>
- IWebHostEnvironment
- CancellationToken
- Ready-to-send response Object
- Object Mapping (optional)
- Simple Endpoint Mapping Helpers
- Common TypedResult Helpers w/[ProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails?view=aspnetcore-8.0)
#### Automatic Endpoint Validation Filter
- Using [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
#### Integration Testing
- [WebApplicationFactory](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0) In-Memory Approach
- Strongly typed "Routeless" HttpClient RadEndpoint Extensions
- Clean and convenient response assertions for RadResponse / HttpResponse / [ProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails?view=aspnetcore-8.0) using [FluentAssertions](https://fluentassertions.com/introduction)
#### Example Api
- Lightweight api to demonstrate framework usage
- Includes example endpoints and integration tests
- Using feature based "vertical slice" style architecture (optional approach)
### Coming Soon:
- Open Telemetry Endpoint Filters (Logging, Metrics, Traces)
- Unit test coverage for framework code
- Project item and api project templates
- CLI Tool w/[Spectre.Console](https://spectreconsole.net/)
- Nuget package
- Demo for Example API: observability infrastructure using Grafana / Prometheus / Zipkin / Kibana
- Demo for Example API: Bogus oriented test mocking service
- Demo Ephemeral Test Environment w/[TestContainers](https://testcontainers.com/) 
- Documentation / How Tos

### Credits
This framework pays humble tribute to the [FastEndpoints](https://fast-endpoints.com/) as it borrows many syntactical and developer experience concepts from it.  Unless you're dead set on using Minimal APIs, I would highly recommend taking a look at that superior project which is actively maintained by a large community of contributors.
