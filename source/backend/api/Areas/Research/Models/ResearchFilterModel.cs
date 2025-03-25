using System;
using System.Collections.Generic;
using Pims.Core.Extensions;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Areas.Research.Models.Search
{
    public class ResearchFilterModel : PageFilter
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
        /// get/set - The moti region that any of the properties on the research file belong to.
        /// </summary>
        public short? RegionCode { get; set; }

        /// <summary>
        /// get/set - The status of the research file,.
        /// </summary>
        public string ResearchFileStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The descriptive name of the address.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The road name or the alias, search for both simultaneously.
        /// </summary>
        public string RoadOrAlias { get; set; }

        /// <summary>
        /// get/set - The generated research file number.
        /// </summary>
        public string RFileNumber { get; set; }

        /// <summary>
        /// get/set - The username/idir of the user that created this row.
        /// </summary>
        public string AppCreateUserid { get; set; }

        /// <summary>
        /// get/set - Search for any research row creation date after this date.
        /// </summary>
        public DateTime? CreatedOnStartDate { get; set; }

        /// <summary>
        /// get/set - Search for any research row creation date before this date.
        /// </summary>
        public DateTime? CreatedOnEndDate { get; set; }

        /// <summary>
        /// get/set - The idir or username of the user that updated this research file row.
        /// </summary>
        public string AppLastUpdateUserid { get; set; }

        /// <summary>
        /// get/set - Search for any research row update date after this date.
        /// </summary>
        public DateTime? UpdatedOnStartDate { get; set; }

        /// <summary>
        /// get/set - Search for any research row update date before this date.
        /// </summary>
        public DateTime? UpdatedOnEndDate { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ResearchFilterModel class.
        /// </summary>
        public ResearchFilterModel()
        {
        }

        /// <summary>
        /// Creates a new instance of a ResearchFilterModel class, initializes with the specified arguments.
        /// </summary>
        /// <param name="query"></param>
        public ResearchFilterModel(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
            : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            Pid = filter.GetStringValue(nameof(Pid));
            Pin = filter.GetStringValue(nameof(Pin));
            RegionCode = filter.GetShortNullValue(nameof(RegionCode));
            ResearchFileStatusTypeCode = filter.GetStringValue(nameof(ResearchFileStatusTypeCode));
            Name = filter.GetStringValue(nameof(Name));
            RoadOrAlias = filter.GetStringValue(nameof(RoadOrAlias));
            RFileNumber = filter.GetStringValue(nameof(RFileNumber));
            AppCreateUserid = filter.GetStringValue(nameof( AppCreateUserid));
            CreatedOnStartDate = filter.GetDateTimeNullValue(nameof(CreatedOnStartDate));
            CreatedOnEndDate = filter.GetDateTimeNullValue(nameof(CreatedOnEndDate));
            AppLastUpdateUserid = filter.GetStringValue(nameof(AppLastUpdateUserid));
            UpdatedOnStartDate = filter.GetDateTimeNullValue(nameof(UpdatedOnStartDate));
            UpdatedOnEndDate = filter.GetDateTimeNullValue(nameof(UpdatedOnEndDate));
            Sort = filter.GetStringArrayValue(nameof(Sort));
        }
        #endregion

        #region Methods

        /// <summary>
        /// Convert to a ResearchFilterModel.
        /// </summary>
        /// <param name="model"></param>
        public static explicit operator ResearchFilter(ResearchFilterModel model)
        {
            var filter = new ResearchFilter
            {
                Pid = model.Pid,
                Pin = model.Pin,
                Page = model.Page,
                Quantity = model.Quantity,
                RegionCode = model.RegionCode,
                RFileNumber = model.RFileNumber,
                ResearchFileStatusTypeCode = model.ResearchFileStatusTypeCode,
                Name = model.Name,
                RoadOrAlias = model.RoadOrAlias,
                AppCreateUserid = model.AppCreateUserid,
                CreatedOnStartDate = model.CreatedOnStartDate,
                CreatedOnEndDate = model.CreatedOnEndDate,
                AppLastUpdateUserid = model.AppLastUpdateUserid,
                UpdatedOnStartDate = model.UpdatedOnStartDate,
                UpdatedOnEndDate = model.UpdatedOnEndDate,
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
            if ((this.CreatedOnStartDate.HasValue && (this.CreatedOnEndDate.HasValue && this.CreatedOnStartDate > this.CreatedOnEndDate)) ||
                (this.UpdatedOnStartDate.HasValue && (this.UpdatedOnEndDate.HasValue && this.UpdatedOnStartDate > this.UpdatedOnEndDate)))
            {
                return false;
            }

            return base.IsValid();
        }
        #endregion
    }
}
