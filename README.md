# RadEndpoints
An API library bringing a [REPR](https://www.apitemplatepack.com/docs/introduction/repr-pattern/) Style (Request-Endpoint-Response) endpoint classes to .NET [Minimal API](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio).

### Library Goals
_Should be:_
- Lightweight -- and easy to work with or without.
- Junior Developer Friendly -- without impeding more experienced engineers.
- Fully Minimal API compatible.
- [Configurable](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/route-handlers?view=aspnetcore-8.0#route-handlers) using a MinimalApi [RouteHandlerBuilder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.routehandlerbuilder).
- Well structured -- for common tasks such as validation, mapping and error handling.
- Extensible -- to allow for custom alternate endpoint implmentations.
- Fast and Easy -- to rapidly scaffold projects, endpoints and tests.
- Low Maintenance -- for testing.

### Features:
#### REPR Endpoint Classes
- Reduced configuration noise over minimal api endpoints
- Constructor dependency injection
- Scoped lifetime
- Assembly scanned and configured request validator and model mapper
- Built-in Endpoint Class Conveniences
  - HttpContext
  - Logger
  - Environment Info
  - Response Object
  - Model Mapper 
  - TypedResult Shortcuts

```csharp
public class GetSampleEndpoint(ISampleService sampleService) : RadEndpoint<GetSampleRequest, GetSampleResponse, GetSampleMapper>
{
    public override void Configure()
    {
        Get("/samples/{id}")
            .Produces<GetSampleResponse>(StatusCodes.Status200OK)            
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .WithDocument(tag: "Sample", desc: "Get Sample by ID");

            //Any NET minimal api (RouteHandlerBuilder) configuration works here.
    }

    public override async Task Handle(GetSampleRequest r, CancellationToken ct)
    {
        var sample = await sampleService.GetSampleById(r.Id, ct);

        if(sample is null)
        {
            SendNotFound("Sample not found.");
            return;
        }
        Response = Map.FromEntity(sample);
        Send();
    }
}
```
  
#### Integration Testing
- Strongly typed "Routeless" HttpClient extensions
- Reduced maintenance with automatic endpoint route discovery and request model binding
- Easy to navigate to endpoint code from test
- Consistent and convenient response assertions for HttpResponse and [ProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails?view=aspnetcore-8.0) using [FluentAssertions](https://fluentassertions.com/introduction)
- Detailed exception messages so you dig less to find test issues.

```csharp
[Fact]
public async void When_RequestValid_ReturnsSuccess()
{
    //Arrange
    var request = new GetSampleRequest { Id = 1 };

    //Act       
    var r = await f.Client.GetAsync<GetSampleEndpoint, GetSampleRequest, GetSampleResponse>(request);

    //Assert
    r.Should()
        .BeSuccessful<GetSampleResponse>()
        .WithStatusCode(HttpStatusCode.OK);
}
```

#### Request Model Binding and Validation
- Automatic request model binding from route, query, header, and body using [AsParameters].
- Automatic request model validation execution using [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
```csharp
public class GetSampleRequest
{
    [FromRoute]
    public int Id { get; set; }
}

public class GetSampleRequestValidator : AbstractValidator<GetSampleRequest>
{
    public GetSampleRequestValidator()
    {
        RuleFor(e => e.Id).GreaterThan(0);
    }
}

public class GetSampleResponse
{
    public SampleDto Data { get; set; } = null!;
    public string Message { get; set; } = "Sample retrieved successfully";
}
```
#### CLI For Scaffolding
- Scaffold multiple new endpoints very quickly
- Bulk endpoint scaffolding with JSON definition
  
### Coming Soon:
- Project templates
- Observability Tooling
- Additional code coverage
- Documentation / How Tos

### Credits
- [FastEndpoints](https://fast-endpoints.com/) -- as many of the ideas from that project inspired this one.
