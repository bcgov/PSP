using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsActivityTemplate class, provides an entity for the datamodel to manage a list of activity template types.
    /// </summary>
    public partial class PimsActivityTemplate : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify volumetric type.
        /// </summary>
        [NotMapped]
        public override long Internal_Id { get => ActivityTemplateId; set => ActivityTemplateId = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsActivityTemplate class.
        /// </summary>
        /// <param name="id"></param>
        public PimsActivityTemplate(long id, string activityTemplateTypeCode, PimsActivityTemplateType pimsActivityTemplateType)
            : this()
        {
            Internal_Id = id;
            this.ActivityTemplateTypeCode = activityTemplateTypeCode;
            this.ActivityTemplateTypeCodeNavigation = pimsActivityTemplateType;
        }
        #endregion
    }
}
