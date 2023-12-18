# RadEndpoints
A lightweight API framework that embraces the power of Net Core [Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio) using a well-defined [REPR](https://www.apitemplatepack.com/docs/introduction/repr-pattern/) Style (Request-Endpoint-Response) pattern.  The framework aims to sprinkle syntactical sugar over Minimal Apis to facilitate a consistent endpoint building / testing / delivery workflow. A fast and easy developer experience is the #1 goal of this project.

While the framework appears to have strong opinions on endpoint structure on the surface, all of the helper/conveniences are virtually optional. Full Minimal Api functionality is preserved for more uncommon edge case or custom use case scenarios.  As of now, this code is for experimental and educational purposes only. 

### Features:
#### Convenient Structured Endpoint Classes
- Less noisy configuration than original minimal api endpoints
- Constructor dependency injection
- Scoped lifetime
- Built-in HttpContext
- Built-in ILogger<EndpointName>
- Built-in IWebHostEnvironment
- Built in Response Object
- Built in Entity / Response Mapper (optional)
- IResult/TypedResult helpers
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
