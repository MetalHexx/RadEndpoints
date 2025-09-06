# Unit Testing RadEndpoints

RadEndpoints provides a comprehensive testing infrastructure that makes unit testing endpoints feel familiar to developers who have worked with ASP.NET Core Minimal APIs. The testing framework uses the same **TypedResults pattern** that you're already familiar with, making tests easy to write and understand.

## Overview

The RadEndpoints testing framework consists of two main components:

1. **`EndpointFactory`** - Creates testable endpoint instances with mocked dependencies.
2. **`TypedResultsTestExtensions`** - Provides endpoint extension methods to extract and verify TypedResults.

## Philosophy

The RadEndpoints testing framework is designed with these principles in mind:

1. **Familiar Patterns** - Uses the same TypedResults pattern developers know from Minimal APIs
2. **Type Safety** - Strongly-typed result extraction with proper casting
3. **Comprehensive Coverage** - Supports most HTTP result types and scenarios
4. **Easy Mocking** - Simple dependency injection for testing different scenarios
5. **Clear Assertions** - Intuitive extension methods for common testing patterns

## Quick Start Example

Here's a simple example showing how to unit test a RadEndpoint:

### Example Endpoint Implementation
```csharp
public class GetItemRequest
{
    [FromRoute]
    public int Id { get; set; }
}

public class GetItemResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Message { get; set; } = "Item retrieved successfully";
}

public class GetItemEndpoint : RadEndpoint<GetItemRequest, GetItemResponse>
{
    public override void Configure()
    {
        Get("/items/{id}")
            .Produces<GetItemResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithDocument(tag: "Items", desc: "Get an item by ID");
    }

    public override async Task Handle(GetItemRequest r, CancellationToken ct)
    {   
        // ... async data retrieval

        if (r.Id <= 0)
        {
            SendNotFound("Item not found");
            return;
        }

        Response = new GetItemResponse
        {
            Id = r.Id,
            Name = $"Item {r.Id}"
        };

        Send();
    }
}
```
### Unit Test Example
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

## Using `EndpointFactory`

The `EndpointFactory` allows you to create testable instances of your endpoints with default or custom mocked dependencies.

### Creating an Endpoint with Default Mocks

By default, `EndpointFactory` provides default auto-mocked dependencies for the following:
- **ILogger**
- **IHttpContextAccessor (HttpContext)**
- **IWebHostEnvironment**
```csharp
var endpoint = EndpointFactory.CreateEndpoint<MyEndpoint>();
```
This is the simplest way to create an endpoint for testing. The default mocks are sufficient for many unit tests where you're not directly testing these dependencies.

### Creating an Endpoint with Custom Mocks
If you need to test specific scenarios that involve these dependencies, you can provide your own custom mocks. For example, you might want to verify logging behavior or simulate different HTTP contexts.
```csharp
You can provide custom dependencies to test specific scenarios. For example:
var customLogger = Substitute.For<ILogger<MyEndpoint>>();
var customContext = Substitute.For<IHttpContextAccessor>();
var customEnvironment = Substitute.For<IWebHostEnvironment>();

var endpoint = EndpointFactory.CreateEndpoint(
    logger: customLogger,
    httpContextAccessor: customContext,
    webHostEnvironment: customEnvironment
);
```
### Passing Constructor Arguments

You can also simply pass mocks for the constructor dependencies.  Doing so will automatically provide the default mocks for ILogger, IHttpContextAccessor, and IWebHostEnvironment.
```csharp
var endpoint = EndpointFactory.CreateEndpoint<MyEndpoint>
(
    Substitute.For<IMyCustomService>(),
    Substitute.For<IOtherDependency>(),
    ...any other constructor dependencies
);
```

### Creating an Endpoint with All Dependencies

