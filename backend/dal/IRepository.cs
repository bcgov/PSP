namespace Pims.Dal
{
    public interface IRepository
    {
        #region Methods
        T OriginalValue<T>(object entity, string propertyName);

        void CommitTransaction();
        #endregion
    }
}
