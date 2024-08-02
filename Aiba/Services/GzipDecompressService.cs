using System.IO.Compression;

namespace Aiba.Services
{
    public class GzipDecompressService : IDecompressService
    {
        public string[] SupportFormat { get; } = ["gzip"];

        public Task<Stream> DecompressAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new ArgumentException("Stream must be readable", nameof(stream));
            }

            Stream decompressedStream = new GZipStream(stream, CompressionMode.Decompress);

            return Task.FromResult(decompressedStream);
        }
    }
}