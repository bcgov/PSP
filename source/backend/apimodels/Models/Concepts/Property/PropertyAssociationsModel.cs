using System.Collections.Generic;

namespace Pims.Api.Models.Concepts.Property
{
    public class PropertyAssociationsModel
    {
        public long Id { get; set; }

        public string Pid { get; set; }

        public IList<AssociationModel> LeaseAssociations { get; set; }

        public IList<AssociationModel> ResearchAssociations { get; set; }

        public IList<AssociationModel> AcquisitionAssociations { get; set; }

        public IList<AssociationModel> DispositionAssociations { get; set; }
    }
}
