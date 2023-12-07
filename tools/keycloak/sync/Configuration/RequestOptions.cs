namespace Pims.Tools.Keycloak.Sync.Configuration
{
    /// <summary>
    /// RequestOptions class, provides a way to configure requests.
    /// </summary>
    public class RequestOptions
    {
        #region Properties

        /// <summary>
        /// get/set - Whether to retry a request after a failure.
        /// </summary>
        public bool RetryAfterFailure { get; set; } = true;

        /// <summary>
        /// get/set - How many retries after a failure should be sent.
        /// </summary>
        public int RetryAttempts { get; set; } = 3;

        /// <summary>
        /// get/set - After how many failures should the import be aborted.
        /// </summary>
        public int AbortAfterFailure { get; set; } = 1;
        #endregion
    }
}
