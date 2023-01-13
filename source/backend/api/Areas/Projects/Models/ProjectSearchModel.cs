using System;

namespace Pims.Api.Areas.Projects.Models
{
    public class ProjectSearchModel
    {
        public long Id { get; set; }

        public string Status { get; set; }

        public int Code { get; set; }

        public string Description { get; set; }

        public string Region { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
