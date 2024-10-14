namespace Aiba.Model
{
    public class MediaInfoScanCallback(MediaInfo mediaInfo, string coverExt)
    {
        public MediaInfo MediaInfo { get; set; } = mediaInfo;
        public string CoverExt { get; set; } = coverExt;
    }
}