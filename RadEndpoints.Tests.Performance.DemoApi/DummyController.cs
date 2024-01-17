using Microsoft.AspNetCore.Mvc;

namespace RadEndpoints.Tests.Performance.DemoApi;

[ApiController]
public class DummyController
{
    [HttpGet("/getusingcontroller")]
    public int GetUsingController()
    {
        return 1;
    }
}