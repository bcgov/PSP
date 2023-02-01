namespace Pims.Dal.Entities
{
    public abstract class IdentityBaseAppEntity<T> : IIdentityEntity<T>
    {
        public abstract T Id { get; set; }
    }
}
