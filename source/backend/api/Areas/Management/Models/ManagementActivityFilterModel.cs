using System;
using System.Collections.Generic;
using Pims.Core.Extensions;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Areas.Management.Models
{
    public class ManagementActivityFilterModel : PageFilter
    {
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
        /// get/set - The Activity Type.
        /// </summary>
        public string ActivityTypeCode { get; set; }

        /// <summary>
        /// get/set - The Activity Status Code.
        /// </summary>
        public string ActivityStatusCode { get; set; }

        /// <summary>
        /// get/set - The MOTI project name or the project number, search for both simultaneously.
        /// </summary>
        public string ProjectNameOrNumber { get; set; }

        public ManagementActivityFilterModel()
        {
        }

        public ManagementActivityFilterModel(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
            : base(query)
        {
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            Pid = filter.GetStringValue(nameof(Pid));
            Pin = filter.GetStringValue(nameof(Pin));
            Address = filter.GetStringValue(nameof(Address));
            FileNameOrNumberOrReference = filter.GetStringValue(nameof(FileNameOrNumberOrReference));
            ActivityTypeCode = filter.GetStringValue(nameof(ActivityTypeCode));
            ActivityStatusCode = filter.GetStringValue(nameof(ActivityStatusCode));
            ProjectNameOrNumber = filter.GetStringValue(nameof(ProjectNameOrNumber));
        }

        /// <summary>
        /// Convert to a ManagementActivityFilter.
        /// </summary>
        /// <param name="model"></param>
        public static explicit operator ManagementActivityFilter(ManagementActivityFilterModel model)
        {
            var filter = new ManagementActivityFilter
            {
                Page = model.Page,
                Quantity = model.Quantity,

                Pid = model.Pid,
                Pin = model.Pin,
                Address = model.Address,
                FileNameOrNumberOrReference = model.FileNameOrNumberOrReference,
                ActivityTypeCode = model.ActivityTypeCode,
                ActivityStatusCode = model.ActivityStatusCode,
                ProjectNameOrNumber = model.ProjectNameOrNumber,

                Sort = model.Sort,
            };

            return filter;
        }
    }
}
