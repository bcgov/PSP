namespace Pims.Api.Models.Concepts
{
    public class OrganizationPersonModel : BaseModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship person.
        /// </summary>
        public PersonModel Person { get; set; }

        /// <summary>
        /// get/set - The relationship person id.
        /// </summary>
        public long PersonId { get; set; }

        /// <summary>
        /// get/set - The relationship organization id.
        /// </summary>
        public long OrganizationId { get; set; }

        /// <summary>
        /// get/set - True if the model is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }
        #endregion
    }
}
