using System;

namespace Pims.Api.Models.Config
{
    public class MayanConfig
    {
        public Uri BaseUri { get; set; }

        public string ConnectionUser { get; set; }

        public string ConnectionPassword { get; set; }
    }
}
