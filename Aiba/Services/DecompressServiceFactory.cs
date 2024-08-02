namespace Aiba.Services
{
    public class DecompressServiceFactory
    {
        public DecompressServiceFactory(IEnumerable<IDecompressService> decompressServices)
        {
            foreach (IDecompressService decompressService in decompressServices)
            {
                if (decompressService.SupportFormat.Length == 0)
                {
                    _defaultDecompressService = decompressService;
                }

                foreach (string format in decompressService.SupportFormat)
                {
                    _decompressServiceMap.TryAdd(format, decompressService);
                }
            }
        }

        private readonly Dictionary<string, IDecompressService> _decompressServiceMap = [];

        private readonly IDecompressService _defaultDecompressService = new DefaultDecompressService();

        public IDecompressService GetDecompressService(string format)
        {
            IDecompressService? decompressService = _decompressServiceMap.GetValueOrDefault(format);
            return decompressService ?? _defaultDecompressService;
        }
    }
}