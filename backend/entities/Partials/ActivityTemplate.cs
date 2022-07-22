using System.ComponentModel.DataAnnotations.Schema;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsVolumetricType class, provides an entity for the datamodel to manage a list of volumetric types.
    /// </summary>
    public partial class PimsActivityTemplate : IdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify volumetric type.
        /// </summary>
        [NotMapped]
        public override long Id { get => ActivityTemplateId; set => ActivityTemplateId = value; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PimsVolumetricType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsActivityTemplate(long id,string activityTemplateTypeCode, PimsActivityTemplateType pimsActivityTemplateType ) : this()
        {
            Id = id;
            this.ActivityTemplateTypeCode = activityTemplateTypeCode;
            this.ActivityTemplateTypeCodeNavigation = pimsActivityTemplateType;
        }
        #endregion
    }
}
