using Microsoft.Net.Http.Headers;

namespace RadEndpoints
{
    public class RadFile
    {
        public string Path { get; set; } = null!;
        public string? ContentType { get; set; }
        public string? FileDownloadName { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public EntityTagHeaderValue? EntityTag { get; set; }
    }
}
