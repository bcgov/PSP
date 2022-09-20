using System;

namespace Pims.Api.Models.Config
{
    public class CdogsConfig
    {
        public Uri AuthHost { get; set; }

        public Uri CDogsHost { get; set; }

        public string ServiceClientId { get; set; }

        public string ServiceClientSecret { get; set; }
    }
}
