using System;
using System.Collections.Generic;
using Pims.Core.Extensions;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Areas.Disposition.Models.Search
{
    public class DispositionFilterModel : PageFilter
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
        /// get/set - The disposition file name or the file number or the legacy reference number, search for all simultaneously.
        /// </summary>
        public string FileNameOrNumberOrReference { get; set; }

        /// <summary>
        /// get/set - The status of the disposition file.
        /// </summary>
        public string DispositionFileStatusCode { get; set; }

        /// <summary>
        /// get/set - The disposition status.
        /// </summary>
        public string DispositionStatusCode { get; set; }

        /// <summary>
        /// get/set - The type of the disposition.
        /// </summary>
        public string DispositionTypeCode { get; set; }

        /// <summary>
        /// get/set - The MOTI person id to search by for disposition team members.
        /// </summary>
        public long? TeamMemberPersonId { get; set; }

        /// <summary>
        /// get/set - The MOTI Organization id to search by for disposition team members.
        /// </summary>
        public long? TeamMemberOrganizationId { get; set; }

        /// <summary>
        /// get/set - The region types.
        /// </summary>
        public IList<int> Regions { get; set; } = new List<int>();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a DispositionFilterModel class.
        /// </summary>
        public DispositionFilterModel()
        {
        }

        /// <summary>
        /// Creates a new instance of a DispositionFilterModel class, initializes with the specified arguments.
        /// </summary>
        /// <param name="query"></param>
        public DispositionFilterModel(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
            : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            Pid = filter.GetStringValue(nameof(this.Pid));
            Pin = filter.GetStringValue(nameof(this.Pin));
            Address = filter.GetStringValue(nameof(this.Address));
            FileNameOrNumberOrReference = filter.GetStringValue(nameof(this.FileNameOrNumberOrReference));
            DispositionFileStatusCode = filter.GetStringValue(nameof(this.DispositionFileStatusCode));
            DispositionStatusCode = filter.GetStringValue(nameof(this.DispositionStatusCode));
            DispositionTypeCode = filter.GetStringValue(nameof(this.DispositionTypeCode));
            TeamMemberPersonId = filter.GetLongNullValue(nameof(this.TeamMemberPersonId));
            TeamMemberOrganizationId = filter.GetLongNullValue(nameof(this.TeamMemberOrganizationId));
            Regions = filter.GetIntArrayValue(nameof(Regions));

            this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
        }
        #endregion

        #region Methods

        /// <summary>
        /// Convert to a DispositionFilter.
        /// </summary>
        /// <param name="model"></param>
        public static explicit operator DispositionFilter(DispositionFilterModel model)
        {
            var filter = new DispositionFilter
            {
                Page = model.Page,
                Quantity = model.Quantity,

                Pid = model.Pid?.Trim(),
                Pin = model.Pin?.Trim(),
                Address = model.Address?.Trim(),
                FileNameOrNumberOrReference = model.FileNameOrNumberOrReference?.Trim(),
                DispositionFileStatusCode = model.DispositionFileStatusCode,
                DispositionStatusCode = model.DispositionStatusCode,
                DispositionTypeCode = model.DispositionTypeCode,
                TeamMemberPersonId = model.TeamMemberPersonId,
                TeamMemberOrganizationId = model.TeamMemberOrganizationId,
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
