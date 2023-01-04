namespace Pims.Av.Configuration
{
    /// <summary>
    /// ClamAvOptions class, provides a way to configure Clam AV.
    /// </summary>
    public class ClamAvOptions
    {
        public const string DEFAULTURI = "localhost";
        #region Properties

        /// <summary>
        /// get/set - The URI to the ClamAV API service.
        /// </summary>
        public string HostUri { get; set; } = DEFAULTURI;

        /// <summary>
        /// get/set - the ClamAV port.
        /// </summary>
        public int Port { get; set; } = 3310;

        /// <summary>
        /// get/set - Whether or not the av scan should be skipped.
        /// </summary>
        public bool DisableScan { get; set; } = false;

        /// <summary>
        /// get/set - The max file size for scanning that should be supported by nclam.
        /// </summary>
        public int MaxFileSize { get; set; } = 524288000;
        #endregion
    }
}
