using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using FluentAssertions;
using Xunit.Abstractions;

namespace RadEndpoints.Tests.Performance;

public class RadEndpointsPerformanceTests
{
    private readonly IConfig _benchmarkRunConfig;

    public RadEndpointsPerformanceTests(ITestOutputHelper testOutputHelper)
    {
        var testLogger = new TestBenchmarkLogger(testOutputHelper);
        _benchmarkRunConfig = DefaultConfig.Instance
            .WithOption(ConfigOptions.DisableOptimizationsValidator, true)
            .AddLogger(testLogger);
    }

    [Fact]
    public void InvokeGetRadEndpoint_ReturnSimpleResponse_PerfMustBeCloseToMinApiEndpoint()
    {
        // Act.
        var benchmarkSummary = BenchmarkRunner.Run<ApiBenchmark>(_benchmarkRunConfig);
        
        // Assert.
        var minApiMean = benchmarkSummary
            .GetReportOrThrow(nameof(ApiBenchmark.InvokeMinimalApiEndpoint))
            .GetStatisticsOrThrow()
            .Mean;

        var radEndpointsMean = benchmarkSummary
            .GetReportOrThrow(nameof(ApiBenchmark.InvokeRadEndpoint))
            .GetStatisticsOrThrow()
            .Mean;
        
        // Ensure that RadEndpoints mean is within X% from Minimal Api mean value.
        // It should be very close to each other, if RadEndpoints do not introduce additional overhead.
        
        // This tolerance is empirically established. RadEndpoints have a small overhead compared to Min. Apis
        // due to a few extra steps done when an Endpoints are executed. This overhead is extremely small, and for any
        // endpoints doing meaningful work, this difference is negligible.
        var tolerance = 5000; // 5 microseconds.
        
        radEndpointsMean.Should().BeApproximately(minApiMean, precision: tolerance);
    }
}