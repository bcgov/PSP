namespace Pims.Dal.Entities
{
    public interface IDisableBaseAppEntity : IBaseAppEntity
    {
        public bool? IsDisabled { get; set; }
    }
}
