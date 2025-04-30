using System;
using System.Collections.Generic;
using Pims.Core.Extensions;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Areas.Management.Models.Search
{
    public class ManagementFilterModel : PageFilter
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
        /// get/set - The management file name or the file number or the legacy reference number, search for all simultaneously.
        /// </summary>
        public string FileNameOrNumberOrReference { get; set; }

        /// <summary>
        /// get/set - The status of the management file.
        /// </summary>
        public string ManagementFileStatusCode { get; set; }

        /// <summary>
        /// get/set - The status of the management file.
        /// </summary>
        public string ManagementFilePurposeCode { get; set; }

        /// <summary>
        /// get/set - The MOTI project name or the project number, search for both simultaneously.
        /// </summary>
        public string ProjectNameOrNumber { get; set; }

        /// <summary>
        /// get/set - The MOTI person id to search by for management team members.
        /// </summary>
        public long? TeamMemberPersonId { get; set; }

        /// <summary>
        /// get/set - The MOTI Organization id to search by for management team members.
        /// </summary>
        public long? TeamMemberOrganizationId { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ManagementFilterModel class.
        /// </summary>
        public ManagementFilterModel()
        {
        }

        /// <summary>
        /// Creates a new instance of a ManagementFilterModel class, initializes with the specified arguments.
        /// </summary>
        /// <param name="query"></param>
        public ManagementFilterModel(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
            : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            this.Pid = filter.GetStringValue(nameof(this.Pid));
            this.Pin = filter.GetStringValue(nameof(this.Pin));
            this.Address = filter.GetStringValue(nameof(this.Address));
            this.FileNameOrNumberOrReference = filter.GetStringValue(nameof(this.FileNameOrNumberOrReference));
            this.ManagementFileStatusCode = filter.GetStringValue(nameof(this.ManagementFileStatusCode));
            this.ProjectNameOrNumber = filter.GetStringValue(nameof(this.ProjectNameOrNumber));
            this.ManagementFilePurposeCode = filter.GetStringValue(nameof(this.ManagementFilePurposeCode));
            this.TeamMemberPersonId = filter.GetLongNullValue(nameof(this.TeamMemberPersonId));
            this.TeamMemberOrganizationId = filter.GetLongNullValue(nameof(this.TeamMemberOrganizationId));

            this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
        }
        #endregion

        #region Methods

        /// <summary>
        /// Convert to a ManagementFilter.
        /// </summary>
        /// <param name="model"></param>
        public static explicit operator ManagementFilter(ManagementFilterModel model)
        {
            var filter = new ManagementFilter
            {
                Page = model.Page,
                Quantity = model.Quantity,

                Pid = model.Pid,
                Pin = model.Pin,
                Address = model.Address,
                FileNameOrNumberOrReference = model.FileNameOrNumberOrReference,
                ManagementFileStatusCode = model.ManagementFileStatusCode,
                ProjectNameOrNumber = model.ProjectNameOrNumber,
                ManagementFilePurposeCode = model.ManagementFilePurposeCode,
                TeamMemberPersonId = model.TeamMemberPersonId,
                TeamMemberOrganizationId = model.TeamMemberOrganizationId,

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
