using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// ActivityInstanceModel class, provides a model to represent activity associated to entities.
    /// </summary>
    public class ActivityInstanceModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - Activity Instance Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - Activity Instance Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - Activity Instance Status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// get/set - Activity Template Id .
        /// </summary>
        public int ActivityTemplateId { get; set; }

        /// <summary>
        /// get/set - JSON form data .
        /// </summary>
        public string ActivityDataJson { get; set; }

        /// <summary>
        /// get/set - Activity Template Id .
        /// </summary>
        public TypeModel<string> ActivityTemplateTypeCode { get; set; }

        /// <summary>
        /// get/set - Activity Template Type Code .
        /// </summary>
        public ActivityTemplateModel ActivityTemplate { get; set; }

        /// <summary>
        /// get/set - Activity Status Type Code .
        /// </summary>
        public TypeModel<string> ActivityStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The list of properties that are part of this research file and this activity.
        /// </summary>
        public IList<ActivityInstancePropertyFileModel> ActInstPropRsrchFiles { get; set; }

        /// <summary>
        /// get/set - The list of properties that are part of this acquisition file and this activity.
        /// </summary>
        public IList<ActivityInstancePropertyFileModel> ActInstPropAcqFiles { get; set; }
        #endregion
    }
}
