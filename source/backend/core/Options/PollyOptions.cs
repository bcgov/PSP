namespace Pims.Core.Configuration
{
    /// <summary>
    /// PollyOptions class, provides a way to configure polly resiliency pipelines.
    /// </summary>
    public class PollyOptions
    {
        #region Properties

        /// <summary>
        /// get/set - The number of times http requests will be retried.
        /// </summary>
        public int MaxRetries { get; set; } = 3;

        /// <summary>
        /// get/set - The number of seconds to delay before making a retry, using iterative backoff.
        /// </summary>
        public int DelayInSeconds { get; set; } = 1;
        #endregion
    }
}
