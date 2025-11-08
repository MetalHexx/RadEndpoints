# RadEndpoints Project - Copilot Instructions

## Project Overview
RadEndpoints is a lightweight .NET 8.0 API library that brings REPR (Request-Endpoint-Response) style endpoint classes to ASP.NET Core Minimal APIs. The library embraces vertical slice architecture, making it easy to rapidly scaffold API endpoints with built-in validation, mapping, and testing support.

**Key Philosophy:**
- Lightweight and easy to work with or without
- Junior Developer Friendly without impeding experienced engineers
- Backward Compatible with Minimal API configuration and features
- Configurable using RouteHandlerBuilder
- Well Structured for validation, mapping, and error handling
- Fast and Easy to scaffold projects, endpoints, and tests

## Solution Structure

### Core Projects
- **RadEndpoints** - Main library package containing endpoint base classes, validation, extensions, and core abstractions
  - `Endpoint/` - Base endpoint classes and abstractions
  - `Extensions/` - Startup, routing, and utility extensions
  - `Validation/` - FluentValidation integration
  - `Problem/` - Problem details and error handling
  - `Mediator/` - Internal mediator pattern implementation

- **RadEndpoints.Testing** - Testing utilities and helpers for integration and unit tests
  - `EndpointFactory` - Creates testable endpoint instances with mocked dependencies
  - `RadTestClientExtensions` - "Routeless" HttpClient extensions for strongly-typed API testing
  - `RadResponseAssertions` - FluentAssertions for responses and ProblemDetails
  - `TypedResultsTestExtensions` - Extension methods for extracting and verifying TypedResults in unit tests
  - Documentation: `UNIT-TESTING-GUIDE.md` and `CUSTOM-JSON-SERIALIZATION.md`

- **RadEndpoints.Cli** - CLI tool for scaffolding endpoints quickly
  - Interactive wizard for guided endpoint generation
  - Bulk endpoint generation from JSON definitions
  - Template system in `Templates/`

### Demo/Test Projects
- **MinimalApi** - Demo API showcasing RadEndpoints features and usage patterns
  - `Features/` - Organized by feature using vertical slices
  - `Domain/` - Example domain models and services
  - Reference implementation for users

- **MinimalApi.Tests.Integration** - Integration tests using RadEndpoints.Testing
- **MinimalApi.Tests.Unit** - Unit tests demonstrating EndpointFactory usage
- **RadEndpoints.Tests.Unit** - Unit tests for RadEndpoints core library
- **RadEndpoints.Testing.Tests** - Tests for the testing library itself
- **RadEndpoints.Tests.Performance** - Performance and benchmark tests

## Key Concepts

### REPR Pattern
Every endpoint follows the Request-Endpoint-Response pattern:
1. **Request Model** - Defines input with binding attributes (`[FromRoute]`, `[FromQuery]`, `[FromBody]`, `[FromHeader]`)
2. **Endpoint Class** - Contains configuration and handle logic
3. **Response Model** - Defines output structure
4. **Optional Mapper** - Maps between domain entities and DTOs
5. **Optional Validator** - FluentValidation AbstractValidator for request validation

### Endpoint Base Classes
- `RadEndpoint` - Base class with common functionality
- `RadEndpoint<TRequest, TResponse>` - Endpoint with request and response
- `RadEndpoint<TRequest, TResponse, TMapper>` - Includes mapper for entity transformations
- `RadEndpointWithoutRequest<TResponse>` - For endpoints without request parameters

### Built-in Endpoint Features
- Constructor dependency injection (scoped lifetime)
- `HttpContext` access
- `Logger<T>` logging
- `Env` (IWebHostEnvironment) access
- `Response` object for building responses
- `Map` property for accessing mapper
- `HasValidator` indicates if FluentValidator is registered
- TypedResults shortcuts: `Send()`, `SendNotFound()`, `SendProblem()`, etc.

### Validation
- Automatic FluentValidation execution before Handle() method
- Create validators by inheriting from `AbstractValidator<TRequest>`
- Assembly scanned and registered automatically
- Returns ValidationProblemDetails on validation failure

### Testing Approaches

#### Integration Testing
- Use `RadTestClientExtensions` for strongly-typed "routeless" HTTP client calls
- Automatic endpoint route discovery and request model binding
- Fluent assertions for responses: `BeSuccessful<T>()`, `BeValidationProblem()`, etc.
- Easy navigation from test to endpoint code
- Tests full pipeline including validation, routing, and serialization

