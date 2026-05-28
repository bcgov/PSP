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
        /// get/set - The Property Owner's name to search.
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// get/set - The MOTI person id to search by for acquisition team members.
        /// </summary>
        public string AcquisitionTeamMemberPersonId { get; set; }

        /// <summary>
        /// get/set - The MOTI Organization id to search by for acquisition team members.
        /// </summary>
        public string AcquisitionTeamMemberOrganizationId { get; set; }

        /// <summary>
        /// get/set - Get the Acquisition files that has NOC.
        /// </summary>
        public bool HasNoticeOfClaim { get; set; }

        /// <summary>
        /// get/set - The region types.
        /// </summary>
        public IList<int> Regions { get; set; } = new List<int>();

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

            Pid = filter.GetStringValue(nameof(Pid));
            Pin = filter.GetStringValue(nameof(Pin));
            Address = filter.GetStringValue(nameof(Address));
            AcquisitionFileStatusTypeCode = filter.GetStringValue(nameof(AcquisitionFileStatusTypeCode));
            AcquisitionFileNameOrNumber = filter.GetStringValue(nameof(AcquisitionFileNameOrNumber));
            ProjectNameOrNumber = filter.GetStringValue(nameof(ProjectNameOrNumber));
            OwnerName = filter.GetStringValue(nameof(OwnerName));
            AcquisitionTeamMemberPersonId = filter.GetStringValue(nameof(AcquisitionTeamMemberPersonId));
            AcquisitionTeamMemberOrganizationId = filter.GetStringValue(nameof(AcquisitionTeamMemberOrganizationId));
            HasNoticeOfClaim = filter.GetBoolValue(nameof(HasNoticeOfClaim));
            Regions = filter.GetIntArrayValue(nameof(Regions));

            Sort = filter.GetStringArrayValue(nameof(Sort));
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

                Pid = model.Pid?.Trim(),
                Pin = model.Pin?.Trim(),
                Address = model.Address?.Trim(),
                AcquisitionFileStatusTypeCode = model.AcquisitionFileStatusTypeCode,
                AcquisitionFileNameOrNumber = model.AcquisitionFileNameOrNumber?.Trim(),
                ProjectNameOrNumber = model.ProjectNameOrNumber?.Trim(),
                OwnerName = model.OwnerName?.Trim(),
                AcquisitionTeamMemberPersonId = model.AcquisitionTeamMemberPersonId,
                AcquisitionTeamMemberOrganizationId = model.AcquisitionTeamMemberOrganizationId,
                HasNoticeOfClaim = model.HasNoticeOfClaim,
                Regions = model.Regions,

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