You can create an endpoint with all possible dependencies, including the ILogger, IHttpContextAccessor, IWebHostEnvironment and any other constructor dependencies your endpoint relies on. For example:
```csharp
var endpoint = EndpointFactory.CreateEndpoint<MyEndpoint>
(
    Substitute.For<ILogger<MyEndpoint>>(),
    Substitute.For<IHttpContextAccessor>(),
    Substitute.For<IWebHostEnvironment>(),
    Substitute.For<IMyCustomService>(),
    Substitute.For<IOtherDependency>()
);
```
### Example: Testing with Custom Logger
```csharp
[Fact]
public async Task When_CustomLoggerProvided_ShouldLogCorrectly()
{
    // Arrange
    var customLogger = Substitute.For<ILogger<MyEndpoint>>();
    var endpoint = EndpointFactory.CreateEndpoint<MyEndpoint>(logger: customLogger);

    // Act
    await endpoint.Handle(new MyRequest(), CancellationToken.None);

    // Assert
    customLogger.Received(1).Log(
        LogLevel.Information,
        Arg.Any<EventId>(),
        Arg.Is<object>(o => o.ToString()!.Contains("Expected log message")),
        Arg.Any<Exception>(),
        Arg.Any<Func<object, Exception?, string>>()
    );
}
```
### Example: Testing with Custom HTTP Context
```csharp
[Fact]
public async Task When_CustomHttpContextProvided_ShouldUseIt()
{
    // Arrange
    var customContext = Substitute.For<IHttpContextAccessor>();
    var httpContext = new DefaultHttpContext();
    httpContext.Request.Headers.Append("mock-header", "mock-value");
    httpContext.Request.Method = "POST";
    customContext.HttpContext.Returns(httpContext);

    var endpoint = EndpointFactory.CreateEndpoint<MyEndpoint>(httpContextAccessor: customContext);

    // Act
    await endpoint.Handle(new MyRequest(), CancellationToken.None);

    // Assert
    Assert.Equal("POST", endpoint.HttpContext.Request.Method);
    // Assert something that depends on the custom header
}
```
These examples demonstrate how to use `EndpointFactory` to create endpoints with default or custom dependencies, making it easy to test various scenarios in isolation.


## RadEndpoints TypedResults Testing

The `TypedResultsTestExtensions` class provides a set of extension methods designed to simplify unit testing on responses for `RadEndpoint` instances. 

These methods enable developers to easily extract and verify result payloads and HTTP status codes using the familiar Minimal API **TypedResults pattern**. 

Key features include:
- **Result Extraction**: Retrieve specific result types (e.g., `Ok<T>`, `Created<T>`, `NotFound<T>`)
- **Problem Handling**: Access problem results (e.g., `ValidationProblem`, `ProblemHttpResult`) for error scenarios.
- **Status Code Verification**: Check the HTTP status code associated with the endpoint's response.

### Available Extension Methods

The `TypedResultsTestExtensions` class provides several extension methods for testing:

#### Result Extraction
- `endpoint.GetResult<T>()` - Gets specific TypedResult types (Ok<T>, Created<T>, NotFound<T>, etc.)
- `endpoint.GetProblem<T>()` - Gets problem results (ProblemHttpResult, ValidationProblem, IRadProblem implementations)
- `endpoint.HasResult()` - Returns true if endpoint set any result
- `endpoint.HasProblem()` - Returns true if endpoint set any problem

#### RadTestException Throwing
- If the expected result type is not found with `GetResult<T>` or `GetProblem<T>`, these methods throw a `RadTestException` with a descriptive message to help diagnose test failures.

#### HTTP Status Code Checking
- `endpoint.GetStatusCode()` - Gets the HTTP status code from any result type

### Supported TypedResults

The testing framework supports all ASP.NET Core TypedResults:

#### Success Results
- `Ok<T>` - 200 OK responses
- `Created<T>` - 201 Created responses
- `Accepted<T>` - 202 Accepted responses

#### Error Results
- `NotFound<T>` - 404 Not Found responses
- `Conflict<T>` - 409 Conflict responses
- `ProblemHttpResult` - Problem details responses
- `ValidationProblem` - 400 Validation error responses

#### Authentication Results
- `UnauthorizedHttpResult` - 401 authentication challenges
- `ForbidHttpResult` - 403 authorization failures

#### File Results
- `FileContentHttpResult` - Byte array file responses
- `FileStreamHttpResult` - Stream file responses
- `PhysicalFileHttpResult` - Physical file responses

#### Navigation Results
- `RedirectHttpResult` - Redirect responses

