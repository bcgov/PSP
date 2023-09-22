namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// PropertyContactModel class, provides a model to represent the property contacts.
    /// </summary>
    public class PropertyContactModel : BaseAppModel
    {
        #region Properties

        public long Id { get; set; }

        public long PropertyId { get; set; }

        public long? OrganizationId { get; set; }

        public OrganizationModel Organization { get; set; }

        public long? PersonId { get; set; }

        public PersonModel Person { get; set; }

        public long? PrimaryContactId { get; set; }

        public PersonModel PrimaryContact { get; set; }

        public string Purpose { get; set; }

        public bool? IsDisabled { get; set; }

        #endregion
    }
}
