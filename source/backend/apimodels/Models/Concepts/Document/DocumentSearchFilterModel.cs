using Pims.Dal.Entities.Models;

namespace Pims.Api.Models.Concepts.Document
{
    public class DocumentSearchFilterModel : PageFilter
    {
        public DocumentSearchFilterModel()
        {
        }

        /// <summary>
        /// get/set - The Document type.
        /// </summary>
        public string DocumentTypTypeCode { get; set; }

        /// <summary>
        /// get/set - The status of the Document.
        /// </summary>
        public string DocumentStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The name of the Document.
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// get/set - The pid identifier to search by.
        /// </summary>
        public string Pid { get; set; }

        /// <summary>
        /// get/set - The pin identifier to search by.
        /// </summary>
        public string Pin { get; set; }

        /// <summary>
        /// get/set - The plan number identifier to search by.
        /// </summary>
        public string Plan { get; set; }

        /// <summary>
        /// get/set - The string content to search by.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// get/set - The Mayan document identifiers that match an external content search.
        /// </summary>
        public long[] MayanDocumentIds { get; set; }

        /// <summary>
        /// Determine if a valid filter was provided.
        /// </summary>
        /// <returns>true if the filter is valid, false otherwise.</returns>
        public override bool IsValid()
        {
            return base.IsValid();
        }
    }
}
