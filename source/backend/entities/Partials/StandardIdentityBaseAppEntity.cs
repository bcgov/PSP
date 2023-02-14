namespace Pims.Dal.Entities
{
    /// <summary>
    /// StandardIdentityBaseAppEntity abstract class, provides an internal representation of the class id as an standard member.
    /// </summary>
    public abstract class StandardIdentityBaseAppEntity<T>
    {
        public abstract T Internal_Id { get; set; }
    }
}
