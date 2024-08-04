using Aiba.Plugin;

namespace Aiba.MediaInfoProviders
{
    public class MediaProviderFactory
    {
        public MediaProviderFactory(IEnumerable<IMediaInfoProvider> providers)
        {
            foreach (IMediaInfoProvider mediaInfoProvider in providers)
            {
                if (_providerMap.TryAdd(mediaInfoProvider.ProviderName, mediaInfoProvider))
                {
                    _providers.Add(mediaInfoProvider);
                }
            }
        }

        public IEnumerable<IMediaInfoProvider> Providers => _providers;

        private readonly Dictionary<string, IMediaInfoProvider> _providerMap = [];
        private readonly List<IMediaInfoProvider> _providers = [];

        public string[] GetProviderList()
        {
            return _providers.Select(x => x.ProviderName).ToArray();
        }

        public IMediaInfoProvider? GetProvider(string name)
        {
            return _providerMap.GetValueOrDefault(name);
        }
    }
}