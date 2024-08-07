using System.Collections.Generic;
using Pims.Api.Models.CodeTypes;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ICompensationRequisitionService
    {
        PimsCompensationRequisition GetById(long compensationRequisitionId);

        PimsCompensationRequisition Update(PimsCompensationRequisition compensationRequisition);

        bool DeleteCompensation(long compensationId);

        IEnumerable<PimsPropertyAcquisitionFile> GetProperties(long id);

        IEnumerable<PimsCompensationRequisition> GetFileCompensationRequisitions(FileTypes fileType, long fileId);

        PimsCompensationRequisition AddCompensationRequisition(FileTypes fileType, PimsCompensationRequisition compensationRequisition);
    }
}
