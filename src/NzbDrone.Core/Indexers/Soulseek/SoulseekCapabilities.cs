using System.Collections.Generic;

namespace NzbDrone.Core.Indexers.Soulseek
{
    public class SoulseekCapabilities
    {
        public int DefaultPageSize { get; set;  }
        public int MaxPageSize { get; set;  }
        public string[] SupportedSearchParameters {  get; set; }

        public SoulseekCapabilities()
        {
            DefaultPageSize = 100;
            MaxPageSize = 100;
            SupportedSearchParameters = new[] { "q" };
        }
    }
}
