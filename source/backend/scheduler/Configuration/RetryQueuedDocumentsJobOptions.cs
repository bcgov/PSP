namespace Pims.Scheduler.Http.Configuration
{
    /// <summary>
    /// RetryQueuedDocumentsJobOptions class, provides a way to store job configuration.
    /// </summary>
    public class RetryQueuedDocumentsJobOptions
    {
        #region Properties

        /// <summary>
        /// get/set - the number of queued documents to pull in a single operation - affects the number of documents that will be uploaded in a single job run.
        /// </summary>
        public int? BatchSize { get; set; }

        /// <summary>
        /// get/set - the file size, in bytes, that will be processed in a single job run.
        /// </summary>
        public int? MaxFileSize { get; set; }
        #endregion
    }
}
