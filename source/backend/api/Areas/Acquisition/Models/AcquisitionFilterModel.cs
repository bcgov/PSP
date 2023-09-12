using System;
using System.Collections.Generic;
using Pims.Core.Extensions;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Areas.Acquisition.Models.Search
{
    public class AcquisitionFilterModel : PageFilter
    {
        #region Properties

        /// <summary>
        /// get/set - The pid identifier to search by.
        /// </summary>
        public string Pid { get; set; }

        /// <summary>
        /// get/set - The pin identifier to search by.
        /// </summary>
        public string Pin { get; set; }

        /// <summary>
        /// get/set - The address to search by.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// get/set - The status of the acquisition file.
        /// </summary>
        public string AcquisitionFileStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The acquisition file name or the file number, search for both simultaneously.
        /// </summary>
        public string AcquisitionFileNameOrNumber { get; set; }

        /// <summary>
        /// get/set - The MOTI project name or the project number, search for both simultaneously.
        /// </summary>
        public string ProjectNameOrNumber { get; set; }

        /// <summary>
        /// get/set - The MOTI person id to search by for acquisition team members.
        /// </summary>
        public string AcquisitionTeamMemberPersonId { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionFilterModel class.
        /// </summary>
        public AcquisitionFilterModel()
        {
        }

        /// <summary>
        /// Creates a new instance of a AcquisitionFilterModel class, initializes with the specified arguments.
        /// </summary>
        /// <param name="query"></param>
        public AcquisitionFilterModel(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
            : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            this.Pid = filter.GetStringValue(nameof(this.Pid));
            this.Pin = filter.GetStringValue(nameof(this.Pin));
            this.Address = filter.GetStringValue(nameof(this.Address));
            this.AcquisitionFileStatusTypeCode = filter.GetStringValue(nameof(this.AcquisitionFileStatusTypeCode));
            this.AcquisitionFileNameOrNumber = filter.GetStringValue(nameof(this.AcquisitionFileNameOrNumber));
            this.ProjectNameOrNumber = filter.GetStringValue(nameof(this.ProjectNameOrNumber));
            this.AcquisitionTeamMemberPersonId = filter.GetStringValue(nameof(this.AcquisitionTeamMemberPersonId));

            this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
        }
        #endregion

        #region Methods

        /// <summary>
        /// Convert to a AcquisitionFilter.
        /// </summary>
        /// <param name="model"></param>
        public static explicit operator AcquisitionFilter(AcquisitionFilterModel model)
        {
            var filter = new AcquisitionFilter
            {
                Page = model.Page,
                Quantity = model.Quantity,

                Pid = model.Pid,
                Pin = model.Pin,
                Address = model.Address,
                AcquisitionFileStatusTypeCode = model.AcquisitionFileStatusTypeCode,
                AcquisitionFileNameOrNumber = model.AcquisitionFileNameOrNumber,
                ProjectNameOrNumber = model.ProjectNameOrNumber,
                AcquisitionTeamMemberPersonId = model.AcquisitionTeamMemberPersonId,

                Sort = model.Sort,
            };

            return filter;
        }

        /// <summary>
        /// Determine if a valid filter was provided.
        /// </summary>
        /// <returns>true if the filter is valid, false otherwise.</returns>
        public override bool IsValid()
        {
            return base.IsValid();
        }
        #endregion
    }
}
