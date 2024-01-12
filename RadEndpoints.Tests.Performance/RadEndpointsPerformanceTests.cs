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
            .Mean;;
        
        // Ensure that RadEndpoints mean is within X% from Minimal Api mean value.
        // It should be very close to each other, if RadEndpoints do not introduce additional overhead.
        var tolerance = minApiMean / 100; // 1%.
        
        radEndpointsMean.Should().BeApproximately(minApiMean, precision: tolerance);
    }
}