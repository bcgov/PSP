using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;

namespace Pims.Api.Services
{
    public interface IResearchFileService
    {
        PimsResearchFile GetById(long id);

        IEnumerable<PimsPropertyResearchFile> GetProperties(long researchFileId);

        PimsResearchFile Add(PimsResearchFile researchFile, IEnumerable<UserOverrideCode> userOverrideCodes);

        PimsResearchFile Update(PimsResearchFile researchFile);

        PimsResearchFile UpdateProperties(PimsResearchFile researchFile, IEnumerable<UserOverrideCode> userOverrideCodes);

        Paged<PimsResearchFile> GetPage(ResearchFilter filter);

        PimsResearchFile UpdateProperty(long researchFileId, long? researchFileVersion, PimsPropertyResearchFile propertyResearchFile);
    }
}
