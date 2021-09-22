using System;
using NLog;
using NzbDrone.Common.Cache;
using NzbDrone.Common.Http;
using NzbDrone.Common.Serializer;

namespace NzbDrone.Core.Indexers.Soulseek
{
    public interface ISoulseekCapabilitiesProvider
    {
        SoulseekCapabilities GetCapabilities(SoulseekSettings settings);
    }

    public class SoulseekCapabilitiesProvider : ISoulseekCapabilitiesProvider
    {
        private readonly ICached<SoulseekCapabilities> _capabilitiesCache;
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;

        public SoulseekCapabilitiesProvider(ICacheManager cacheManager, IHttpClient httpClient, Logger logger)
        {
            _capabilitiesCache = cacheManager.GetCache<SoulseekCapabilities>(GetType());
            _httpClient = httpClient;
            _logger = logger;
        }

        public SoulseekCapabilities GetCapabilities(SoulseekSettings indexerSettings)
        {
            var key = indexerSettings.ToJson();
            var capabilities = _capabilitiesCache.Get(key, () => FetchCapabilities(indexerSettings), TimeSpan.FromDays(7));

            return capabilities;
        }

        private SoulseekCapabilities FetchCapabilities(SoulseekSettings indexerSettings)
        {
            throw new NotImplementedException();
        }
    }
}
