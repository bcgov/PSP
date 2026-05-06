namespace Pims.Scheduler.Models.Base
{
    public class BaseTaskResponseModel
    {
        public TaskResponseStatusTypes Status { get; set; }

        public string Message { get; set; }
    }
}
