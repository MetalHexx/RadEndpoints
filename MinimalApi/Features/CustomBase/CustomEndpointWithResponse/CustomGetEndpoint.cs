using MinimalApi.Features.CustomBase._common;

namespace MinimalApi.Features.CustomBase.GetAwesomeExample
{   
    /// <summary>
    /// This endpoint uses a custom base to demonstrate how you can custom 
    /// tailor RadEndpoints to suit the needs different applications and coding 
    /// standards.
    /// </summary>
    public sealed class CustomGetEndpoint : CustomBaseEndpoint<CustomGetRequest, CustomGetResponse>
    {
        public override void Configure()
        {
            Get("custom-base/{id}")
                .WithDocument(tag: "Custom Base Endpoint", desc: "This endpoint uses a custom base to demonstrate how you can custom tailor RadEndpoints to suit the needs different applications and coding standards.");
        }

        public override async Task<CustomGetResponse> Handle(CustomGetRequest r, CancellationToken ct)
        {
            await Task.Delay(1, ct);

            if(r.Id == 1)
            {
                return new CustomGetResponse() 
                { 
                    Data = "Some Data", 
                    Message = "Success"
                };
            }
            return BadRequest("This is a bad request");
        }
    }
}