Example pattern:
```csharp
var response = await client.GetAsync<GetExampleEndpoint, GetExampleRequest, GetExampleResponse>(request);
response.Should().BeSuccessful<GetExampleResponse>().WithStatusCode(HttpStatusCode.OK);
```

#### Unit Testing
- Use `EndpointFactory.CreateEndpoint<T>()` to create testable instances
- Mock dependencies including ILogger, IHttpContextAccessor, IWebHostEnvironment
- Extract results using `TypedResultsTestExtensions`: `GetResult<Ok<T>>()`, `GetStatusCode()`, etc.
- Tests endpoint logic in isolation without HTTP pipeline
- Reference: `RadEndpoints.Testing/UNIT-TESTING-GUIDE.md`

## Code Organization Patterns

### Vertical Slice Structure
Each feature/endpoint lives in its own folder with related files:
```
Features/
  ExampleResource/
    GetExample/
      GetExampleEndpoint.cs      # Endpoint class
      GetExampleModels.cs         # Request/Response models + validator
      GetExampleMapper.cs         # Optional mapper
    CreateExample/
      ...
    _common/                      # Shared DTOs and utilities
```

### Endpoint File Structure
1. **EndpointName.cs** - Contains the endpoint class that inherits from RadEndpoint base
2. **EndpointNameModels.cs** - Request model, response model, and validator
3. **EndpointNameMapper.cs** (optional) - Implements IRadMapper for entity mapping

### Naming Conventions
- Endpoints: `{Verb}{Resource}Endpoint` (e.g., GetExampleEndpoint, CreateUserEndpoint)
- Requests: `{Verb}{Resource}Request` (e.g., GetExampleRequest)
- Responses: `{Verb}{Resource}Response` (e.g., GetExampleResponse)
- Validators: `{Request}Validator` (e.g., GetExampleRequestValidator)
- Mappers: `{Verb}{Resource}Mapper` (e.g., GetExampleMapper)

## Development Workflow

### Adding a New Endpoint

1. **Using CLI (Recommended):**
   ```bash
   rad generate endpoint  # Interactive wizard
   # OR
   rad generate endpoint -i endpoints.json  # Bulk generation
   ```

2. **Manual Creation:**
   - Create folder in appropriate Features subdirectory
   - Create endpoint class inheriting from RadEndpoint
   - Implement `Configure()` method with routing and OpenAPI configuration
   - Implement `Handle()` method with business logic
   - Create request/response models
   - Add validator if needed (inherits from AbstractValidator)
   - Add mapper if needed (implements IRadMapper)

3. **Registration:**
   - Endpoints are automatically discovered and registered via `AddRadEndpoints(typeof(Program))`
   - Mapped via `MapRadEndpoints()` in Program.cs

### Building and Testing

**Build Solution:**
```powershell
dotnet build MinimalApiPoc.sln
```

**Run Demo API:**
```powershell
cd MinimalApi
dotnet run
# Swagger available at: https://localhost:{port}/swagger
```

**Run Tests:**
```powershell
dotnet test  # All tests
dotnet test --filter FullyQualifiedName~Integration  # Integration tests only
dotnet test --filter FullyQualifiedName~Unit  # Unit tests only
```

**Build Local NuGet Packages:**
```powershell
.\build-local-packages.ps1
# Creates test packages in ./local-packages directory
```

### Working with Dependencies
- **FluentValidation** - Used for request validation
- **Microsoft.AspNetCore.OpenApi** - OpenAPI/Swagger support
- **NSubstitute** - Mocking framework for tests
- **FluentAssertions** - Fluent assertion library for tests
- **xUnit** - Test framework

## Common Tasks

### Adding Validation to a Request
```csharp
public class MyRequestValidator : AbstractValidator<MyRequest>
{
    public MyRequestValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
```

### Configuring an Endpoint
```csharp
public override void Configure()
{
    Get("/resources/{id}")
        .Produces<MyResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesValidationProblem()
        .WithDocument(tag: "Resources", desc: "Get resource by ID");
}
```

