using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyMgmtActivityStatusType class, provides an entity for the datamodel to manage types.
    /// </summary>
    public partial class PimsPropMgmtActivityStatusType : ITypeEntity<string>
    {
        [NotMapped]
        public string Id { get => PropMgmtActivityStatusTypeCode; set => PropMgmtActivityStatusTypeCode = value; }
    }
}
