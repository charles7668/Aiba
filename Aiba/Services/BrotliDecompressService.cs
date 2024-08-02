using System.IO.Compression;

namespace Aiba.Services
{
    public class BrotliDecompressService : IDecompressService
    {
        public string[] SupportFormat { get; } = ["br"];

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

            Stream decompressedStream = new BrotliStream(stream, CompressionMode.Decompress);

            return Task.FromResult(decompressedStream);
        }
    }
}