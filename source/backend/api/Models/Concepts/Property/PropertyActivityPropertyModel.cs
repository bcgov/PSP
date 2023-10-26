namespace Pims.Api.Models.Concepts
{
    public class PropertyActivityPropertyModel : BaseAppModel
    {
        #region Properties

        public long Id { get; set; }

        public long PropertyActivityId { get; set; }

        public PropertyActivityModel PropertyActivity { get; set; }

        public long PropertyId { get; set; }

        public PropertyModel Property { get; set; }

        #endregion
    }
}
