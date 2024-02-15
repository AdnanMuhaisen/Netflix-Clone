namespace Netflix_Clone.Domain.Options
{
    public class ContentTVShowOptions
    {
        public string TargetDirectoryToCompressTo { get; set; } = string.Empty;
        public string TargetDirectoryToSaveTo { get; set; } = string.Empty;
        public string DefaultPathToDownload { get; set; } = string.Empty;
    }
}
