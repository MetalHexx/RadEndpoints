using FluentAssertions.Primitives;
using MinimalApi.Http.Endpoints;

namespace MinimalApi.Tests.Integration.Common
{
    public static class RadResponseAssertionsExtensions
    {
        public static RadResponseBuilder BeRadResponse<T>(this ObjectAssertions assertions) where T : RadResponse
        {
            var response = assertions.Subject.Should().BeOfType<T>().Subject;

            var castedResponse = response as RadResponse;

            return castedResponse is null
                ? throw new RadTestException("The object is not a RadResponse or derived type.")
                : new RadResponseBuilder(castedResponse);
        }
    }

    public class RadResponseBuilder(RadResponse response)
    {
        public RadResponseBuilder WithMessage(string message)
        {
            response.Message.Should().Be(message);
            return this;
        }
    }

    public static class ProblemDetailsAssertionsExtensions
    {
        public static ProblemDetailsAssertionBuilder BeProblem(this ObjectAssertions assertions)
        {
            var problemDetails = assertions.Subject.Should().BeOfType<ProblemDetails>().Subject;
            return new ProblemDetailsAssertionBuilder(problemDetails);
        }
    }

    public class ProblemDetailsAssertionBuilder
    {
        private readonly ProblemDetails _problemDetails;

        public ProblemDetailsAssertionBuilder(ProblemDetails problemDetails)
        {
            _problemDetails = problemDetails;
        }

        public ProblemDetailsAssertionBuilder WithKey(string expectedKey)
        {
            _problemDetails.Extensions.Should().ContainKey(expectedKey);
            return this;
        }

        public ProblemDetailsAssertionBuilder WithKeyAndValue(string expectedKey, string expectedValue)
        {
            _problemDetails.Extensions.Should().ContainKey(expectedKey)
                .WhoseValue!.ToString().Should().Be(expectedValue);
            return this;
        }

        public ProblemDetailsAssertionBuilder WithTitle(string title)
        {
            _problemDetails.Title.Should().Be(title);
            return this;
        }

        public ProblemDetailsAssertionBuilder WithStatus(int status)
        {
            _problemDetails.Status.Should().Be(status);
            return this;
        }

        public ProblemDetailsAssertionBuilder WithDetail(string detail)
        {
            _problemDetails.Detail.Should().Be(detail);
            return this;
        }

        public ProblemDetailsAssertionBuilder WithInstance(string instance)
        {
            _problemDetails.Instance.Should().Be(instance);
            return this;
        }

        public ProblemDetailsAssertionBuilder WithType(string type)
        {
            _problemDetails.Type.Should().Be(type);
            return this;
        }
    }
}