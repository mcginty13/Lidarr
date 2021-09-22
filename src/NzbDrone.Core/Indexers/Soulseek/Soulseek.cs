using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NzbDrone.Common.Http;
using NzbDrone.Core.Configuration;
using NzbDrone.Core.Parser;
using NzbDrone.Core.ThingiProvider;

namespace NzbDrone.Core.Indexers.Soulseek
{
    public class Soulseek : HttpIndexerBase<SoulseekSettings>
    {
        private readonly ISoulseekCapabilitiesProvider _capabilitiesProvider;

        public override string Name => "Soulseek - slskd API";

        public override DownloadProtocol Protocol => DownloadProtocol.Soulseek;
        public override int PageSize => _capabilitiesProvider.GetCapabilities(Settings).DefaultPageSize;
        public override IIndexerRequestGenerator GetRequestGenerator()
        {
            return new SoulseekRequestGenerator
            {
                HttpClient = _httpClient,
                Logger = _logger,
                Settings = GetSettings()
            };
        }

        private SoulseekSettings GetSettings()
        {
            var settings = new SoulseekSettings
            {
                BaseUrl = "http://192.168.1.10:5000",
                ApiPath = "/api/v0",
                Username = "username",
                Password = "password"
            };

            return settings;
        }

        public override IParseIndexerResponse GetParser()
        {
            return new SoulseekParser(Settings);

            //throw new NotImplementedException();
        }

        public Soulseek(ISoulseekCapabilitiesProvider capabilitiesProvider, IHttpClient httpClient, IIndexerStatusService indexerStatusService, IConfigService configService, IParsingService parsingService, Logger logger)
           : base(httpClient, indexerStatusService, configService, parsingService, logger)
        {
            _capabilitiesProvider = capabilitiesProvider;
        }

        public override IEnumerable<ProviderDefinition> DefaultDefinitions
        {
            get
            {
                yield return GetDefinition("Soulseek", GetSettings());

                //yield return GetDefinition("Not What CD", GetSettings("https://notwhat.cd"));
            }
        }

        private IndexerDefinition GetDefinition(string name, SoulseekSettings settings)
        {
            return new IndexerDefinition
            {
                EnableRss = false,
                EnableAutomaticSearch = false,
                EnableInteractiveSearch = false,
                Name = name,
                Implementation = GetType().Name,
                Settings = settings,
                Protocol = DownloadProtocol.Soulseek,
                SupportsRss = SupportsRss,
                SupportsSearch = SupportsSearch
            };
        }
    }
}
