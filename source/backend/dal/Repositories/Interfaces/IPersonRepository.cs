using System;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Interface that provides a service layer to administer persons within the datasource.
    /// </summary>
    public interface IPersonRepository : IRepository<PimsPerson>
    {
        long GetRowVersion(long id);

        PimsPerson GetById(long id);

        PimsPerson Add(PimsPerson person, bool userOverride);

        PimsPerson Update(PimsPerson person);

        PimsPerson GetPersonAtTime(long personId, DateTime time);
    }
}
