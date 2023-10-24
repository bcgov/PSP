namespace Pims.Dal
{
    public interface IRepository<out T_Entity> : IRepository
    {
        #region Methods
        T_Entity Find(params object[] keyValues);
        #endregion
    }
}
