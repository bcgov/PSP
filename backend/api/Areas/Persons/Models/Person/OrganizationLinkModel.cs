namespace Pims.Api.Areas.Persons.Models.Person
{
    /// <summary>
    /// Provides a contact-oriented organization model.
    /// This model contains a sub-set of organization fields as it is only used to link a person to an organization.
    /// </summary>
    public class OrganizationLinkModel
    {
        #region Properties

        /// <summary>
        /// get/set - The organization's id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The organization's name.
        /// </summary>
        public string Text { get; set; }
        #endregion
    }
}
