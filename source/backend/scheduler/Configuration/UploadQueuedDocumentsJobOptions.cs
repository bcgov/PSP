namespace Pims.Scheduler.Http.Configuration
{
    /// <summary>
    /// UploadQueuedDocumentsJobOptions class, provides a way to store job configuration.
    /// </summary>
    public class UploadQueuedDocumentsJobOptions
    {
        #region Properties

        /// <summary>
        /// get/set - the number of queued documents to pull in a single operation - affects the number of documents that will be uploaded in a single job run.
        /// </summary>
        public int? BatchSize { get; set; }

        /// <summary>
        /// get/set - the file size, in mb, that will be processed in a single job run.
        /// </summary>
        public int? FileSize { get; set; }
        #endregion
    }
}
