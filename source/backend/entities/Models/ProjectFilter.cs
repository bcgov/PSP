namespace Pims.Dal.Entities.Models
{
    public class ProjectFilter : PageFilter
    {
        public ProjectFilter()
        {
        }

        public ProjectFilter(string projectNumber, string projectName, string projectStatus, string projectRegion)
        {
            this.ProjectNumber = projectNumber;
            this.ProjectName = projectName;
            this.ProjectStatusCode = projectStatus;
            this.ProjectRegionCode = projectRegion;
        }

        public string ProjectNumber { get; set; }

        public string ProjectName { get; set; }

        public string ProjectStatusCode { get; set; }

        public string ProjectRegionCode { get; set; }
    }
}
