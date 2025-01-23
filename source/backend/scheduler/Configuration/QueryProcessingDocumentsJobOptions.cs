namespace Pims.Scheduler.Http.Configuration
{
    /// <summary>
    /// QueryProcessingDocumentsJobOptions class, provides a way to store job configuration.
    /// </summary>
    public class QueryProcessingDocumentsJobOptions
    {
        #region Properties

        /// <summary>
        /// get/set - the number of queued documents to pull in a single operation - affects the number of documents that will be uploaded in a single job run.
        /// </summary>
        public int? BatchSize { get; set; }

        /// <summary>
        /// get/set - the maximum number of minutes a document can be processing for before the upload is considered to be a failure.
        /// </summary>
        public int MaxProcessingMinutes { get; set; }
        #endregion
    }
}
