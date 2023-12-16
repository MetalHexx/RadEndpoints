using System.Collections.Specialized;

namespace RadEndpoints
{
    public static class RadUriExtensions
    {
        public static Uri AppendUri(this Uri uri, string path, NameValueCollection queries)
        {
            ArgumentNullException.ThrowIfNull(uri);

            if (uri.ToString().EndsWith("/") && path.StartsWith("/"))
            {
                path = path.TrimStart('/');
            }
            return new Uri($"{uri}{path}?{queries}");
        }
    }
}
