using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Services
{
    public interface IResearchFileService
    {
        PimsResearchFile GetById(long id);

        IEnumerable<PimsPropertyResearchFile> GetProperties(long researchFileId);

        PimsResearchFile Add(PimsResearchFile researchFile);

        PimsResearchFile Update(PimsResearchFile researchFile);

        PimsResearchFile UpdateProperties(PimsResearchFile researchFile);

        Paged<PimsResearchFile> GetPage(ResearchFilter filter);

        PimsResearchFile UpdateProperty(long researchFileId, long? researchFileVersion, PimsPropertyResearchFile propertyResearchFile);
    }
}
