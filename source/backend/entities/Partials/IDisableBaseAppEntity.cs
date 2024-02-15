namespace Pims.Dal.Entities
{
    public interface IDisableBaseAppEntity<T_IsDisabled> : IBaseAppEntity
    {
        T_IsDisabled IsDisabled { get; set; }
    }

    public interface IDisableBaseAppEntity : IDisableBaseAppEntity<bool>
    {
    }
}
