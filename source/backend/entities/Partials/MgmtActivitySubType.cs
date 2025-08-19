using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsMgmtActivitySubtype class, provides an entity for the datamodel to manage management activities sub-types.
    /// </summary>
    public partial class PimsMgmtActivitySubtype : ITypeEntity<string>
    {
        [NotMapped]
        public string Id { get => MgmtActivitySubtypeCode; set => MgmtActivitySubtypeCode = value; }
    }
}
