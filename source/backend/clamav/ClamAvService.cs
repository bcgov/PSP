using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using nClam;
using Pims.Av.Configuration;
using Pims.Core.Exceptions;

namespace Pims.Av
{
    /// <summary>
    /// ClamAvService class, provides a service for integration with a running ClamAv Service and performing scans.
    /// </summary>
    public class ClamAvService : IAvService
    {
        private readonly ILogger<IAvService> _logger;

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ClamAvService, initializes with specified arguments.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public ClamAvService(IOptions<ClamAvOptions> options, IClamClient client, ILogger<IAvService> logger)
        {
            this.Options = options.Value;
            this.Client = client;
            this.Client.Port = this.Options.Port;
            this.Client.Server = this.Options.HostUri;
            this._logger = logger;
        }
        #endregion

        #region Properties
        public ClamAvOptions Options { get; }

        protected IClamClient Client { get; }
        #endregion

        #region Methods

        /// <summary>
        /// Scan the passed file, throw exceptions if the scan fails, or returns a positive result.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task ScanAsync(IFormFile file)
        {
            if (this.Options.DisableScan)
            {
                this._logger.LogInformation("ClamAV scan disabled");
                return;
            }
            using var ms = new MemoryStream();
            file.OpenReadStream().CopyTo(ms);
            byte[] fileBytes = ms.ToArray();

            await ScanAsync(fileBytes);
        }

        /// <summary>
        /// Scan the passed file, throw exceptions if the scan fails, or returns a positive result.
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public async Task ScanAsync(byte[] fileData)
        {
            if (this.Options.DisableScan)
            {
                this._logger.LogInformation("ClamAV scan disabled");
                return;
            }
            this._logger.LogInformation("ClamAV scan start");
            var scanResult = await this.Client.SendAndScanFileAsync(fileData);

            switch (scanResult.Result)
            {
                case ClamScanResults.Unknown:
                    this._logger.LogError("ClamAV scan failed", scanResult);
                    throw new AvException("Virus scan failed with unknown response. If you continue to see this error, please contact your site administrator.");
                case ClamScanResults.Clean:
                    this._logger.LogDebug("ClamAV scan was clean", scanResult);
                    break;
                case ClamScanResults.VirusDetected:
                    this._logger.LogWarning("ClamAV scan possible threat", scanResult);
                    throw new AvException($"Virus scan failed, one or more threats found. If you are confident this file is safe, please contact your site administrator.");
                case ClamScanResults.Error:
                    this._logger.LogError("ClamAV scan returned an error", scanResult);
                    throw new AvException($"Virus scan failed with error: {scanResult.RawResult}. If you continue to see this error, please contact your site administrator.");
            }
            this._logger.LogInformation("ClamAV scan end");
        }
        #endregion
    }
}
