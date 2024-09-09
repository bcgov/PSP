
using System;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Lease
{
    /// <summary>
    /// Provides an Api Lease Renewal model.
    /// </summary>
    public class LeaseRenewalModel : BaseAuditModel
    {
        #region Properties

        public long Id { get; set; }

        public long LeaseId { get; set; }

        public DateTime? CommencementDt { get; set; }

        public DateTime? ExpiryDt { get; set; }

        public bool? IsExercised { get; set; }

        public string RenewalNote { get; set; }

        public LeaseModel Lease { get; set; }
        #endregion
    }
}