### Handling Responses
```csharp
// Success
Response = new MyResponse { Data = result };
Send();  // Sends 200 OK

// Not Found
SendNotFound("Resource not found");

// Problem
SendProblem(problemObject);

// Custom status
Send(StatusCodes.Status201Created);
SendCreated("/resources/123", response);
```

### Using Mappers
```csharp
public class MyMapper : IRadMapper<MyRequest, MyResponse, MyEntity>
{
    public MyResponse FromEntity(MyEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name
    };
    
    public MyEntity ToEntity(MyRequest request) => new()
    {
        Name = request.Name
    };
}

// In endpoint:
var entity = Map.ToEntity(request);
Response = Map.FromEntity(entity);
```

### Writing Integration Tests
```csharp
[Fact]
public async Task When_ValidRequest_ReturnsSuccess()
{
    // Arrange
    var request = new GetExampleRequest { Id = 1 };
    
    // Act
    var response = await Client.GetAsync<GetExampleEndpoint, GetExampleRequest, GetExampleResponse>(request);
    
    // Assert
    response.Should()
        .BeSuccessful<GetExampleResponse>()
        .WithStatusCode(HttpStatusCode.OK);
}
```

### Writing Unit Tests
```csharp
[Fact]
public async Task When_ValidRequest_SetsCorrectResponse()
{
    // Arrange
    var mockService = Substitute.For<IMyService>();
    var endpoint = EndpointFactory.CreateEndpoint<MyEndpoint>(mockService);
    
    // Act
    await endpoint.Handle(request, CancellationToken.None);
    
    // Assert
    var result = endpoint.GetResult<Ok<MyResponse>>();
    result.Value.Should().NotBeNull();
    endpoint.GetStatusCode().Should().Be(HttpStatusCode.OK);
}
```

## Important Notes

### Testing Strategy
- **Integration tests are recommended** for most scenarios as they provide more value
- Use unit tests for complex logic that's hard to test through HTTP layer
- Integration tests validate the entire pipeline including validation and routing
- Unit tests validate endpoint logic in isolation

### Custom JSON Serialization
- Configure custom JsonSerializerOptions via RadHttpClientOptions
- Server must use matching JSON configuration (configure in Program.cs via ConfigureHttpJsonOptions)
- Error responses always use default serialization for standard ProblemDetails
- See `RadEndpoints.Testing/CUSTOM-JSON-SERIALIZATION.md` for details

### Problem Handling
- Use built-in problem types: ValidationError, NotFoundError, ConflictError, ForbiddenError, InternalError, ExternalError
- Implement IRadProblem for custom error types
- Override GetProblemResult() for custom problem handling

### Route Caching
- Routes are cached internally for performance
- Access via `RadEndpoint.GetRoute<TEndpoint>()` static method
- Used by testing extensions for routeless client calls

### Assembly Scanning
- Endpoints, validators, and mappers are discovered via assembly scanning
- Use `AddRadEndpoints(typeof(Program))` to scan the assembly containing Program class
- All concrete implementations of IRadEndpoint, IRadMapper, and AbstractValidator are registered

## Project Goals
When working on this project, prioritize:
1. **Simplicity** - Keep the API surface simple and intuitive
2. **Performance** - Minimal overhead over raw Minimal APIs
3. **Developer Experience** - Make it easy to scaffold, test, and maintain endpoints
4. **Backward Compatibility** - Don't break existing Minimal API features
5. **Documentation** - Keep examples and docs up to date

## References
- Main README: `/README.md`
- Unit Testing Guide: `/RadEndpoints.Testing/UNIT-TESTING-GUIDE.md`
- Custom JSON Guide: `/RadEndpoints.Testing/CUSTOM-JSON-SERIALIZATION.md`
- CLI Guide: `/RadEndpoints.Cli/README.md`
- Demo Endpoints: `/MinimalApi/Features/`
- GitHub: https://github.com/MetalHexx/RadEndpoints
- NuGet: RadEndpoints, RadEndpoints.Testing, RadEndpoints.Cli

## Code Style Guidelines
- Use nullable reference types (enabled in project)
- Use implicit usings where appropriate
- Follow C# naming conventions (PascalCase for public members, _camelCase for private fields)
- Keep endpoint Handle() methods focused - extract complex logic to services
- Add XML documentation comments for public APIs
- Write clear, descriptive test names following the When_Condition_ExpectedResult pattern
