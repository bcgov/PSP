namespace Pims.Dal
{
    public interface IService<EntityType> : IService
    {
        #region Methods
        EntityType Find(params object[] keyValues);
        #endregion
    }
}
