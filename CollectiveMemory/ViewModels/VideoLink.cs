using System.Text.RegularExpressions;

namespace CollectiveMemory.ViewModels
{
    /// <summary>Recognises the two kinds of clip links the site can show.</summary>
    public static partial class VideoLink
    {
        private static readonly string[] YouTubeHosts =
            ["youtube.com", "www.youtube.com", "m.youtube.com", "youtube-nocookie.com", "www.youtube-nocookie.com", "youtu.be"];

        [GeneratedRegex("^[A-Za-z0-9_-]{11}$")]
        private static partial Regex VideoId();

        /// <summary>watch?v=, youtu.be/, /shorts/, /embed/ and /live/ links to a privacy-friendly embed URL.</summary>
        public static bool TryYouTube(string? url, out string embedUrl)
        {
            embedUrl = string.Empty;
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)
                || (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
                || !YouTubeHosts.Contains(uri.Host.ToLowerInvariant()))
                return false;

            string? id;
            if (uri.Host.Equals("youtu.be", StringComparison.OrdinalIgnoreCase))
                id = uri.AbsolutePath.Trim('/');
            else if (uri.AbsolutePath.Equals("/watch", StringComparison.OrdinalIgnoreCase))
                id = System.Web.HttpUtility.ParseQueryString(uri.Query)["v"];
            else
            {
                var parts = uri.AbsolutePath.Trim('/').Split('/');
                id = parts.Length == 2 && parts[0] is "embed" or "shorts" or "live" ? parts[1] : null;
            }

            if (id is null || !VideoId().IsMatch(id)) return false;
            embedUrl = $"https://www.youtube-nocookie.com/embed/{id}";
            return true;
        }

        /// <summary>
        /// Identifies an uploaded file by its first bytes (never by its name): "video/mp4", "video/webm" or null.
        /// </summary>
        public static string? SniffVideoType(ReadOnlySpan<byte> head)
        {
            if (head.Length >= 12 && head[4] == 'f' && head[5] == 't' && head[6] == 'y' && head[7] == 'p')
                return "video/mp4";
            if (head.Length >= 4 && head[0] == 0x1A && head[1] == 0x45 && head[2] == 0xDF && head[3] == 0xA3)
                return "video/webm";
            return null;
        }

        /// <summary>An https link (or a site-relative path) to an .mp4 or .webm file.</summary>
        public static bool IsVideoFile(string? url, bool allowSiteRelative = false)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;
            string path;
            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                if (uri.Scheme != Uri.UriSchemeHttps) return false;
                path = uri.AbsolutePath;
            }
            else if (allowSiteRelative && url.StartsWith('/') && !url.StartsWith("//"))
                path = url;
            else return false;

            return path.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase)
                || path.EndsWith(".webm", StringComparison.OrdinalIgnoreCase);
        }
    }
}
