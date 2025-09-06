using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace RadEndpoints.Testing.Tests
{
    /// <summary>
    /// Example tests demonstrating the new RadEndpoint testing extension methods
    /// using the standard TypedResults pattern that developers are familiar with from minimal APIs.
    /// </summary>
    public class TypedResultsTestExtensionsTests
    {
        [Fact]
        public async Task When_EndpointSendsOkResponse_GetResult_ShouldReturnTypedResult()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 5 };
            var endpoint = EndpointFactory.CreateEndpoint<TestOkEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            var result = endpoint.GetResult<Ok<TestResponse>>();

            result!.Value.Should().NotBeNull();
            result.Value!.IntProperty.Should().Be(6);

            endpoint.HasResult().Should().BeTrue();
            endpoint.HasProblem().Should().BeFalse();
            endpoint.GetStatusCode().Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_EndpointSendsError_GetResult_ShouldReturnProblemHttpResult()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestErrorEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            var result = endpoint.GetResult<ProblemHttpResult>();
            result!.StatusCode.Should().Be(500);
            result.ProblemDetails.Title.Should().Be("Test Error");

            endpoint.HasResult().Should().BeTrue();
            endpoint.GetStatusCode().Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task When_EndpointSendsCreatedResponse_GetResult_ShouldReturnCreatedResult()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestCreatedEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            var result = endpoint.GetResult<Created<TestResponse>>();
            result!.Location.Should().Be("/test/100");
            result.Value.Should().NotBeNull();
            result.Value!.IntProperty.Should().Be(100);

            endpoint.GetStatusCode().Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task When_EndpointSendsProblem_GetProblem_ShouldReturnTypedProblem()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestRadProblemEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            var problem = endpoint.GetProblem<NotFoundError>();
            problem!.Message.Should().Be("Resource not found");

            endpoint.HasProblem().Should().BeTrue();
            endpoint.HasResult().Should().BeFalse();
        }

        [Fact]
        public async Task When_EndpointReturnsNotFound_GetResult_ShouldReturnNotFoundResult()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestNotFoundEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            var result = endpoint.GetResult<NotFound<string>>();
            result!.Value.Should().Be("Resource not found");

            endpoint.GetStatusCode().Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task When_EndpointSendsValidationError_GetResult_ShouldReturnValidationProblem()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestValidationErrorEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            var result = endpoint.GetResult<ValidationProblem>();
            result!.ProblemDetails.Title.Should().Be("Validation failed");

            endpoint.GetStatusCode().Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task When_EndpointSendsRedirect_GetResult_ShouldReturnRedirectResult()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestRedirectEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            var result = endpoint.GetResult<RedirectHttpResult>();
            result!.Url.Should().Be("/redirected");
            result.Permanent.Should().BeFalse();
            result.PreserveMethod.Should().BeFalse();

            endpoint.GetStatusCode().Should().Be(HttpStatusCode.Found);
        }

        [Fact]
        public async Task When_EndpointSendsConflict_GetResult_ShouldReturnConflictResult()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestConflictEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            var result = endpoint.GetResult<Conflict<string>>();
            result!.Value.Should().Be("Resource conflict");

            endpoint.GetStatusCode().Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task When_EndpointSendsUnauthorized_GetResult_ShouldReturnProblemHttpResult()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestUnauthorizedEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            var result = endpoint.GetResult<ProblemHttpResult>();
            result!.StatusCode.Should().Be(401);
            result.ProblemDetails.Title.Should().Be("Unauthorized access");

            endpoint.GetStatusCode().Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task When_EndpointSendsForbidden_GetResult_ShouldReturnProblemHttpResult()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestForbiddenEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            var result = endpoint.GetResult<ProblemHttpResult>();
            result!.StatusCode.Should().Be(403);
            result.ProblemDetails.Title.Should().Be("Forbidden access");

            endpoint.GetStatusCode().Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task When_EndpointSendsAuthenticationChallenge_GetResult_ShouldReturnUnauthorizedHttpResult()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestAuthenticationChallengeEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            var result = endpoint.GetResult<UnauthorizedHttpResult>();

            endpoint.GetStatusCode().Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task When_EndpointSendsAuthenticationForbid_GetResult_ShouldReturnForbidHttpResult()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestAuthenticationForbidEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            var result = endpoint.GetResult<ForbidHttpResult>();
            endpoint.GetStatusCode().Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task When_EndpointSendsParameterlessOk_GetResult_ShouldReturnOkResult()
        {
            var request = new TestRequest { IntProperty = 50 };
            var endpoint = EndpointFactory.CreateEndpoint<TestParameterlessOkEndpoint>();

            await endpoint.Handle(request, CancellationToken.None);

            var result = endpoint.GetResult<Ok<TestResponse>>();
            result!.Value.Should().NotBeNull();
            result.Value!.IntProperty.Should().Be(50);
            endpoint.GetStatusCode().Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_EndpointSendsCreatedAtWithResponseProperty_GetResult_ShouldReturnCreatedResult()
        {
            var request = new TestRequest { IntProperty = 75 };
            var endpoint = EndpointFactory.CreateEndpoint<TestCreatedAtSingleParamEndpoint>();

            await endpoint.Handle(request, CancellationToken.None);

            var result = endpoint.GetResult<Created<TestResponse>>();
            result!.Location.Should().Be("/test/75");
            result.Value!.IntProperty.Should().Be(75);
            endpoint.GetStatusCode().Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task When_EndpointSendsExternalError_GetResult_ShouldReturnProblemHttpResult()
        {
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestExternalErrorEndpoint>();

            await endpoint.Handle(request, CancellationToken.None);

            var result = endpoint.GetResult<ProblemHttpResult>();
            result!.StatusCode.Should().Be(502);
            result.ProblemDetails.Title.Should().Be("External service error");
            endpoint.GetStatusCode().Should().Be(HttpStatusCode.BadGateway);
        }

        [Fact]
        public async Task When_EndpointSendsExternalTimeout_GetResult_ShouldReturnProblemHttpResult()
        {
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestExternalTimeoutEndpoint>();

            await endpoint.Handle(request, CancellationToken.None);

            var result = endpoint.GetResult<ProblemHttpResult>();
            result!.StatusCode.Should().Be(504);
            result.ProblemDetails.Title.Should().Be("External service timeout");
            endpoint.GetStatusCode().Should().Be(HttpStatusCode.GatewayTimeout);
        }

        [Fact]
        public async Task When_EndpointSendsBytes_GetResult_ShouldReturnFileContentHttpResult()
        {
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestBytesEndpoint>();

            await endpoint.Handle(request, CancellationToken.None);

            var result = endpoint.GetResult<FileContentHttpResult>();
            result!.FileContents.ToArray().Should().BeEquivalentTo(new byte[] { 1, 2, 3, 4 });
            result.ContentType.Should().Be("application/octet-stream");
            endpoint.GetStatusCode().Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_EndpointSendsStream_GetResult_ShouldReturnFileStreamHttpResult()
        {
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestStreamEndpoint>();

            await endpoint.Handle(request, CancellationToken.None);

            var result = endpoint.GetResult<FileStreamHttpResult>();
            result!.ContentType.Should().Be("text/plain");
            result.FileDownloadName.Should().Be("test.txt");
            endpoint.GetStatusCode().Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_EndpointSendsFile_GetResult_ShouldReturnPhysicalFileHttpResult()
        {
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestFileEndpoint>();

            await endpoint.Handle(request, CancellationToken.None);

            var result = endpoint.GetResult<PhysicalFileHttpResult>();
            result!.FileName.Should().Be("/path/to/file.txt");
            result.ContentType.Should().Be("text/plain");
            endpoint.GetStatusCode().Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_EndpointSendsRedirectWithParameters_GetResult_ShouldReturnRedirectResult()
        {
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestRedirectWithParamsEndpoint>();

            await endpoint.Handle(request, CancellationToken.None);

            var result = endpoint.GetResult<RedirectHttpResult>();
            result!.Url.Should().Be("/permanent-redirect");
            result.Permanent.Should().BeTrue();
            result.PreserveMethod.Should().BeTrue();
            endpoint.GetStatusCode().Should().Be(HttpStatusCode.Found);
        }

        [Fact]
        public async Task When_EndpointSendsProblemHttpResult_GetProblem_ShouldReturnProblemHttpResult()
        {
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestDirectProblemHttpResultEndpoint>();

            await endpoint.Handle(request, CancellationToken.None);

            var problem = endpoint.GetProblem<ProblemHttpResult>();
            problem!.StatusCode.Should().Be(418);
            problem.ProblemDetails.Title.Should().Be("I'm a teapot");
            endpoint.HasProblem().Should().BeTrue();
        }

        [Fact]
        public async Task When_EndpointSendsValidationProblem_GetProblem_ShouldReturnValidationProblem()
        {
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestDirectValidationProblemEndpoint>();

            await endpoint.Handle(request, CancellationToken.None);

            var problem = endpoint.GetProblem<ValidationProblem>();
            problem!.ProblemDetails.Title.Should().Be("Validation Error");
            endpoint.HasProblem().Should().BeTrue();
        }

        [Fact]
        public async Task When_EndpointDoesNotSendResult_GetResult_ShouldThrowRadTestException()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestParameterlessOkEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            Action act = () => endpoint.GetResult<NotFound<string>>();

            // Assert
            act.Should().Throw<RadTestException>()
                .WithMessage("TypedResult of type NotFound`1 was not returned by the endpoint.");
        }

        [Fact]
        public async Task When_EndpointDoesNotSendProblem_GetProblem_ShouldThrowRadTestException()
        {
            // Arrange
            var request = new TestRequest { IntProperty = 10 };
            var endpoint = EndpointFactory.CreateEndpoint<TestParameterlessOkEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            Action act = () => endpoint.GetProblem<ValidationProblem>();

            // Assert
            act.Should().Throw<RadTestException>()
                .WithMessage("TypedProblem of type ValidationProblem was not returned by the endpoint.");
        }
    }
}