using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface IManagementFileRepository : IRepository
    {
        PimsManagementFile GetById(long id);

        PimsManagementFile GetByName(string name);

        PimsManagementFile Add(PimsManagementFile managementFile);

        PimsManagementFile Update(long managementFileId, PimsManagementFile managementFile);

        LastUpdatedByModel GetLastUpdateBy(long id);

        List<PimsManagementFileTeam> GetTeamMembers();

        long GetRowVersion(long id);

        Paged<PimsManagementFile> GetPageDeep(ManagementFilter filter);

        List<PimsManagementFileContact> GetContacts(long managementFileId);

        PimsManagementFileContact GetContact(long managementFileId, long contactId);

        PimsManagementFileContact AddContact(PimsManagementFileContact contact);

        PimsManagementFileContact UpdateContact(PimsManagementFileContact contact);

        void DeleteContact(long managementFileId, long contactId);
    }
}
