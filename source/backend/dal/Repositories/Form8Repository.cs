using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public class Form8Repository : BaseRepository<PimsForm8>, IForm8Repository
    {
        public Form8Repository(PimsContext dbContext, ClaimsPrincipal user, ILogger<BaseRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        public PimsForm8 Add(PimsForm8 form8)
        {
            Context.PimsForm8s.Add(form8);

            return form8;
        }

        public IList<PimsForm8> GetAllByAcquisitionFileId(long acquisitionFileId)
        {
            return Context.PimsForm8s
                .Include(x => x.PaymentItemTypeCodeNavigation)
                .Include(x => x.InterestHolder)
                .Include(x => x.AcquisitionOwner)
                .Include(x => x.ExpropriatingAuthorityNavigation)
                .AsNoTracking()
                .Where(x => x.AcquisitionFileId == acquisitionFileId)
                .ToList();
        }

        public PimsForm8 GetById(long form8Id)
        {
            return Context.PimsForm8s
                .Include(x => x.PaymentItemTypeCodeNavigation)
                .Include(x => x.InterestHolder)
                .Include(x => x.AcquisitionOwner)
                .Include(x => x.ExpropriatingAuthorityNavigation)
                .AsNoTracking()
                .FirstOrDefault(x => x.Form8Id.Equals(form8Id)) ?? throw new KeyNotFoundException();
        }
    }
}
