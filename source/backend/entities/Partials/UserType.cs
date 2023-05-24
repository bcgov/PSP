using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsUserType class, provides an entity for the datamodel to manage User types.
    /// </summary>
    public partial class PimsUserType : ITypeEntity<string>
    {
        public PimsUserType(string id)
            : this()
        {
            Id = id;
        }

        [NotMapped]
        public string Id { get => UserTypeCode; set => UserTypeCode = value; }
    }

    public enum EnumUserTypeCodes
    {
        CONTRACT,
        MINSTAFF,
    }
}
