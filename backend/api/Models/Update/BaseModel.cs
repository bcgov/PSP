namespace Pims.Api.Models.Update
{
    public abstract class BaseModel
    {
        #region Properties
        public long RowVersion { get; set; }
        #endregion
    }
}
