using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsFormType class, provides an entity for the datamodel to manage a list of property road types.
    /// </summary>
    public partial class PimsFormType : StandardIdentityBaseAppEntity<string>, IBaseEntity
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify a road type.
        /// </summary>
        [NotMapped]
        public override string Internal_Id { get => FormTypeCode; set => FormTypeCode = value; }
        #endregion
    }
}
