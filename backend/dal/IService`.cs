namespace Pims.Dal
{
    // TODO: File name has a tilde
    public interface IService<EntityType> : IService
    {
        #region Methods
        EntityType Find(params object[] keyValues);
        #endregion
    }
}
