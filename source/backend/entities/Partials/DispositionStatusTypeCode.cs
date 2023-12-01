using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionStatusType class, provides an entity for the datamodel to manage Disposition status types.
    /// </summary>
    public partial class PimsDispositionStatusType : ITypeEntity<string>
    {
        public PimsDispositionStatusType(string id)
            : this()
        {
            Id = id;
        }

        [NotMapped]
        public string Id { get => DispositionStatusTypeCode; set => DispositionStatusTypeCode = value; }
    }
}
