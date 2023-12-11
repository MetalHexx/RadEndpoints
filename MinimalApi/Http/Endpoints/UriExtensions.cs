using System.Collections.Specialized;

namespace MinimalApi.Http.Endpoints
{
    public static class UriExtensions
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
