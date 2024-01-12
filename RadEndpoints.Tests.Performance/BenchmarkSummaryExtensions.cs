using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Reports;

namespace RadEndpoints.Tests.Performance;

internal static class BenchmarkSummaryExtensions
{
    public static BenchmarkReport GetReportOrThrow(this Summary summary, string benchmarkName)
    {
        return summary.Reports.Single(r =>
            r.BenchmarkCase.Descriptor.DisplayInfo.Contains(benchmarkName));
    }
    
    public static Statistics GetStatisticsOrThrow(this BenchmarkReport report)
    {
        return report.ResultStatistics 
               ?? throw new InvalidOperationException("Unable to retrieve statistics from the benchmark report");
    }
}