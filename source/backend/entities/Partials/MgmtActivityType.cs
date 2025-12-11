using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropMgmtActivityType class, provides an entity for the datamodel to manage management activities types.
    /// </summary>
    public partial class PimsMgmtActivityType : ITypeEntity<string>
    {
        [NotMapped]
        public string Id { get => MgmtActivityTypeCode; set => MgmtActivityTypeCode = value; }
    }
}
