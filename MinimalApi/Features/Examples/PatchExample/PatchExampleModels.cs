using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.PatchExample
{
    public record PartialExample( string? FirstName, string? LastName);
    public class PatchExampleRequest
    {
        [FromRoute]
        public int Id { get; set; }
        [FromBody]
        public ExampleDto Example { get; set; } = new();
    }
    public class PatchExampleResponse : ExampleDto { }
}
