namespace Pims.Dal
{
    public interface IRepository<TEntityType> : IRepository
    {
        #region Methods
        TEntityType Find(params object[] keyValues);
        #endregion
    }
}
