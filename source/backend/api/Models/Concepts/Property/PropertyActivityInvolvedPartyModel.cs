namespace Pims.Api.Models.Concepts
{
    public class PropertyActivityInvolvedPartyModel : BaseAppModel
    {
        #region Properties

        public long Id { get; set; }

        public long? OrganizationId { get; set; }

        public OrganizationModel Organization { get; set; }

        public long? PersonId { get; set; }

        public PersonModel Person { get; set; }

        public long PropertyActivityId { get; set; }

        public PropertyActivityModel PropertyActivity { get; set; }

        #endregion
    }
}
