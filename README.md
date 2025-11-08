# RadEndpoints
An API library bringing [REPR](https://www.apitemplatepack.com/docs/introduction/repr-pattern/)-style endpoint classes to .NET [Minimal API](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio).

### Library Goals
_Should be:_
- Lightweight -- and easy to work with or without.
- Junior Developer Friendly -- without impeding more experienced engineers.
- Backward Compatible -- with Minimal API configuration and features.
- Configurable -- using MinimalApi [RouteHandlerBuilder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.routehandlerbuilder).
- Well Structured -- for common tasks such as validation, mapping and error handling.
- Extensible -- to allow for custom alternate endpoint implmentations.
- Fast and Easy -- to rapidly scaffold projects, endpoints and tests.

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

#### Endpoint Base Classes
RadEndpoints provides three base classes to support different endpoint scenarios:

**`RadEndpoint<TRequest, TResponse>`** - Standard endpoint with request and response
```csharp
public class GetUserEndpoint : RadEndpoint<GetUserRequest, GetUserResponse>
{
    public override void Configure() => Get("/users/{id}");
    
    public override async Task Handle(GetUserRequest req, CancellationToken ct)
    {
        // Access request via req parameter
        Response = new GetUserResponse { Id = req.Id, Name = "John" };
        Send();
    }
}
```

**`RadEndpointWithoutRequest<TResponse>`** - Endpoint without request parameters
```csharp
public class GetHealthEndpoint : RadEndpointWithoutRequest<HealthResponse>
{
    public override void Configure() => Get("/health");
    
    public override async Task Handle(CancellationToken ct)
    {
        // No request parameter needed
        Response = new HealthResponse { Status = "Healthy" };
        Send();
    }
}
```

**`RadEndpoint<TRequest, TResponse, TMapper>`** - Endpoint with integrated mapper
```csharp
public class GetUserEndpoint : RadEndpoint<GetUserRequest, GetUserResponse, UserMapper>
{
    public override void Configure() => Get("/users/{id}");
    
    public override async Task Handle(GetUserRequest req, CancellationToken ct)
    {
        var user = await userService.GetById(req.Id);
        Response = Map.FromEntity(user); // Use integrated mapper
        Send();
    }
}
```

All base classes provide:
- Constructor dependency injection (scoped lifetime)
- Built-in conveniences: `HttpContext`, `Logger`, `Env`, `Response`
- TypedResult shortcuts: `Send()`, `SendNotFound()`, `SendProblem()`, etc.
- Automatic validation via FluentValidation

#### Response Methods
RadEndpoints provides strongly-typed response methods that return ASP.NET Core TypedResults:

**Success Responses**
```csharp
// 200 OK with response body
Response = new UserResponse { Id = 1, Name = "John" };
Send(); // or Send(customResponse)

// 201 Created with Location header
SendCreatedAt("/users/123", response);
```

**Client Error Responses**
```csharp
// 400 Bad Request - Validation errors
SendValidationError("Field is required");

// 404 Not Found
SendNotFound("User not found");

// 409 Conflict
SendConflict("Email already exists");
```

**Server Error Responses**
```csharp
// 500 Internal Server Error
SendInternalError("Database connection failed");

// 502 Bad Gateway
SendExternalError("External API unreachable");

// 504 Gateway Timeout
SendExternalTimeout("External API timeout");
```

**Redirect Responses**
```csharp
// 301 Moved Permanently
SendRedirect("/new-location", permanent: true);

// 302 Found (temporary redirect)
SendRedirect("/temporary-location");

// 307/308 with method preservation
SendRedirect("/location", permanent: true, preserveMethod: true);
```

**Binary Responses**
```csharp
// Byte array download
SendBytes(new RadBytes 
{ 
    Bytes = data, 
    ContentType = "application/pdf",
    FileDownloadName = "report.pdf"
});

// Stream response
SendStream(new RadStream 
{ 
    Stream = fileStream, 
    ContentType = "text/csv"
});

// Physical file
SendFile(new RadFile 
{ 
    Path = "/path/to/file.txt",
    ContentType = "text/plain"
});
```

**Custom Problem Details**
```csharp
// Using ProblemHttpResult
SendProblem(TypedResults.Problem(
    title: "Custom error",
    statusCode: 422,
    detail: "Detailed error message"));

// Using IRadProblem for domain errors
SendProblem(new CustomDomainError("Business rule violated"));
```

**Testing Response Methods**

Integration tests use typed client extensions:
```csharp
// Test string responses (NotFound, Conflict)
var response = await client.GetAsync<NotFoundEndpoint, Request, string>(request);
response.Http.StatusCode.Should().Be(HttpStatusCode.NotFound);

// Test ProblemDetails responses
var response = await client.GetAsync<ErrorEndpoint, Request, ProblemDetails>(request);
response.Content.Title.Should().Be("Internal server error");

// Test binary responses (use raw HttpClient)
var response = await client.GetAsync("/api/download");
var bytes = await response.Content.ReadAsByteArrayAsync();
```

See the test endpoints in [`/MinimalApi/Features/ResultEndpoints`](https://github.com/MetalHexx/RadEndpoints/tree/main/MinimalApi/Features/ResultEndpoints) for complete examples of all response methods.

#### Request Model Binding and Validation
- Automatic request model binding from route, query, header, and body using [AsParameters].
- Automatic request model validation execution using [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
- Simply add your AbstractValidator class that targets your request model type. 
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

#### Endpoint Model Mapper
- Assign mappers for conventient access from endpoint
- Makes mapping a first class citizen with the endpoint configuration
- Map manually or with a mapping tool like AutoMapper or Mapster
```csharp
public class GetExampleMapper : IRadMapper<GetExampleRequest, GetExampleResponse, Example>
{
    public GetExampleResponse FromEntity(Example e) => new()
    {
        Data = new()
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName
        }
    };
    public Example ToEntity(GetExampleRequest r) => throw new NotImplementedException();
}
```
#### Flexibility and Alternate Base Endpoint Class
- Don't like the endpoint base classes?  Make your own using the included abstractions.
- Want to use a bare minimum REPR endpoint with Open Union Types?  Go for it.
- Need to create a super specialized edge case endpoint with pure minimal api endpoint?  No problem.

```csharp
  //Code samples coming soon
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
#### Unit Testing
- `EndpointFactory` generates mockable, unit testable endpoint instances.
- `RadTestClientExtensions` test responses using TypedResults pattern, much like vanilla Minimal Apis 
- See [UNIT-TESTING-GUIDE.md](https://github.com/MetalHexx/RadEndpoints/blob/main/RadEndpoints.Testing/UNIT-TESTING-GUIDE.md) for details and more examples.
```csharp
[Fact]
public async Task When_ValidId_ShouldReturnItem()
{
    // Arrange
    var endpoint = EndpointFactory.CreateEndpoint<GetItemEndpoint>();
    var request = new GetItemRequest { Id = 1 };

    // Act
    await endpoint.Handle(request, CancellationToken.None);

    // Assert
    var result = endpoint.GetResult<Ok<GetItemResponse>>();
    result.Value.Id.Should().Be(1);
    result.Value.Name.Should().Be("Item 1");    

    endpoint.GetStatusCode().Should().Be(HttpStatusCode.OK);
}

[Fact]
public async Task When_InvalidId_ShouldReturnNotFound()
{
    // Arrange
    var endpoint = EndpointFactory.CreateEndpoint<GetItemEndpoint>();
    var request = new GetItemRequest { Id = 0 };

    // Act
    await endpoint.Handle(request, CancellationToken.None);

    // Assert
    var result = endpoint.GetResult<NotFound<string>>();
    result.Value.Should().Be("Item not found");

    endpoint.GetStatusCode().Should().Be(HttpStatusCode.NotFound);
}
```

### CLI For Scaffolding
- Scaffold multiple new endpoints very quickly.
- Import a set of endoints using a JSON definition.
- Full parameter support for 1 line endpoint creation.

#### Endpoint Wizard
<img src="https://github.com/MetalHexx/RadEndpoints/assets/9291740/8782c1e9-ef40-4c0b-9b1c-dc9f96ae3826" width="60%" height="60%" alt="Description of Image"/>

#### JSON definition
```javascript
[
  {
    "BaseNamepace": "Demo.Api.Endpoints",
    "ResourceName": "User",
    "Verb": "Get",
    "EndpointName": "GetUser",
    "Path": "/users/{id}",
    "Entity": "User",
    "Tag": "User",
    "Description": "Get User by ID",
    "WithMapper": true
  },
  {
    "BaseNamepace": "Demo.Api.Endpoints",
    "ResourceName": "User",
    "Verb": "Post",
    "EndpointName": "CreateUser",
    "Path": "/users",
    "Entity": "User",
    "Tag": "User",
    "Description": "Create a new User",
    "WithMapper": true
  }
  ...other endpoints.
]
```
#### Bulk JSON Import
<img src="https://github.com/MetalHexx/RadEndpoints/assets/9291740/eafc6050-9afd-4c4b-a844-a6b1033b9f98" width="60%" height="60%" alt="Description of Image"/>

### 📦 Installation

You can install the RadEndpoints packages from NuGet:

- [RadEndpoints](https://www.nuget.org/packages/RadEndpoints/)
- [RadEndpoints.Cli](https://www.nuget.org/packages/RadEndpoints.Cli/)
- [RadEndpoints.Testing](https://www.nuget.org/packages/RadEndpoints.Testing/)

#### Install via .NET CLI:

```bash
dotnet add package RadEndpoints
dotnet add package RadEndpoints.Cli
dotnet add package RadEndpoints.Testing
```
  
### Coming Soon:
- Project templates
- Observability Tooling
- Additional code coverage
- Documentation / How Tos

### Credits
- [FastEndpoints](https://fast-endpoints.com/) -- as many of the ideas from that project inspired this one.
