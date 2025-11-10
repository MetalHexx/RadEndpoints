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
- AI Friendly -- as simpler patterns help produce more consistent agent generated code.

### Features:
#### REPR Endpoint Classes
- Constructor dependency injection
- Scoped lifetime endpoint classes
- Automatic endpoint, mapper, and validator discovery with assembly scanning 
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
        // The Get() method here returns RouteHandlerBuilder

        Get("/samples/{id}")
            .Produces<GetSampleResponse>(StatusCodes.Status200OK)            
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .WithDocument(tag: "Sample", desc: "Get Sample by ID");

            // Works with .any net core Minimal API compatible extension methods.
    }

    public override async Task Handle(GetSampleRequest r, CancellationToken ct)
    {
        var sample = await sampleService.GetSampleById(r.Id, ct);

        if(sample is null)
        {
            // Helper methods for setting TypedResults-based responses
            SendNotFound("Sample not found.");
            return;
        }
        Response = Map.FromEntity(sample); // Using typed mapper

        Send(); // Sends 200 OK with Response object
    }
}
```

#### Endpoint Base Classes
RadEndpoints provides four base classes you can use to support different endpoint scenarios:

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

**`RadEndpointWithoutRequest<TResponse>`** - Endpoint without request model
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

**`RadEndpoint`** - Untyped base class for maximum flexibility

The untyped RadEndpoint provides full control of .net Minimal API endpoint creation while still offering additional conveniences.  If you like using .NET 8+ TypedResults, or have specific requirements that don't fit the opinionated typed RadEndpoint pattern, this base class is for you.
```csharp
public class TypedResultsEndpoint : RadEndpoint
{
    public override void Configure()
    {
        var route = SetRoute("/items/{id}"); // Optional: SetRoute() enables routless integration testing on untyped endpoints.
                                             // Typed endpoints don't require you to call SetRoute().
        
        //The RouteBuilder property on the endpoint class gives you granular control over the Minimal endpoint configuration with nothing in the way.
        RouteBuilder
            .MapPut(route, ([AsParameters]ItemRequest r) => Handle(r))
            .WithRadValidation<ItemRequest>();
    }
    
    // Minimal API style handler method returning TypedResults with Union Types.
    public async Task<Results<Ok<ItemResponse>, NotFound<ProblemDetails>>> Handle(ItemRequest r)
    {
        var item = await itemService.GetById(r.Id);
        if (item is null)
            return TypedResults.NotFound(new ProblemDetails { Title = "Not found" });
        
        return TypedResults.Ok(new ItemResponse { Id = item.Id });
    }
}
```
**Advantages of using untyped RadEndpoint:**
- Full control over Minimal API endpoint configuration via `RouteBuilder`
- Use of .NET TypedResults with Open Union Types
- Flexibility to define custom handler method signatures.

**RadEndpoint Features you still get:**
- Assembly-scanned endpoint discovery
- Constructor dependency injection (scoped lifetime)
- Built-in conveniences: `Logger`, `Env`, `HttpContext`
- Optional routeless integration testing (using `SetRoute()`)
- Rad Validation filters (requires manual `.WithRadValidation<T>()` configuration)

**Trade-offs of not using typed endpoints:**
- No automatic request/response typing enforcement
- No `Send()` shortcuts (use `TypedResults` directly or create custom helpers)
- No automatic FluentValidation execution (requires manual configuration)
- No built-in mapper integration
- More boilerplate code in endpoint configuration

**Custom Base Endpoints**

Use the untyped `RadEndpoint` base class to create your own base endpoint classes and establish custom patterns and helpers tailored uniquely for your application:

```csharp
// Custom base class with your own conventions
public abstract class CustomBaseEndpoint<TRequest, TResponse> : RadEndpoint
    where TRequest : CustomBaseRequest
    where TResponse : CustomBaseResponse, new()
{
    public abstract Task<TResponse> Handle(TRequest r, CancellationToken ct);

    // Example Get helper method
    public RouteHandlerBuilder Get(string route)
    {
        SetRoute(route);

        return RouteBuilder!.MapGet(route, 
            async ([AsParameters] TRequest r, CancellationToken ct) => await Handle(r, ct));
    }

    // ...Similarly, define Post, Put, Delete helpers

    public abstract Task<TResponse> Handle(TRequest r, CancellationToken ct);
    
    // Add your own response helper methods
    public TResponse BadRequest(string message) => new TResponse 
    { 
        Message = message,
        StatusCode = HttpStatusCode.BadRequest
    };
}

// Use your custom base
public class GetItemEndpoint : CustomBaseEndpoint<GetItemRequest, GetItemResponse>
{
    public override void Configure() => Get("items/{id}");
    
