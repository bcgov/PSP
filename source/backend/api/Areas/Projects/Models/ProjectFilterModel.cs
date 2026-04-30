using System.Collections.Generic;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Areas.Projects.Models
{
    public class ProjectFilterModel : PageFilter
    {
        public ProjectFilterModel()
        {
        }

        public string ProjectNumber { get; set; }

        public string ProjectName { get; set; }

        public string ProjectStatusCode { get; set; }

        /// <summary>
        /// get/set - The region types.
        /// </summary>
        public IList<short> Regions { get; set; } = new List<short>();

        public static explicit operator ProjectFilter(ProjectFilterModel model)
        {
            var filter = new ProjectFilter()
            {
                Page = model.Page,
                Quantity = model.Quantity,

                ProjectNumber = model.ProjectNumber?.Trim(),
                ProjectName = model.ProjectName?.Trim(),
                ProjectStatusCode = model.ProjectStatusCode,
                Regions = model.Regions,

                Sort = model.Sort,
            };

            return filter;
        }
    }
}
