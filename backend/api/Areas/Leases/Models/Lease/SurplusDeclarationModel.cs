using System;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    /// <summary>
    /// Provides a lease-property-oriented surplus declaration model.
    /// </summary>
    public class SurplusDeclarationModel
    {
        #region SurplusDeclaration

        /// <summary>
        /// get/set - The surplus declaration date.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// get/set - Surplus declaration type description.
        /// </summary>
        public string TypeDescription { get; set; }

        /// <summary>
        /// get/set - Surplus declaration type code.
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// get/set - Surplus declaration comment.
        /// </summary>
        public string Comment { get; set; }
        #endregion
    }
}
