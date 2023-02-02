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

        public string ProjectRegionCode { get; set; }

        public static explicit operator ProjectFilter(ProjectFilterModel model)
        {
            var filter = new ProjectFilter()
            {
                Page = model.Page,
                Quantity = model.Quantity,

                ProjectNumber = model.ProjectNumber,
                ProjectName = model.ProjectName,
                ProjectStatusCode = model.ProjectStatusCode,
                ProjectRegionCode = model.ProjectRegionCode,

                Sort = model.Sort,
            };

            return filter;
        }
    }
}
