namespace Pims.Api.Models.Concepts
{

    /// <summary>
    /// ActivityTemplateModel class, provides a model to represent activity associated to entities.
    /// </summary>
    public class ActivityTemplateModel : BaseAppModel
    {

        /// <summary>
        /// get/set - Activity Template Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - Activity Template Code.
        /// </summary>
        public TypeModel<string> ActivityTemplateTypeCode { get; set; }
    }
}
