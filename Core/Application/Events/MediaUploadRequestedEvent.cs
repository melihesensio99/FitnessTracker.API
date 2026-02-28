namespace Application.Events
{
    public class MediaUploadRequestedEvent
    {
        public string TempFilePath { get; set; } = string.Empty;
        public string? FolderName { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
    }
}
