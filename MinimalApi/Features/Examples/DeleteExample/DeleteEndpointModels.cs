namespace MinimalApi.Features.Examples.DeleteExample
{
    public class DeleteExampleRequest : RadRequest
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

    public class DeleteExampleResponse : RadResponse { }
}
