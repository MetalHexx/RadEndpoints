using Microsoft.Net.Http.Headers;

namespace RadEndpoints
{
    public class RadStream
    {
        public Stream Stream { get; set; } = default!;
        public string? ContentType { get; set; }
        public string? FileDownloadName { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public EntityTagHeaderValue? EntityTag { get; set; }
        public bool EnableRangeProcessing { get; set; }
        
    }
}
