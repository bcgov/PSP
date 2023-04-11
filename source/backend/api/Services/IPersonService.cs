using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IPersonService
    {
        PimsPerson GetPerson(long id);

        PimsPerson AddPerson(PimsPerson person, bool userOverride);

        PimsPerson UpdatePerson(PimsPerson person, long? rowVersion);
    }
}
