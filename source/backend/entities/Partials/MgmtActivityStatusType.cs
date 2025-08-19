using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsMgmtActivityStatusType class, provides an entity for the datamodel to manage management activities status types.
    /// </summary>
    public partial class PimsMgmtActivityStatusType : ITypeEntity<string>
    {
        [NotMapped]
        public string Id { get => MgmtActivityStatusTypeCode; set => MgmtActivityStatusTypeCode = value; }
    }
}
