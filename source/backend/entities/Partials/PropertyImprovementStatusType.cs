using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropImprvmntStatusType class, provides an entity for the datamodel to manage Property's Improvement Status types.
    /// </summary>
    public partial class PimsPropImprvmntStatusType : ITypeEntity<string>
    {
        [NotMapped]
        public string Id { get => PropImprvmntStatusTypeCode; set => PropImprvmntStatusTypeCode = value; }

        public PimsPropImprvmntStatusType()
        {
        }

        public PimsPropImprvmntStatusType(string id)
        {
            Id = id;
        }
    }
}
