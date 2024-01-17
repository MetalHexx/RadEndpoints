using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RadEndpoints.Tests.Performance;

[SimpleJob]
[MemoryDiagnoser]
public class ApiBenchmark
{
    private HttpClient _httpClient = null!; // Field will always be initialized by setup.

    [GlobalSetup]
    public void Setup()
    {
        var appFactory = new WebApplicationFactory<Program>();

        _httpClient = appFactory.CreateClient();
    }
    
    [Benchmark]
    public async Task InvokeMinimalApiEndpoint()
    {
        await _httpClient.GetAsync("getusingminapi");
    }
    
    [Benchmark]
    public async Task InvokeRadEndpoint()
    {
        await _httpClient.GetAsync("getusingradendpoints");
    }
}