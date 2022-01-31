namespace Pims.Dal
{
    public interface IRepository<EntityType> : IRepository
    {
        #region Methods
        EntityType Find(params object[] keyValues);
        #endregion
    }
}
