using System.IO.Compression;

namespace Aiba.Services
{
    public class DeflateDecompressService : IDecompressService
    {
        public string[] SupportFormat { get; } = ["deflate"];

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

            Stream decompressedStream = new DeflateStream(stream, CompressionMode.Decompress);

            return Task.FromResult(decompressedStream);
        }
    }
}