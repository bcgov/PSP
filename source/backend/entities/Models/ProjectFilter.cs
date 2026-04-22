using System.Collections.Generic;

namespace Pims.Dal.Entities.Models
{
    public class ProjectFilter : PageFilter
    {
        public ProjectFilter()
        {
        }

        public string ProjectNumber { get; set; }

        public string ProjectName { get; set; }

        public string ProjectStatusCode { get; set; }

        /// <summary>
        /// get/set - The region types.
        /// </summary>
        public IList<short> Regions { get; set; } = new List<short>();

    }
}
