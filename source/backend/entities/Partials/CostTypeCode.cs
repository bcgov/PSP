using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsCostTypeCode class, provides an entity for the datamodel to manage a list of cost type codes.
    /// </summary>
    public partial class PimsCostTypeCode : IFinancialCodeEntity
    {
        [NotMapped]
        public long Internal_Id { get => this.Id; set => this.Id = value; }
    }
}
