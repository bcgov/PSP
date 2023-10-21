namespace Pims.Api.Models.Concepts
{
    public class PropertyManagementActivityModel : BaseAppModel
    {
        public long Id { get; set; }

        public long PropertyId { get; set; }

        public long PropertyActivityId { get; set; }

        public PropertyActivityModel Activity { get; set; }
    }
}
