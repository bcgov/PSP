namespace Pims.Dal.Entities
{
    public interface IDisableBaseAppEntity : IBaseAppEntity
    {
        bool? IsDisabled { get; set; }
    }
}
