namespace Pims.Api.Models.Lookup
{
    public class CommonLookupModel<T> : LookupModel
    {
        #region Properties
        public T Id { get; set; }
        #endregion
    }
}
