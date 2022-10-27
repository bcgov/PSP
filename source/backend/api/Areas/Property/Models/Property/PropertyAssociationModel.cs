using System.Collections.Generic;

namespace Pims.Api.Areas.Property.Models.Property
{
    public class PropertyAssociationModel
    {
        public string Id { get; set; }

        public string Pid { get; set; }

        public IList<AssociationModel> LeaseAssociations { get; set; }

        public IList<AssociationModel> ResearchAssociations { get; set; }

        public IList<AssociationModel> AcquisitionAssociations { get; set; }
    }
}
