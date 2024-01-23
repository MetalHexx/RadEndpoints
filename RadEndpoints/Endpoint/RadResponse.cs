using Microsoft.Net.Http.Headers;

namespace RadEndpoints
{
    public class RadResponse
    {
        public string Message { get; set; } = string.Empty;
    }

    public class RadResponse<T> : RadResponse
    {
        public T? Data { get; set; } = default!;
    }

    public class RadResponseStream : RadResponse
    {
        public Stream Stream { get; set; } = default!;
        public string? ContentType { get; set; }
        public string? FileDownloadName { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public EntityTagHeaderValue? EntityTag { get; set; }
        public bool EnableRangeProcessing { get; set; }
        
    }

    public class RadResponseBytes : RadResponse
    {
        public byte[] Bytes { get; set; } = default!;
        public string? ContentType { get; set; }
        public string? FileDownloadName { get; set; }
        public bool EnableRangeProcessing { get; set; }
        public DateTimeOffset? LastModified { get; set; }
    }

    public class RadResponseFile : RadResponse
    {
        public string Path { get; set; } = null!;
        public string? ContentType { get; set; }
        public string? FileDownloadName { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public EntityTagHeaderValue? EntityTag { get; set; }
    }
}
