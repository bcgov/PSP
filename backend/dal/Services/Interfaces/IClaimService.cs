using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using System;

namespace Pims.Dal.Services
{
    /// <summary>
    /// IClaimService interface, provides a service layer to administer roles within the datasource.
    /// </summary>
    public interface IClaimService : IService<Claim>
    {
        Paged<Claim> Get(int page, int quantity, string name = null);
        Claim Get(Guid key);
        Claim GetByName(string name);
        int RemoveAll(Guid[] exclude);
        Claim Add(Claim add);
        Claim Update(Claim update);
        void Delete(Claim delete);
    }
}
