using System.Collections.Generic;
using NzbDrone.Common.Serializer;
using NzbDrone.Core.Parser.Model;

namespace NzbDrone.Core.Indexers.Soulseek
{
    internal class SoulseekParser : IParseIndexerResponse
    {
        private readonly SoulseekSettings _settings;

        public SoulseekParser(SoulseekSettings settings)
        {
            _settings = settings;
        }

        public IList<ReleaseInfo> ParseResponse(IndexerResponse indexerResponse)
        {
            List<ReleaseInfo> releases = new List<ReleaseInfo>();
            var soulseekSearchResponses = Json.Deserialize<List<SoulseekIndexerResponse>>(indexerResponse.Content);
            soulseekSearchResponses.ForEach((response) =>
            {
                releases.Add(new ReleaseInfo
                {
                    DownloadProtocol = DownloadProtocol.Soulseek,
                    
                })
            });
            return releases;
        }
    }

    internal class SoulseekIndexerResponse
    {
        public int fileCount { get; set; }
        public int freeUploadSlots { get; set; }
        public int lockedFileCount { get; set; }
        public int queueLength { get; set; }
        public long uploadSpeed {  get; set; }
        public string username { get; set;  }
        public SoulseekIndexerResponseFile[] files { get; set; }
    }

    internal class SoulseekIndexerResponseFile
    {
        public int bitRate {  get; set; }
        public int code { get; set; }
        public string extension { get; set; }
        public string filename { get; set; }
        public int length { get; set; }
        public long size { get; set; }
        public bool isLocked { get; set; }
    }
}
