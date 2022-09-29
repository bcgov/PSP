using System;
using Pims.Api.Models;

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
        /// get/set - Surplus declaration type.
        /// </summary>
        public TypeModel<string> Type { get; set; }

        /// <summary>
        /// get/set - Surplus declaration comment.
        /// </summary>
        public string Comment { get; set; }
        #endregion
    }
}
