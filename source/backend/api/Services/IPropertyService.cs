using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IPropertyService
    {
        PimsProperty GetById(long id);

        PimsProperty GetByPid(string pid);

        PimsProperty Update(PimsProperty property);
    }
}
