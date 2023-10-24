namespace Pims.Api.Models.Concepts
{
    public class PersonOrganizationModel : BaseModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship person id.
        /// </summary>
        public long PersonId { get; set; }

        /// <summary>
        /// get/set - The relationship organization.
        /// </summary>
        public OrganizationModel Organization { get; set; }
        #endregion
    }
}