### Result Testing Examples
Many different scenarios are possible, but here are a few examples:
#### Success Response
```csharp
[Fact]
public async Task When_EndpointCalled_ReturnsSuccess()
{
    // Arrange
    var request = new TestRequest { TestProperty = 5 };
    var endpoint = EndpointFactory.CreateEndpoint<TestOkEndpoint>();

    // Act
    await endpoint.Handle(request, CancellationToken.None);  // adds 1 to the request TestProperty

    // Assert
    endpoint.HasResult().Should().BeTrue();
    var result = endpoint.GetResult<Ok<TestResponse>>();
    result.Value.TestProperty.Should().Be(6);

    endpoint.GetStatusCode().Should().Be(HttpStatusCode.OK);
}
```
#### Not Found Responses
```csharp
[Fact]
public async Task When_EndpointReturnsNotFound_ShouldReturnNotFoundResult()
{
    // Arrange
    var endpoint = EndpointFactory.CreateEndpoint<MyEndpoint>();
    var request = new MyRequest { Id = 999 }; 

    // Act
    await endpoint.Handle(request, CancellationToken.None);

    // Assert
    var result = endpoint.GetResult<NotFound<string>>();
    result.Value.Should().Be("Resource not found");

    endpoint.GetStatusCode().Should().Be(HttpStatusCode.NotFound);
}
```
#### Created Response
```csharp
[Fact]
public async Task When_EndpointSendsCreatedResponse_GetResult_ShouldReturnCreatedResult()
{
    // Arrange
    var request = new TestRequest { TestProperty = 10 };
    var endpoint = EndpointFactory.CreateEndpoint<TestCreatedEndpoint>();

    // Act
    await endpoint.Handle(request, CancellationToken.None);

    // Assert
    var result = endpoint.GetResult<Created<TestResponse>>();
    result!.Location.Should().Be("/test/100");
    result.Value!.TestProperty.Should().Be(100);

    endpoint.GetStatusCode().Should().Be(HttpStatusCode.Created);
}
```

#### Validation Problem Response
```csharp
[Fact]
public async Task When_EndpointReturnsValidationError_ShouldReturnValidationProblem()
{
    // Arrange
    var endpoint = EndpointFactory.CreateEndpoint<MyEndpoint>();
    var request = new MyRequest { Name = "" };

    // Act
    await endpoint.Handle(request, CancellationToken.None);

    // Assert
    var result = endpoint.GetResult<ValidationProblem>();
    result.ProblemDetails.Title.Should().Be("Validation failed");

    endpoint.GetStatusCode().Should().Be(HttpStatusCode.BadRequest);
}
```

#### Authentication Results
```csharp
[Fact]
public async Task When_EndpointReturnsAuthChallenge_ShouldReturnAuthenticationResult()
{
    // Arrange
    var endpoint = EndpointFactory.CreateEndpoint<MyEndpoint>();
    var request = new MyRequest { Token = null }; // No token provided

    // Act
    await endpoint.Handle(request, CancellationToken.None);

    // Assert
    var authResult = endpoint.GetResult<UnauthorizedHttpResult>();
    endpoint.GetStatusCode().Should().Be(HttpStatusCode.Unauthorized);
}
```
#### File Responses
```csharp
[Fact]
public async Task When_EndpointSendsFile_GetResult_ShouldReturnPhysicalFileHttpResult()
{
    // Arrange
    var endpoint = EndpointFactory.CreateEndpoint<TestFileEndpoint>();

    // Act
    await endpoint.Handle(CancellationToken.None);

    // Assert
    var result = endpoint.GetResult<PhysicalFileHttpResult>();
    result!.FileName.Should().Be("/path/to/file.txt");
    result.ContentType.Should().Be("text/plain");
    endpoint.GetStatusCode().Should().Be(HttpStatusCode.OK);
}
```
### Testing Endpoints without Requests (`RadEndpointWithoutRequest<TResponse>`)

For endpoints that don't require request parameters, the testing pattern is similar but simpler:
```csharp
[Fact]
public async Task When_EndpointSendsOkResponse_GetResult_ShouldReturnTypedResult()
{
    // Arrange
    var endpoint = EndpointFactory.CreateEndpoint<TestOkWithoutRequestEndpoint>();

    // Act
    await endpoint.Handle(CancellationToken.None);

    // Assert
    var result = endpoint.GetResult<Ok<TestResponse>>();
    result!.Value.Should().NotBeNull();
    result.Value!.TestProperty.Should().Be(42);

    endpoint.HasResult().Should().BeTrue();
    endpoint.GetStatusCode().Should().Be(HttpStatusCode.OK);
}
```
### Conclusion
The RadEndpoints testing framework provides a powerful and familiar way to unit test your endpoints using the TypedResults pattern. By leveraging `EndpointFactory` for creating testable `RadEndpoint` instances and `TypedResultsTestExtensions` for result extraction and verification, you can write clear and effective tests that ensure your endpoints behave as expected across a variety of scenarios.