    public override async Task<GetItemResponse> Handle(GetItemRequest r, CancellationToken ct)
    {
        if (r.Id <= 0)
            return BadRequest("Invalid ID");
        
        return new GetItemResponse { Id = r.Id, Name = "Item" };
    }
}
```

**Custom base classes let you:**
- Define application-specific patterns and conventions
- Create custom helper methods tailored to your needs
- Establish architectural guardrails for your team
- Mix and match with standard RadEndpoint base classes

See [`TypedResultsPutEndpoint`](https://github.com/MetalHexx/RadEndpoints/blob/main/MinimalApi/Features/WithTypedResults/TypedResultsPut/TypedResultsPutEndpoint.cs) and [`CustomBaseEndpoint`](https://github.com/MetalHexx/RadEndpoints/blob/main/MinimalApi/Features/CustomBase/_common/CustomBaseEndpoint.cs) for complete examples.

**All base classes provide:**
- Constructor dependency injection (scoped lifetime)
- Built-in conveniences: `HttpContext`, `Logger`, `Env`
- Assembly-scanned endpoint configuration

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

#### Request/Response Mapping

RadEndpoints provides an optional mapper abstraction (`IRadMapper`) to transform data between your domain entities and API DTOs. Mappers provide a declarative and consistent place to perform endpoint mapping operations, helping maintain clean separation between your app / domain layers and API contracts.

**Mapper Interfaces**

RadEndpoints provides two mapper interfaces:

**`IRadMapper<TResponse, TEntity>`** - One-way mapping for read operations (GET endpoints)
```csharp
public interface IRadMapper<TResponse, TEntity> : IRadMapper
{
    TResponse FromEntity(TEntity e);
}
```

**`IRadMapper<TRequest, TResponse, TEntity>`** - Two-way mapping for read/write operations (POST, PUT, PATCH endpoints)
```csharp
public interface IRadMapper<TRequest, TResponse, TEntity> : IRadMapper
{
    TEntity ToEntity(TRequest r);      // Convert request DTO to domain entity
    TResponse FromEntity(TEntity e);   // Convert domain entity to response DTO
}
```

**Creating a Mapper**

Define a mapper class that implements one of the `IRadMapper` interfaces:

```csharp
// Domain entity
public record User(string FirstName, string LastName, int Id = 0);

// Request/Response DTOs
public class CreateUserRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class UserResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
}

// Two-way mapper for create operations
public class CreateUserMapper : IRadMapper<CreateUserRequest, UserResponse, User>
{
    // Convert request DTO to domain entity (for saving)
    public User ToEntity(CreateUserRequest r) => new User(r.FirstName, r.LastName);
    
    // Convert domain entity to response DTO (for returning)
    public UserResponse FromEntity(User e) => new()
    {
        Id = e.Id,
        FullName = $"{e.FirstName} {e.LastName}"
    };
}

// One-way mapper for read operations
public class GetUserMapper : IRadMapper<GetUserRequest, UserResponse, User>
{
    public UserResponse FromEntity(User e) => new()
    {
        Id = e.Id,
        FullName = $"{e.FirstName} {e.LastName}"
    };
    
    // Not needed for GET operations
    public User ToEntity(GetUserRequest r) => throw new NotImplementedException();
}
```

**Using Mappers in Endpoints**

Reference your mapper as a type parameter in your endpoint class. The mapper is automatically injected and accessible via the `Map` property:

```csharp
// Endpoint with integrated mapper (third type parameter)
public class CreateUserEndpoint(IUserService userService) 
    : RadEndpoint<CreateUserRequest, UserResponse, CreateUserMapper>
{
    public override void Configure()
    {
        Post("/users")
            .Produces<UserResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .WithDocument(tag: "Users", desc: "Create a new user");
    }

    public override async Task Handle(CreateUserRequest r, CancellationToken ct)
    {
        // Convert request to domain entity using mapper
        var user = Map.ToEntity(r);
        
        // Pass domain entity to service layer
        var createdUser = await userService.CreateUser(user, ct);
        
        // Convert domain entity back to response DTO
        Response = Map.FromEntity(createdUser);
        
        SendCreatedAt($"/users/{createdUser.Id}");
    }
}
```

**Mapper Features**

- **Automatic Discovery**: Mappers are automatically discovered and registered via assembly scanning
- **Scoped Lifetime**: Mappers have scoped lifetime, allowing dependency injection of scoped services
- **Type-Safe**: Compile-time type checking ensures correct usage
- **Integration Tools**: Compatible with AutoMapper, Mapster, or manual mapping
- **Testing**: Mappers can be tested independently of endpoints

See the [`CreateExampleEndpoint`](https://github.com/MetalHexx/RadEndpoints/blob/main/MinimalApi/Features/Examples/CreateExample/CreateExampleEndpoint.cs) and other examples in [`/MinimalApi/Features/Examples`](https://github.com/MetalHexx/RadEndpoints/tree/main/MinimalApi/Features/Examples) for complete working implementations.

  
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
