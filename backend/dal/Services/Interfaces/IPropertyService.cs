using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    public interface IPropertyService
    {
        PimsProperty GetByPid(string pid);
        PimsProperty Update(PimsProperty property);
    }
}
