using RadEndpoints;

namespace MinimalApi.Features.ParameterTestEndpoints.EmptyStringTestEndpoints
{
    /// <summary>
    /// Test endpoint for verifying empty string handling in complex query parameters with route parameters
    /// </summary>
    public class EmptyQueryStringRequest
    {
        [FromRoute]
        public string DeviceId { get; set; } = string.Empty;

        [FromRoute]
        public string StorageType { get; set; } = string.Empty;

        [FromQuery]
        public string? Path { get; set; }

        [FromQuery]
        public string? Filter { get; set; }

        [FromQuery]
        public string? SortBy { get; set; }
    }

    public class EmptyQueryStringRequestValidator : AbstractValidator<EmptyQueryStringRequest>
    {
        public EmptyQueryStringRequestValidator()
        {
            RuleFor(x => x.DeviceId)
                .NotEmpty()
                .WithMessage("DeviceId is required.")
                .Length(8)
                .WithMessage("DeviceId must be exactly 8 characters long.");

            RuleFor(x => x.StorageType)
                .NotEmpty()
                .WithMessage("StorageType is required.")
                .Must(st => new[] { "SD", "USB", "EMMC", "FLASH" }.Contains(st.ToUpperInvariant()))
                .WithMessage("StorageType must be one of: SD, USB, EMMC, FLASH.");

            RuleFor(x => x.Path)
                .NotEmpty()
                .WithMessage("Path cannot be empty.")
                .Must(path => path!.StartsWith("/"))
                .WithMessage("Path must start with '/'.");

            // Only validate Filter if it's not null AND not empty
            RuleFor(x => x.Filter)
                .NotEmpty()
                .When(x => x.Filter != null)
                .WithMessage("Filter cannot be empty when provided.");

            // Only validate SortBy if it's not null AND not empty  
            RuleFor(x => x.SortBy)
                .NotEmpty()
                .When(x => x.SortBy != null)
                .WithMessage("SortBy cannot be empty when provided.");
        }
    }

    public class EmptyQueryStringResponse
    {
        public string DeviceId { get; set; } = string.Empty;
        public string StorageType { get; set; } = string.Empty;
        public string? Path { get; set; }
        public string? Filter { get; set; }
        public string? SortBy { get; set; }
        public string Message { get; set; } = "Query string parameters processed successfully";
        public bool AllQueryParametersReceived { get; set; }
        public int ProcessedParametersCount { get; set; }
    }

    public class EmptyQueryStringEndpoint : RadEndpoint<EmptyQueryStringRequest, EmptyQueryStringResponse>
    {
        public override void Configure()
        {
            Get("/test/query-strings/{deviceId}/storage/{storageType}")
                .Produces<EmptyQueryStringResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithTags("ParameterTests")
                .WithDescription("Test endpoint for empty string handling in complex query string scenarios with multiple parameters");
        }

        public override async Task Handle(EmptyQueryStringRequest r, CancellationToken ct)
        {
            await Task.CompletedTask;

            var processedCount = 0;
            if (!string.IsNullOrEmpty(r.Path)) processedCount++;
            if (!string.IsNullOrEmpty(r.Filter)) processedCount++;
            if (!string.IsNullOrEmpty(r.SortBy)) processedCount++;

            Response = new EmptyQueryStringResponse
            {
                DeviceId = r.DeviceId,
                StorageType = r.StorageType,
                Path = r.Path,
                Filter = r.Filter,
                SortBy = r.SortBy,
                AllQueryParametersReceived = r.Path != null && r.Filter != null && r.SortBy != null,
                ProcessedParametersCount = processedCount,
                Message = $"Processed {processedCount} query string parameters for {r.StorageType} storage on device {r.DeviceId}"
            };

            Send();
        }
    }
}