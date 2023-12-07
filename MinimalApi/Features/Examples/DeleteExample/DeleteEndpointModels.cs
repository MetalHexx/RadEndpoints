namespace MinimalApi.Features.Examples.DeleteExample
{
    public class DeleteExampleResponse : EndpointResponse { }

    public class DeleteExampleRequest
    {
        public int Id { get; set; }
    }

    public class DeleteExampleRequestValidator : AbstractValidator<DeleteExampleRequest>
    {
        public DeleteExampleRequestValidator()
        {
            RuleFor(e => e.Id).GreaterThan(0);
        }
    }
}
