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
        /// get/set - The physical file status of the disposition file.
        /// </summary>
        public string PhysicalFileStatusCode { get; set; }

        /// <summary>
        /// get/set - The disposition status of the disposition file.
        /// </summary>
        public string DispositionStatusCode { get; set; }

        /// <summary>
        /// get/set - The type of the disposition file.
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

            this.Pid = filter.GetStringValue(nameof(this.Pid));
            this.Pin = filter.GetStringValue(nameof(this.Pin));
            this.Address = filter.GetStringValue(nameof(this.Address));
            this.FileNameOrNumberOrReference = filter.GetStringValue(nameof(this.FileNameOrNumberOrReference));
            this.PhysicalFileStatusCode = filter.GetStringValue(nameof(this.PhysicalFileStatusCode));
            this.DispositionStatusCode = filter.GetStringValue(nameof(this.DispositionStatusCode));
            this.DispositionTypeCode = filter.GetStringValue(nameof(this.DispositionTypeCode));
            this.TeamMemberPersonId = filter.GetLongNullValue(nameof(this.TeamMemberPersonId));
            this.TeamMemberOrganizationId = filter.GetLongNullValue(nameof(this.TeamMemberOrganizationId));

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

                Pid = model.Pid,
                Pin = model.Pin,
                Address = model.Address,
                FileNameOrNumberOrReference = model.FileNameOrNumberOrReference,
                PhysicalFileStatusCode = model.PhysicalFileStatusCode,
                DispositionStatusCode = model.DispositionStatusCode,
                DispositionTypeCode = model.DispositionTypeCode,
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
