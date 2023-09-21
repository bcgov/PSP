
namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// PropertyContactModel class, provides a model to represent the property contacts.
    /// </summary>
    public class PropertyContactModel : BaseModel
    {
        #region Properties

        public long Id { get; set; }

        public OrganizationModel Organization { get; set; }

        public PersonModel Person { get; set; }

        public PersonModel PrimaryContact { get; set; }

        public string Purpose { get; set; }

        #endregion
    }
}
