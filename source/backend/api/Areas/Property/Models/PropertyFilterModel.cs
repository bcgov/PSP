using System;
using System.Collections.Generic;
using System.Linq;
using Pims.Core.Extensions;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Areas.Property.Models.Search
{
    /// <summary>
    /// PropertyFilterModel class, provides a model to contain the parcel and building filters.
    /// </summary>
    public class PropertyFilterModel : PageFilter
    {
        #region Properties

        /// <summary>
        /// get/set - The property address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// get/set - The pin or pid property identifier.
        /// </summary>
        public string PinOrPid { get; set; }

        /// <summary>
        /// get/set - The property plan number.
        /// </summary>
        public string PlanNumber { get; set; }

        /// <summary>
        /// get/set - The property ownership status.
        /// </summary>
        public IList<string> Ownership { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyFilterModel class.
        /// </summary>
        public PropertyFilterModel()
        {
        }

        /// <summary>
        /// Creates a new instance of a PropertyFilterModel class, initializes with the specified arguments.
        /// </summary>
        /// <param name="query"></param>
        public PropertyFilterModel(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
            : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
            var tempSort = this.Sort.ToList();

            // Convert sort to db format
            for (int i = 0; i < this.Sort.Length; i++)
            {
                if (tempSort[i].StartsWith("Location"))
                {
                    tempSort[i] = tempSort[i].Replace("Location", "MunicipalityName");
                }
                if (tempSort[i].StartsWith("LotSizeInHa"))
                {
                    tempSort[i] = tempSort[i].Replace("LotSizeInHa", "LandArea");
                }
                if (tempSort[i].StartsWith("Ownership"))
                {
                    // The order will affect the display in the frontend. For now in alphabetical order.
                    // i.e. [Core Inventory, Disposed, Other Interest, Property of Interest]
                    var direction = this.Sort[i].Split(' ')[1];
                    tempSort[i] = this.Sort[i].Replace("Ownership", "IsOwned");
                    tempSort.Add($"IsDisposed {direction}");
                    tempSort.Add($"IsOtherInterest {direction}");
                    tempSort.Add($"HasActiveAcquisitionFile {direction}");
                    tempSort.Add($"HasActiveResearchFile {direction}");
                }
            }
            this.Sort = tempSort.ToArray();

            this.PinOrPid = filter.GetStringValue(nameof(this.PinOrPid));
            this.Address = filter.GetStringValue(nameof(this.Address));
            this.PlanNumber = filter.GetStringValue(nameof(this.PlanNumber));
            this.Ownership = filter.GetStringArrayValue(nameof(this.Ownership));
        }
        #endregion

        #region Methods

        /// <summary>
        /// Convert to a ParcelFilter.
        /// </summary>
        /// <param name="model"></param>
        public static explicit operator PropertyFilter(PropertyFilterModel model)
        {
            var filter = new PropertyFilter
            {
                Page = model.Page,
                Quantity = model.Quantity,
                Sort = model.Sort,

                PinOrPid = model.PinOrPid,
                Address = model.Address,
                PlanNumber = model.PlanNumber,
                Ownership = model.Ownership,
            };

            return filter;
        }

        /// <summary>
        /// Determine if a valid filter was provided.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return base.IsValid()
                || !string.IsNullOrWhiteSpace(this.PinOrPid)
                || !string.IsNullOrWhiteSpace(this.Address);
        }
        #endregion
    }
}
