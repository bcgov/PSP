using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsBusinessFunctionCode class, provides an entity for the datamodel to manage a list of business function codes.
    /// </summary>
    public partial class PimsBusinessFunctionCode : IFinancialCodeEntity
    {
        [NotMapped]
        public long Internal_Id { get => this.Id; set => this.Id = value; }
    }
}
