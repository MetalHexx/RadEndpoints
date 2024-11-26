using RadEndpoints;

namespace MinimalApi.Features.Forms.PostForm
{
    public class PostFormRequest 
    {
        [FromForm]
        public string? TestFormField { get; set; }
    }

    public class PostFormRequestValidator : AbstractValidator<PostFormRequest>
    {
        public PostFormRequestValidator()
        {
            //TODO: Add validation rules here
        }
    }

    public class PostFormResponse
    {
        public string Message { get; set; } = "Success!";
    }
}