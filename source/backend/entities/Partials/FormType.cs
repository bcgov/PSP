using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsFormType class, provides an entity for the datamodel to manage form types.
    /// </summary>
    public partial class PimsFormType : ITypeEntity<string>
    {
        #region Properties
        [NotMapped]
        public string Id { get => this.FormTypeCode; set => this.FormTypeCode = value; }
        #endregion
    }
}
