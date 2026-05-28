using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionFormType class, provides an entity for the datamodel to manage Disposition Forms.
    /// </summary>
    public partial class PimsDispositionFormType : ITypeEntity<string>
    {
        [NotMapped]
        public string Id { get => DispositionFormTypeCode; set => DispositionFormTypeCode = value; }

        public PimsDispositionFormType()
        {
        }

        public PimsDispositionFormType(string id)
        {
            Id = id;
        }
    }
}
