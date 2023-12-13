# RadEndpoints
A lightweight API framework that embraces the power of Net Core Minimal APIs using a well-defined REPR Style (Request-Endpoint-Response) endpoint pattern.  

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
- Common TypedResult Helpers w/ProblemDetails
#### Automatic Endpoint Validation Filter
- Using FluentValidations
#### Integration Testing
- WebApplicationFactory Fixture
- Routeless HttpClient Extentions
- Custom response validators using FluentAssertions
#### Planned Features:
- Open Telemetry Endpoint Filters (Logging, Metrics, Traces)

### How to (coming soon!):

### Credits
This framework pays humble tribute to the [FastEndpoints](https://fast-endpoints.com/) as it borrows many syntactical and developer experience concepts from it.  Unless you're dead set on using Minimal APIs, I would highly recommend taking a look at that superior project which is actively maintained by a large community of contributors.
