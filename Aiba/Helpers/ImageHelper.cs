namespace Aiba.Helpers
{
    public static class ImageHelper
    {
        public static string GetImageContentType(string extension)
        {
            string ext = extension.ToLowerInvariant();
            return ext switch
            {
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}