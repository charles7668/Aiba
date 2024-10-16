using SkiaSharp;

namespace Aiba.Services
{
    public static class ImageService
    {
        public static SKEncodedImageFormat? GetImageFormat(Stream inputStream)
        {
            using var codec = SKCodec.Create(inputStream);
            return codec.EncodedFormat;
        }

        public static SKBitmap Resize(Stream inputStream, int targetWidth, int targetHeight)
        {
            using var originalBitmap = SKBitmap.Decode(inputStream);
            var resizedBitmap = new SKBitmap(targetWidth, targetHeight);
            originalBitmap.ScalePixels(resizedBitmap, new SKSamplingOptions(SKFilterMode.Linear));
            return resizedBitmap;
        }

        public static void SaveBitmapToFile(SKBitmap bitmap, string filePath, SKEncodedImageFormat format,
            int quality = 100)
        {
            using var image = SKImage.FromBitmap(bitmap);
            using SKData? data = image.Encode(format, quality);
            using FileStream stream = File.OpenWrite(filePath);
            data.SaveTo(stream);
        }
    }
}