using System;

namespace Pims.Api.Models.Config
{
    public class MayanConfig
    {
        public Uri BaseUri { get; set; }

        public string ConnectionUser { get; set; }

        public string ConnectionPassword { get; set; }

        public int UploadRetries { get; set; }

        public bool ExposeErrors { get; set; }

        public int ImageRetries { get; set; }

        public int PreviewPages { get; set; }

        public bool CacheDocumentTypes { get; set; }

    }
}
