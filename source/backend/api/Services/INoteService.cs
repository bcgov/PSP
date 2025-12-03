using System.Collections.Generic;
using Pims.Api.Constants;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface INoteService
    {
        PimsNote GetById(long id);

        PimsAcquisitionFileNote AddAcquisitionFileNote(PimsAcquisitionFileNote acquisitionFileNote);

        PimsDispositionFileNote AddDispositionFileNote(PimsDispositionFileNote dispositionFileNote);

        PimsLeaseNote AddLeaseNote(PimsLeaseNote leaseNote);

        PimsManagementFileNote AddManagementFileNote(PimsManagementFileNote managementFileNote);

        PimsProjectNote AddProjectNote(PimsProjectNote projectNote);

        PimsPropertyNote AddPropertyNote(PimsPropertyNote propertyNote);

        PimsResearchFileNote AddResearchFileNote(PimsResearchFileNote researchFileNote);

        PimsNote Update(PimsNote note);

        bool DeleteNote(NoteType type, long noteId, bool commitTransaction = true);

        IEnumerable<PimsNote> GetNotes(NoteType type, long parentId);
    }
}
