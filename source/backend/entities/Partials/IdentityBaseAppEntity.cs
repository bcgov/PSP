namespace Pims.Dal.Entities
{
    public abstract class IdentityBaseAppEntity<T> : IIdentityEntity<T>
    {
        public abstract T Internal_Id { get; set; }
    }
}
