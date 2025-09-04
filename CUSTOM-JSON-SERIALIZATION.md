# Custom JSON Serialization

RadEndpoints.Testing helper classes support custom JSON serialization options to handle advanced scenarios like enum string conversion, camelCase property naming, and null value handling.

## Overview

The `RadHttpClientOptions` class allows you to specify custom `JsonSerializerOptions` that control how requests and responses are serialized/deserialized during testing.
```csharp
public class RadHttpClientOptions 
{
    public HeaderDictionary Headers { get; set; } = [];
    public JsonSerializerOptions? JsonSerializerOptions { get; set; }
}
```
## Basic Usage

### 1. Configure JSON Options
```csharp
var jsonOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Converters = { new JsonStringEnumConverter() },
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};
```

### 2. Create Client Options
```csharp
var options = new RadHttpClientOptions
{
    JsonSerializerOptions = jsonOptions
};
```

### 3. Use in Tests
```csharp
var request = new CustomJsonRequest
{
    Id = "test-id",
    Body = new CustomJsonBody
    {
        Name = "Test User",
        EnumValue = TestEnumValue.ThirdOption,
        CreatedDate = DateTime.UtcNow
    }
};

var response = await client.PostAsync<CustomJsonEndpoint, CustomJsonRequest, CustomJsonResponse>(
    request, 
    options
);
```
## Common Scenarios

### Enum as String

Convert enums to/from strings instead of numbers:

```csharp
var jsonOptions = new JsonSerializerOptions
{
    Converters = { new JsonStringEnumConverter() }
};
// TestEnumValue.ThirdOption becomes "ThirdOption" instead of 2
```
### CamelCase Properties


Convert PascalCase properties to camelCase:
```csharp
var jsonOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

// "EnumValue" becomes "enumValue"
```
### Ignore Null Values

Exclude null properties from JSON:
```csharp
var jsonOptions = new JsonSerializerOptions
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};
```
## Server Configuration

> **Important:** Your API server must use matching JSON options for requests to be processed correctly.

Configure your server in `Program.cs`:
```csharp
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
```

## Error Response Handling

- Custom `JsonSerializerOptions` only apply to successful (2xx) responses. 
- Error responses (4xx, 5xx) always use default .NET serialization to ensure standard `ValidationProblemDetails` and `ProblemDetails` objects deserialize correctly.

```csharp 
// This works even with custom JSON options
var response = await client.PostAsync<MyEndpoint, MyRequest, ValidationProblemDetails>(request, options);

response.Should().BeValidationProblem()
    .WithKeyAndValue("PropertyName", "Error message");

```

## Complete Example

```csharp
[Fact]
public async Task CustomJson_WithEnumStrings_ShouldWork()
{
    // Arrange
    var jsonOptions = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    var options = new RadHttpClientOptions
    {
        JsonSerializerOptions = jsonOptions
    };

    var request = new CustomJsonRequest
    {
        Id = "enum-test",
        Body = new CustomJsonBody
        {
            Name = "Enum Test",
            EnumValue = TestEnumValue.SecondOption, // Serialized as "SecondOption"
            CreatedDate = DateTime.UtcNow
        }
    };

    // Act
    var response = await f.Client.PostAsync<CustomJsonEndpoint, CustomJsonRequest, CustomJsonResponse>(
        request, 
        options
    );

    // Assert
    response.Should().BeSuccessful<CustomJsonResponse>()
        .WithStatusCode(HttpStatusCode.OK);
    
    response.Content.EnumValue.Should().Be(TestEnumValue.SecondOption);
}
```

## Troubleshooting

| Problem | Solution |
|---------|----------|
| `400 Bad Request` errors | Ensure server JSON options match test options |
| Enum deserialization fails | Add `JsonStringEnumConverter` to both server and test options |
| Property names don't match | Use consistent `PropertyNamingPolicy` on server and tests |
| Response deserialization fails | Check server response serialization configuration |

## Backward Compatibility

- Tests without `JsonSerializerOptions` use default .NET JSON serialization
- All existing tests continue working without changes
- Custom options are completely optional

## Reference Examples

The following files in this codebase demonstrate custom JSON serialization in action:

### Endpoint Implementation
- **[CustomJsonEndpoint.cs](MinimalApi/Features/ParameterTests/EmptyStringTests/CustomJsonEndpoint.cs)** - Shows the endpoint setup with request/response models and enum handling

### Test Examples
- **[CustomJsonSerializationTests.cs](MinimalApi.Tests.Integration/Tests/ParameterTests/CustomJsonSerializationTests.cs)** - Complete test suite demonstrating:
  - Default serialization behavior
  - Custom headers with JSON requests
  - Validation with custom JSON options
  - CamelCase property naming
  - Enum string conversion

### Server Configuration
- **[Program.cs](MinimalApi/Program.cs)** - Server-side JSON configuration that matches the test client options

These examples provide working implementations you can reference when implementing custom JSON serialization in your own tests.