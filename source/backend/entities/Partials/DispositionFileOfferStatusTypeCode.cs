using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionOfferStatusType class, provides an entity for the datamodel.
    /// </summary>
    public partial class PimsDispositionOfferStatusType : ITypeEntity<string>
    {
        public PimsDispositionOfferStatusType(string id)
            : this()
        {
            Id = id;
        }

        [NotMapped]
        public string Id { get => DispositionOfferStatusTypeCode; set => DispositionOfferStatusTypeCode = value; }
    }
}
