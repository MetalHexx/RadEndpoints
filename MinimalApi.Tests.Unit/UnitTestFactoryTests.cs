using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MinimalApi.Features.FactoryTestEndpoints;

namespace MinimalApi.Tests.Unit
{
    public class UnitTestFactoryTests
    {
        [Fact]
        public async Task When_CreateEndpoint_WithDefaultDependencies_ShouldUseDefaultDependencies()
        {
            // Arrange
            var request = new TestRequest { TestProperty = 5 };           
            var endpoint = EndpointFactory.CreateEndpoint<FactoryTestingEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);
            
            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.TestProperty.Should().Be(6); // TestProperty + 1
        }
        
        [Fact]
        public async Task When_CreateEndpoint_WithCustomLogger_ShouldUseProvidedLogger()
        {
            // Arrange
            var request = new TestRequest { TestProperty = 10 };
            var mockLogger = Substitute.For<ILogger<FactoryTestingEndpoint>>();
            
            var endpoint = EndpointFactory.CreateEndpoint(
                logger: mockLogger,
                constructorArgs: []);

            // Act
            await endpoint.Handle(request, CancellationToken.None);
            
            // Assert
            mockLogger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("TestProperty: 10")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }
        
        [Fact]
        public async Task When_CreateEndpoint_WithCustomEnvironment_ShouldUseProvidedEnvironment()
        {
            // Arrange
            var request = new TestRequest { TestProperty = 15 };
            var mockLogger = Substitute.For<ILogger<FactoryTestingEndpoint>>();            
            var mockEnvironment = Substitute.For<IWebHostEnvironment>();
            mockEnvironment.EnvironmentName.Returns("Production");
            
            // Act
            var endpoint = EndpointFactory.CreateEndpoint<FactoryTestingEndpoint>(
                logger: mockLogger,
                webHostEnvironment: mockEnvironment);
            
            await endpoint.Handle(request, CancellationToken.None);
            
            // Assert
            mockLogger.Received(1).Log(
                LogLevel.Critical,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("AspNetEnvironment: Production")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }
        
        [Fact]
        public async Task When_CreateEndpoint_WithCustomHttpContext_ShouldUseProvidedHttpContext()
        {
            // Arrange
            var request = new TestRequest { TestProperty = 20 };
            
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = "POST";
            
            var mockHttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            mockHttpContextAccessor.HttpContext.Returns(httpContext);
            
            var mockLogger = Substitute.For<ILogger<FactoryTestingEndpoint>>();

            var endpoint = EndpointFactory.CreateEndpoint<FactoryTestingEndpoint>(
                logger: mockLogger,
                httpContextAccessor: mockHttpContextAccessor);
            
            await endpoint.Handle(request, CancellationToken.None);
            
            // Assert            
            mockLogger.Received(1).Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("HttpVerb:POST")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }
        
        [Fact]
        public async Task When_CreateEndpoint_WithAllCustomDependencies_ShouldUseAllProvidedDependencies()
        {
            // Arrange
            var request = new TestRequest { TestProperty = 30 };
            
            var mockLogger = Substitute.For<ILogger<FactoryTestingEndpoint>>();            
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = "PUT";
            var mockHttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            mockHttpContextAccessor.HttpContext.Returns(httpContext);
            
            var mockEnvironment = Substitute.For<IWebHostEnvironment>();
            mockEnvironment.EnvironmentName.Returns("Staging");
            mockEnvironment.ApplicationName.Returns("StagingApp");
            
            // Act
            var endpoint = EndpointFactory.CreateEndpoint<FactoryTestingEndpoint>(
                logger: mockLogger,
                httpContextAccessor: mockHttpContextAccessor,
                webHostEnvironment: mockEnvironment);
            
            await endpoint.Handle(request, CancellationToken.None);
            
            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.TestProperty.Should().Be(31); // TestProperty + 1
            
            mockLogger.Received(1).Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("HttpVerb:PUT")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
                
            mockLogger.Received(1).Log(
                LogLevel.Critical,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("AspNetEnvironment: Staging")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }
    }
}