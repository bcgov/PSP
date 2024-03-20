using System.Collections.Generic;

namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// PropertyFilterCriteria class, provides a model for advanced filtering property.
    /// </summary>
    public class PropertyFilterCriteria
    {
        #region Properties

        /// <summary>
        /// get/set - The project id to filter by.
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// get/set - The tenure statuses to filter by.
        /// </summary>
        public List<string> TenureStatuses { get; set; }

        /// <summary>
        /// get/set - The tenure provincial public highway code to filter by.
        /// </summary>
        public string TenurePPH { get; set; }

        /// <summary>
        /// get/set - The tenure road types to filter by.
        /// </summary>
        public List<string> TenureRoadTypes { get; set; }

        /// <summary>
        /// get/set - The lease status to filter by.
        /// </summary>
        public string LeaseStatus { get; set; }

        /// <summary>
        /// get/set - The lease receivable/payable type to filter by.
        /// </summary>
        public string LeasePayRcvblType { get; set; }

        /// <summary>
        /// get/set - The multiple lease types to filter by.
        /// </summary>
        public List<string> LeaseTypes { get; set; }

        /// <summary>
        /// get/set - The multiple lease purposes to filter by.
        /// </summary>
        public List<string> LeasePurposes { get; set; }

        /// <summary>
        /// get/set - The ids of the anomalies to filter by.
        /// </summary>
        public List<string> AnomalyIds { get; set; }

        /// <summary>
        /// get/set - Whether or not to show core inventory properties.
        /// </summary>
        public bool IsCoreInventory { get; set; } = true;

        /// <summary>
        /// get/set - Whether or not to show properties of interest.
        /// </summary>
        public bool IsPropertyOfInterest { get; set; } = true;

        /// <summary>
        /// get/set - Whether or not to show other interest properties.
        /// </summary>
        public bool IsOtherInterest { get; set; } = true;

        /// <summary>
        /// get/set - Whether or not to show disposed properties.
        /// </summary>
        public bool IsDisposed { get; set; } = false;

        /// <summary>
        /// get/set - Whether or not to show retired properties.
        /// </summary>
        public bool IsRetired { get; set; } = false;

        #endregion
    }
}
