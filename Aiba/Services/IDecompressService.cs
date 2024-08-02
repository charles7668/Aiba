namespace Aiba.Services
{
    public interface IDecompressService
    {
        public string[] SupportFormat { get; }
        public Task<Stream> DecompressAsync(Stream stream, CancellationToken cancellationToken);
    }
}