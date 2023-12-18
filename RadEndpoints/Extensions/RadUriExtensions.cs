using System.Collections.Specialized;

namespace RadEndpoints
{
    public static class RadUriExtensions
    {
        public static Uri Combine(this Uri baseUri, string path, NameValueCollection queries)
        {
            ArgumentNullException.ThrowIfNull(baseUri);

            if (baseUri.ToString().EndsWith("/") && path.StartsWith("/"))
            {
                path = path.TrimStart('/');
            }
            return new Uri($"{baseUri}{path}?{queries}");
        }

        public static Uri Combine(this Uri baseUri, string path)
        {
            ArgumentNullException.ThrowIfNull(baseUri);

            if (baseUri.ToString().EndsWith("/") && path.StartsWith("/"))
            {
                path = path.TrimStart('/');
            }
            return new Uri($"{baseUri}{path}");
        }
    }
}
