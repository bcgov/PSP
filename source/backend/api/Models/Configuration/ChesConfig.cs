using System;

namespace Pims.Api.Models.Config
{
    public class ChesConfig
    {
        public Uri AuthEndpoint { get; set; }

        public Uri ChesHost { get; set; }

        public string ServiceClientId { get; set; }

        public string ServiceClientSecret { get; set; }

        public string FromEmail { get; set; }
    }
}