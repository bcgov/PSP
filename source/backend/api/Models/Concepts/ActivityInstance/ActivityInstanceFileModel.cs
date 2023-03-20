namespace Pims.Api.Models.Concepts
{
    public class ActivityInstanceFileModel : BaseAppModel
    {
        public long FileId { get; set; }

        public ActivityInstanceModel Activity { get; set; }
    }
}
