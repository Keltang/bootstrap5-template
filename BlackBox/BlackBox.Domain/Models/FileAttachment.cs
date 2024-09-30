namespace BlackBox.Domain.Models
{
    public class FileAttachment
    {
        public string DisplayName { get; }
        public string FilePath { get; }

        public FileAttachment(string displayName, string filePath)
        {
            DisplayName = displayName;
            FilePath = filePath;
        }

    }
}
