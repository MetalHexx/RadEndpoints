using System.Text;
using BenchmarkDotNet.Loggers;
using Xunit.Abstractions;

namespace RadEndpoints.Tests.Performance;

public sealed class TestBenchmarkLogger(ITestOutputHelper helper) : ILogger
{
    private readonly StringBuilder _sb = new();
    
    public string Id => nameof(TestBenchmarkLogger);

    public int Priority => 0;

    public void Write(LogKind logKind, string text)
    {
        _sb.Append(text);
    }

    public void WriteLine()
    {
        helper.WriteLine(_sb.ToString());

        _sb.Clear();
    }

    public void WriteLine(LogKind logKind, string text)
    {
        _sb.Append(text);
        
        helper.WriteLine(_sb.ToString());

        _sb.Clear();
    }

    public void Flush()
    {
        _sb.Clear();
    }
}