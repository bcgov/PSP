using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    public interface IPropertyService
    {
        PimsProperty GetById(long id);

        PimsProperty GetByPid(string pid);

        PimsProperty Update(PimsProperty property);
    }
}
