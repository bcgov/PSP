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
        public string DocumentQueueStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The date/time that processing of the document started.
        /// </summary>
        public DateTime? DocProcessStartDate { get; set; }

        /// <summary>
        /// get/set - The date/time that processing of the document ended.
        /// </summary>
        public DateTime? DocProcessEndDate { get; set; }

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
