using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropMgmtActivitySubtype class, provides an entity for the datamodel to manage management activities stubtypes.
    /// </summary>
    public partial class PimsPropMgmtActivitySubtype : ITypeEntity<string>
    {
        [NotMapped]
        public string Id { get => PropMgmtActivitySubtypeCode; set => PropMgmtActivitySubtypeCode = value; }
    }
}
