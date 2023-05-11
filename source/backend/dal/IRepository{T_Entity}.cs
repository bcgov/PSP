namespace Pims.Dal
{
    public interface IRepository<T_Entity> : IRepository
    {
        #region Methods
        T_Entity Find(params object[] keyValues);
        #endregion
    }
}
