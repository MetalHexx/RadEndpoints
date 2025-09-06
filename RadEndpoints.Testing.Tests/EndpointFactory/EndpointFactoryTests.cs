using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RadEndpoints.Testing.Tests
{
    public class EndpointFactoryTests
    {
        [Fact]
        public async Task Should_Handle_NoDependencyEndpoint()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10, StringProperty = "Test"};
            var endpoint = EndpointFactory.CreateEndpoint<NoDependencyEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response!.IntProperty.Should().Be(10);
            endpoint.Response.StringProperty.Should().Be("Test");
        }

        [Fact]
        public async Task Should_Handle_SingleServiceDependencyEndpoint()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 20 };

            var mockDependency = Substitute.For<IServiceDependency>();
            mockDependency.GetInt().Returns(5);
            mockDependency.GetString().Returns("MockedString");

            var endpoint = EndpointFactory
                .CreateEndpoint<SingleServiceDependencyEndpoint>(mockDependency);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response!.IntProperty.Should().Be(25);
            endpoint.Response.StringProperty.Should().Be("MockedString");
        }

        [Fact]
        public async Task Should_Handle_ServiceAndAnotherDependencyEndpoint()
        {
            // Arrange
            var request = new TestRequest { StringProperty = "TestString", IntProperty = 30 };

            var mockDependency = Substitute.For<IServiceDependency>();
            mockDependency.GetInt().Returns(10);
            mockDependency.GetString().Returns("MockedString");

            var mockAnotherDependency = Substitute.For<IAnotherDependency>();
            mockAnotherDependency.GetAnotherValue().Returns("AnotherMockedValue");

            var endpoint = EndpointFactory.CreateEndpoint<ServiceAndAnotherDependencyEndpoint>(
                constructorArgs: new object[] { mockDependency, mockAnotherDependency });

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response!.IntProperty.Should().Be(40);
            endpoint.Response.StringProperty.Should().Be("TestStringMockedStringAnotherMockedValue");
        }

        [Fact]
        public async Task Should_Handle_Endpoint_With_Logger_Only()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var mockLogger = Substitute.For<ILogger<NoDependencyEndpoint>>();

            var endpoint = EndpointFactory.CreateEndpoint<NoDependencyEndpoint>(logger: mockLogger);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response!.IntProperty.Should().Be(10);
            endpoint.Response.StringProperty.Should().Be(string.Empty);
            mockLogger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Handling NoDependencyEndpoint")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public async Task Should_Handle_Endpoint_With_HttpContextAccessor_Only()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 20, StringProperty = "Test"};

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Append("mock-header", "mock-value");

            var mockHttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            mockHttpContextAccessor.HttpContext.Returns(httpContext);

            var endpoint = EndpointFactory.CreateEndpoint<NoDependencyEndpoint>(httpContextAccessor: mockHttpContextAccessor);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response!.IntProperty.Should().Be(20);
            endpoint.Response.StringProperty.Should().Be("Test");
            endpoint.Response.HeaderValue.Should().Be("mock-value");
        }

        [Fact]
        public async Task Should_Handle_Endpoint_With_WebHostEnvironment_Only()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 30 };

            var mockEnvironment = Substitute.For<IWebHostEnvironment>();
            mockEnvironment.EnvironmentName.Returns("Production");

            var endpoint = EndpointFactory.CreateEndpoint<NoDependencyEndpoint>(webHostEnvironment: mockEnvironment);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.IntProperty.Should().Be(30);
            endpoint.Response.StringProperty.Should().Be(string.Empty);
            endpoint.Response.EnvironmentName.Should().Be("Production");
        }

        [Fact]
        public async Task Should_Handle_Endpoint_With_Constructor_Dependencies_Only()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 40 };

            var mockDependency = Substitute.For<IServiceDependency>();
            mockDependency.GetInt().Returns(5);
            mockDependency.GetString().Returns("MockedString");

            var endpoint = EndpointFactory.CreateEndpoint<SingleServiceDependencyEndpoint>(mockDependency);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.IntProperty.Should().Be(45);
            endpoint.Response.StringProperty.Should().Be("MockedString");
        }

        [Fact]
        public async Task Should_Handle_Endpoint_With_Logger_And_HttpContextAccessor()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 50 };

            var mockLogger = Substitute.For<ILogger<NoDependencyEndpoint>>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Append("mock-header", "mock-value");

            var mockHttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            mockHttpContextAccessor.HttpContext.Returns(httpContext);

            var endpoint = EndpointFactory.CreateEndpoint<NoDependencyEndpoint>(
                logger: mockLogger,
                httpContextAccessor: mockHttpContextAccessor);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.IntProperty.Should().Be(50);
            endpoint.Response.StringProperty.Should().Be(string.Empty);
            endpoint.Response.HeaderValue.Should().Be("mock-value");

            mockLogger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Handling NoDependencyEndpoint with IntProperty: 50")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public async Task Should_Handle_Endpoint_With_All_Custom_Dependencies()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 60, StringProperty = "RequestString" };

            var mockLogger = Substitute.For<ILogger<ServiceAndAnotherDependencyEndpoint>>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Append("mock-header", "mock-value");

            var mockHttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            mockHttpContextAccessor.HttpContext.Returns(httpContext);

            var mockEnvironment = Substitute.For<IWebHostEnvironment>();
            mockEnvironment.EnvironmentName.Returns("Staging");

            var mockDependency = Substitute.For<IServiceDependency>();
            mockDependency.GetInt().Returns(10);
            mockDependency.GetString().Returns("MockedString");

            var mockAnotherDependency = Substitute.For<IAnotherDependency>();
            mockAnotherDependency.GetAnotherValue().Returns("AnotherMockedValue");

            var endpoint = EndpointFactory.CreateEndpoint<ServiceAndAnotherDependencyEndpoint>(
                logger: mockLogger,
                httpContextAccessor: mockHttpContextAccessor,
                webHostEnvironment: mockEnvironment,
                constructorArgs: new object[] { mockDependency, mockAnotherDependency });

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response!.IntProperty.Should().Be(70);
            endpoint.Response.StringProperty.Should().Be("RequestStringMockedStringAnotherMockedValue");
            endpoint.Response.HeaderValue.Should().Be("mock-value");

            mockLogger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Handling ServiceAndAnotherDependencyEndpoint with IntProperty: 60")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public async Task Should_Log_Message_When_Logger_Is_Provided()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var mockLogger = Substitute.For<ILogger<NoDependencyEndpoint>>();

            var endpoint = EndpointFactory.CreateEndpoint<NoDependencyEndpoint>(logger: mockLogger);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            mockLogger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Handling NoDependencyEndpoint")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public async Task Should_Use_HttpContext_When_HttpContextAccessor_Is_Provided()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 20 };

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = "GET";
            httpContext.Items["CustomKey"] = "CustomValue";

            var mockHttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            mockHttpContextAccessor.HttpContext.Returns(httpContext);

            var endpoint = EndpointFactory.CreateEndpoint<NoDependencyEndpoint>(httpContextAccessor: mockHttpContextAccessor);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            httpContext.Request.Method.Should().Be("GET");
        }

        [Fact]
        public async Task Should_Use_WebHostEnvironment_When_Provided()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 30 };

            var mockEnvironment = Substitute.For<IWebHostEnvironment>();
            mockEnvironment.EnvironmentName.Returns("Production");
            mockEnvironment.ApplicationName.Returns("TestApp");

            var endpoint = EndpointFactory.CreateEndpoint<NoDependencyEndpoint>(webHostEnvironment: mockEnvironment);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            mockEnvironment.EnvironmentName.Should().Be("Production");
            mockEnvironment.ApplicationName.Should().Be("TestApp");
        }

        [Fact]
        public async Task Should_Use_Constructor_Dependencies_When_Provided()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 40 };

            var mockDependency = Substitute.For<IServiceDependency>();
            mockDependency.GetInt().Returns(5);
            mockDependency.GetString().Returns("MockedString");

            var endpoint = EndpointFactory.CreateEndpoint<SingleServiceDependencyEndpoint>(
                constructorArgs: new object[] { mockDependency });

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.IntProperty.Should().Be(45);
            endpoint.Response.StringProperty.Should().Be("MockedString");

            mockDependency.Received(1).GetInt();
            mockDependency.Received(1).GetString();
        }

        [Fact]
        public async Task Should_Use_All_Custom_Dependencies()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 60 };

            var mockLogger = Substitute.For<ILogger<ServiceAndAnotherDependencyEndpoint>>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = "PUT";

            var mockHttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            mockHttpContextAccessor.HttpContext.Returns(httpContext);

            var mockEnvironment = Substitute.For<IWebHostEnvironment>();
            mockEnvironment.EnvironmentName.Returns("Staging");
            mockEnvironment.ApplicationName.Returns("StagingApp");

            var mockDependency = Substitute.For<IServiceDependency>();
            mockDependency.GetInt().Returns(10);
            mockDependency.GetString().Returns("MockedString");

            var mockAnotherDependency = Substitute.For<IAnotherDependency>();
            mockAnotherDependency.GetAnotherValue().Returns("AnotherMockedValue");

            var endpoint = EndpointFactory.CreateEndpoint<ServiceAndAnotherDependencyEndpoint>(
                logger: mockLogger,
                httpContextAccessor: mockHttpContextAccessor,
                webHostEnvironment: mockEnvironment,
                constructorArgs: new object[] { mockDependency, mockAnotherDependency });

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            mockLogger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Handling ServiceAndAnotherDependencyEndpoint")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());

            httpContext.Request.Method.Should().Be("PUT");

            mockEnvironment.EnvironmentName.Should().Be("Staging");
            mockEnvironment.ApplicationName.Should().Be("StagingApp");

            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.IntProperty.Should().Be(70);
            endpoint.Response.StringProperty.Should().Be("MockedStringAnotherMockedValue");
        }
    }
}