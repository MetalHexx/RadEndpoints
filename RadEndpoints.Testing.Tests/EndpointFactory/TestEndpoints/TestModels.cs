namespace RadEndpoints.Testing.Tests
{
    public class TestRequest
    {
        public string StringProperty { get; set; } = string.Empty;
        public int IntProperty { get; set; }
    }

    public class TestResponse
    {
        public string StringProperty { get; set; } = string.Empty;
        public int IntProperty { get; set; }
        public string HeaderValue { get; set; } = string.Empty;
        public string EnvironmentName { get; set; } = string.Empty;
    }
}