using System;

namespace Pims.Api.Models.Concepts.Property
{
    public class AssociationModel
    {
        public long Id { get; set; }

        public string FileNumber { get; set; }

        public string FileName { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedByGuid { get; set; }

        public string Status { get; set; }
    }
}
