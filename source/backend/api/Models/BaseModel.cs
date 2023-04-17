namespace Pims.Api.Models
{
    public abstract class BaseModel
    {
        #region Properties
        public long? RowVersion { get; set; }
        #endregion
    }
}
