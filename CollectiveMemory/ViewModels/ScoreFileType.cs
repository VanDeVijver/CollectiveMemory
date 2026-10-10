namespace CollectiveMemory.ViewModels
{
    /// <summary>Recognises the score files the songbook accepts, by content, never by file name.</summary>
    public static class ScoreFileType
    {
        // Scores are stored in the database, so keep them small (a scanned page is ~0.2-2 MB).
        public const long MaxBytes = 10 * 1024 * 1024;

        /// <summary>"application/pdf", "image/png", "image/jpeg", or null when it's none of those.</summary>
        public static string? Sniff(ReadOnlySpan<byte> head)
        {
            if (head.Length >= 5 && head[0] == '%' && head[1] == 'P' && head[2] == 'D' && head[3] == 'F' && head[4] == '-')
                return "application/pdf";
            if (head.Length >= 8 && head[0] == 0x89 && head[1] == 'P' && head[2] == 'N' && head[3] == 'G'
                && head[4] == 0x0D && head[5] == 0x0A && head[6] == 0x1A && head[7] == 0x0A)
                return "image/png";
            if (head.Length >= 3 && head[0] == 0xFF && head[1] == 0xD8 && head[2] == 0xFF)
                return "image/jpeg";
            return null;
        }
    }
}
