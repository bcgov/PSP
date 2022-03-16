namespace Pims.Api.Models.Concepts
{
    public class PersonOrganizationModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - The relationship person.
        /// </summary>
        public PersonModel Person { get; set; }

        /// <summary>
        /// get/set - The relationship organization.
        /// </summary>
        public OrganizationModel Organization { get; set; }

        /// <summary>
        /// get/set - True if the model is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }
        #endregion
    }
}
