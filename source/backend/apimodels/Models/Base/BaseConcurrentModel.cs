namespace Pims.Api.Models.Base
{
    public abstract class BaseConcurrentModel
    {
        #region Properties
        public long? RowVersion { get; set; }
        #endregion
    }
}
