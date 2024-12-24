using System;

namespace Pims.Dal.Entities.Models
{
    public class DocumentQueueFilter : PageFilter
    {
        #region Properties

        /// <summary>
        /// get/set - The source system for this document.
        /// </summary>
        public string DataSourceTypeCode { get; set; }

        /// <summary>
        /// get/set - The status of the document in the queue, such as 'Pending'.
        /// </summary>
        public string[] DocumentQueueStatusTypeCodes { get; set; }

        /// <summary>
        /// get/set - The date/time that processing of the document started.
        /// </summary>
        public DateTime? DocProcessStartDate { get; set; }

        /// <summary>
        /// get/set - The date/time that processing of the document ended.
        /// </summary>
        public DateTime? DocProcessEndDate { get; set; }

        /// <summary>
        /// get/set - The maximum number of times that the system has attempted to upload the document after the initial failure.
        /// </summary>
        public int? MaxDocProcessRetries { get; set; }

        /// <summary>
        /// get/set - The maximum file size to return from the filter.
        /// </summary>
        public int? MaxFileSize { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a DocumentQueueFilter class.
        /// </summary>
        public DocumentQueueFilter()
        {
        }

        #endregion
    }
}
