namespace Aiba.Model;

public class MediaInfo
{
    public string Name { get; set; } = string.Empty;

    public string[] Author { get; set; } = [];

    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string[] Genres { get; set; } = [];

    public string[] Tags { get; set; } = [];

    public ChapterInfo[] Chapters { get; set; } = [];

    public string ProviderName { get; set; } = string.Empty;

    public string ProviderUrl { get; set; } = string.Empty;
}