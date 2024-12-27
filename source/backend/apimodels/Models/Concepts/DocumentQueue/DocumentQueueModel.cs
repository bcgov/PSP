using System;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Document
{
    /// <summary>
    /// DocumentQueueModel class, provides a model to represent document associated to entities.
    /// </summary>
    public class DocumentQueueModel : BaseAuditModel
    {

        #region Properties

        /// <summary>
        /// get/set - Document Queue Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The document id within PIMS.
        /// </summary>
        public long? DocumentId { get; set; }

        /// <summary>
        /// get/set - The original identifier in the source system.
        /// </summary>
        public long? DocumentExternalId { get; set; }

        /// <summary>
        /// get/set - The document queue status type.
        /// </summary>
        public CodeTypeModel<string> DocumentQueueStatusType { get; set; }

        /// <summary>
        /// get/set - The document source type.
        /// </summary>
        public CodeTypeModel<string> DataSourceTypeCode { get; set; }

        /// <summary>
        /// get/set - When the processing of the document began.
        /// </summary>
        public DateTime? DocumentProcessStartTimestamp { get; set; }

        /// <summary>
        /// get/set - When the processing of the document ended.
        /// </summary>
        public DateTime? DocumentProcessEndTimestamp { get; set; }

        /// <summary>
        /// get/set - The number of times an attempt was made to process this document.
        /// </summary>
        public int? DocumentProcessRetries { get; set; }

        /// <summary>
        /// get/set - The latest error from mayan, if processing failed.
        /// </summary>
        public string MayanError { get; set; }

        /// <summary>
        /// get/set - The related pims document.
        /// </summary>
        public DocumentModel PimsDocument { get; set; }

        /// <summary>
        /// get/set - The actual document content, as a byte array.
        /// </summary>
        public byte[] Document { get; set; }

        /// <summary>
        /// get/set - The queue status type.
        /// </summary>
        public CodeTypeModel<string> DocumentQueueStatusTypeCode { get; set; }
    #endregion
    }
}
