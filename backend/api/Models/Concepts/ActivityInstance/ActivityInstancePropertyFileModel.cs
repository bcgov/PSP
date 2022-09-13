namespace Pims.Api.Models.Concepts
{
    public class ActivityInstancePropertyFileModel : BaseAppModel
    {
        public long Id { get; set; }

        public long ActivityId { get; set; }

        public long PropertyFileId { get; set; }
    }
}
