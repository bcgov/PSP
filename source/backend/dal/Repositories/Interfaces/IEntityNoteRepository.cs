using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IEntityNoteRepository : IRepository
    {
        T Add<T>(T entity)
            where T : class;

        IEnumerable<PimsNote> GetAllAcquisitionNotesById(long acquisitionId);

        IEnumerable<PimsNote> GetAllDispositionNotesById(long dispositionId);

        IEnumerable<PimsNote> GetAllLeaseNotesById(long leaseId);

        IEnumerable<PimsNote> GetAllProjectNotesById(long projectId);

        IEnumerable<PimsNote> GetAllResearchNotesById(long researchId);

        IEnumerable<PimsNote> GetAllManagementNotesById(long managementId);

        bool DeleteAcquisitionFileNotes(long noteId);

        bool DeleteDispositionFileNotes(long noteId);

        bool DeleteLeaseFileNotes(long noteId);

        bool DeleteProjectNotes(long noteId);

        bool DeleteResearchNotes(long noteId);

        bool DeleteManagementFileNotes(long noteId);
    }
}
