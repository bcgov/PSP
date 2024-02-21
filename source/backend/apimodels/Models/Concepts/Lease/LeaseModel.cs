using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.File;
using Pims.Api.Models.Concepts.Project;

namespace Pims.Api.Models.Concepts.Lease
{
    /// <summary>
    /// Provides a lease-oriented model.
    /// </summary>
    public class LeaseModel : FileModel
    {
        #region Properties

        /// <summary>
        /// get/set - The lease amount.
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// get/set - The value of the moti resource assigned to this lease.
        /// </summary>
        public string MotiName { get; set; }

        /// <summary>
        /// get/set - The value of the program name.
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// get/set - The location of documents related to this lease.
        /// </summary>
        public string DocumentationReference { get; set; }

        /// <summary>
        /// get/set - The lease notes.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// get/set - The lease description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The LIS L File #.
        /// </summary>
        public string LFileNo { get; set; }

        /// <summary>
        /// get/set - The LIS TFA File #.
        /// </summary>
        public string TfaFileNumber { get; set; }

        /// <summary>
        /// get/set - The LIS Ps File #.
        /// </summary>
        public string PsFileNo { get; set; }

        /// <summary>
        /// get/set - The text description if the lease category type is set to "other".
        /// </summary>
        public string OtherCategoryType { get; set; }

        /// <summary>
        /// get/set - The text description if the lease program type is set to "other".
        /// </summary>
        public string OtherProgramType { get; set; }

        /// <summary>
        /// get/set - The text description if the lease purpose type is set to "other".
        /// </summary>
        public string OtherPurposeType { get; set; }

        /// <summary>
        /// get/set - The text description if the lease type is set to "other".
        /// </summary>
        public string OtherType { get; set; }

        /// <summary>
        /// get/set - The calculated expiry date of the lease.
        /// </summary>
        public DateOnly? ExpiryDate { get; set; }

        /// <summary>
        /// get/set - The original start date of the lease.
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// get/set - The lease renewal count.
        /// </summary>
        public int RenewalCount { get; set; }

        /// <summary>
        /// get/set - The receivable payment type code identifier.
        /// </summary>
        public CodeTypeModel<string> PaymentReceivableType { get; set; }

        /// <summary>
        /// get/set - The entity that initiated this lease.
        /// </summary>
        public CodeTypeModel<string> Type { get; set; }

        /// <summary>
        /// get/set - The entity that initiated this lease.
        /// </summary>
        public CodeTypeModel<string> InitiatorType { get; set; }

        /// <summary>
        /// get/set - The entity responsible for this lease.
        /// </summary>
        public CodeTypeModel<string> ResponsibilityType { get; set; }

        /// <summary>
        /// get/set - The entity responsible for this lease.
        /// </summary>
        public CodeTypeModel<string> CategoryType { get; set; }

        /// <summary>
        /// get/set - The entity responsible for this lease.
        /// </summary>
        public CodeTypeModel<string> PurposeType { get; set; }

        /// <summary>
        /// get/set - The region of this lease within PIMS.
        /// </summary>
        public CodeTypeModel<short> Region { get; set; }

        /// <summary>
        /// get/set - The status of this lease within PIMS, Draft by default.
        /// </summary>
        public CodeTypeModel<string> ProgramType { get; set; }

        /// <summary>
        /// get/set - Notes accompanying Lease.
        /// </summary>
        public string ReturnNotes { get; set; }

        /// <summary>
        /// get/set - The date this entity assumed responsibility for this lease.
        /// </summary>
        public DateOnly? ResponsibilityEffectiveDate { get; set; }

        /// <summary>
        /// get/set - A list of properties associated with this lease.
        /// </summary>
        public new IList<PropertyLeaseModel> FileProperties { get; set; }

        /// <summary>
        /// get/set - A collection the consultations for this lease.
        /// </summary>
        public IEnumerable<ConsultationLeaseModel> Consultations { get; set; }

        /// <summary>
        /// get/set - A collection of the tenants for this lease.
        /// </summary>
        public IEnumerable<LeaseTenantModel> Tenants { get; set; }

         /// <summary>
        /// get/set - A collection of the terms for this lease.
        /// </summary>
        public IEnumerable<LeaseTermModel> Terms { get; set; }

        /// <summary>
        /// get/set - Whether this improvement contains a building that is subject to RTA (Residential Tenancy Act).
        /// </summary>
        public bool IsResidential { get; set; }

        /// <summary>
        /// get/set - Whether this improvement contains a commercial building.
        /// </summary>
        public bool IsCommercialBuilding { get; set; }

        /// <summary>
        /// get/set - Whether this improvement is of type other.
        /// </summary>
        public bool IsOtherImprovement { get; set; }

        public bool HasPhysicalFile { get; set; }

        public bool HasDigitalFile { get; set; }

        public bool? HasPhysicalLicense { get; set; }

        public bool? HasDigitalLicense { get; set; }

        public bool IsExpired { get; set; }

        /// <summary>
        /// get/set - The project associated with this lease.
        /// </summary>
        public ProjectModel Project { get; set; }
        #endregion
    }
}
