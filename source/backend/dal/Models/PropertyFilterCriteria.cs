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
        /// get/set - The lease status to filter by.
        /// </summary>
        public string LeaseStatus { get; set; }

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

        #endregion
    }
}
