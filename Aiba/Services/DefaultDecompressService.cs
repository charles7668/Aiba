namespace Aiba.Services
{
    public class DefaultDecompressService : IDecompressService
    {
        public string[] SupportFormat { get; } = [];

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

            return Task.FromResult(stream);
        }
    }
}