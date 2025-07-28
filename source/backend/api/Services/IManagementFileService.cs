using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;

namespace Pims.Api.Services
{
    public interface IManagementFileService
    {
        PimsManagementFile GetById(long id);

        PimsManagementFile Add(PimsManagementFile managementFile, IEnumerable<UserOverrideCode> userOverrides);

        PimsManagementFile Update(long id, PimsManagementFile managementFile, IEnumerable<UserOverrideCode> userOverrides);

        LastUpdatedByModel GetLastUpdateInformation(long managementFileId);

        IEnumerable<PimsManagementFileProperty> GetProperties(long id);

        IEnumerable<PimsManagementFileTeam> GetTeamMembers();

        PimsManagementFile UpdateProperties(PimsManagementFile managementFile, IEnumerable<UserOverrideCode> userOverrides);

        Paged<PimsManagementFile> GetPage(ManagementFilter filter);

        IEnumerable<PimsManagementFileContact> GetContacts(long id);

        PimsManagementFileContact GetContact(long managementFileId, long contactId);

        PimsManagementFileContact AddContact(PimsManagementFileContact contact);

        PimsManagementFileContact UpdateContact(PimsManagementFileContact contact);

        bool DeleteContact(long managementFileId, long contactId);
    }
}
