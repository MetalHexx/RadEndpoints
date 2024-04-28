namespace RadEndpoints
{
    public class RadBytes
    {
        public byte[] Bytes { get; set; } = default!;
        public string? ContentType { get; set; }
        public string? FileDownloadName { get; set; }
        public bool EnableRangeProcessing { get; set; }
        public DateTimeOffset? LastModified { get; set; }
    }
}
