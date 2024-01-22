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

    public class RadBytesResponse : RadResponse
    {
        public byte[] Bytes { get; set; } = default!;
        public string? ContentType { get; set; }
        public string? FileDownloadName { get; set; }
        public bool EnableRangeProcessing { get; set; }
        public DateTimeOffset? LastModified { get; set; }
    }
}
