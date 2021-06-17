using System.Collections.Generic;
using Model = Pims.Api.Models;

namespace Pims.Api.Areas.Property.Models.Parcel
{
    public class PropertyModel : Model.BaseModel
    {
        #region Properties
        public long Id { get; set; }

        public long PropertyTypeId { get; set; }

        public IEnumerable<string> ProjectNumbers { get; set; }

        public string ProjectWorkflow { get; set; }

        public string ProjectStatus { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long ClassificationId { get; set; }

        public string Classification { get; set; }

        public string EncumbranceReason { get; set; }

        public long AgencyId { get; set; }

        public virtual string SubAgency { get; set; }

        public string Agency { get; set; }

        public virtual string SubAgencyFullName { get; set; }

        public string AgencyFullName { get; set; }

        public AddressModel Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsSensitive { get; set; }

        public bool IsVisibleToOtherAgencies { get; set; }
        #endregion
    }
}
