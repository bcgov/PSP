using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionAgreementType class, provides an entity for the datamodel to manage Disposition Agreement Type types.
    /// </summary>
    public partial class PimsDispositionAgreementType : ITypeEntity<string>
    {
        [NotMapped]
        public string Id { get => DispositionAgreementTypeCode; set => DispositionAgreementTypeCode = value; }

        public PimsDispositionAgreementType()
        {
        }

        public PimsDispositionAgreementType(string id)
        {
            Id = id;
        }
    }
}
