using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsFormType class, provides an entity for the datamodel to manage a list of property road types.
    /// </summary>
    public partial class PimsFormType : StandardIdentityBaseAppEntity<string>, IBaseEntity, ITypeEntity<string>
    {
        #region Properties

        [NotMapped]
        public override string Internal_Id { get => FormTypeCode; set => FormTypeCode = value; }

        [NotMapped]
        public string Id { get => this.FormTypeCode; set => this.FormTypeCode = value; }
        #endregion
    }
}
