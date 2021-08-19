namespace Pims.Dal
{
    public interface IService
    {
        #region Methods
        T OriginalValue<T>(object entity, string propertyName);
        void CommitTransaction();
        #endregion
    }
}
