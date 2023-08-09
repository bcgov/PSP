using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public class ExpropriationPaymentRepository : BaseRepository<PimsExpropriationPayment>, IExpropriationPaymentRepository
    {
        public ExpropriationPaymentRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<BaseRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        public PimsExpropriationPayment Add(PimsExpropriationPayment expropriationPayment)
        {
            Context.PimsExpropriationPayments.Add(expropriationPayment);

            return expropriationPayment;
        }

        public IList<PimsExpropriationPayment> GetAllByAcquisitionFileId(long acquisitionFileId)
        {
            return Context.PimsExpropriationPayments
                .Include(x => x.PimsExpropPmtPmtItems)
                    .ThenInclude(y => y.PaymentItemTypeCodeNavigation)
                .Include(x => x.InterestHolder)
                .Include(x => x.AcquisitionOwner)
                .Include(x => x.ExpropriatingAuthorityNavigation)
                .AsNoTracking()
                .Where(x => x.AcquisitionFileId == acquisitionFileId)
                .ToList();
        }

        public PimsExpropriationPayment GetById(long expropriationPaymentId)
        {
            return Context.PimsExpropriationPayments
                .Include(x => x.PimsExpropPmtPmtItems)
                    .ThenInclude(y => y.PaymentItemTypeCodeNavigation)
                .Include(x => x.InterestHolder)
                .Include(x => x.AcquisitionOwner)
                .Include(x => x.ExpropriatingAuthorityNavigation)
                .AsNoTracking()
                .FirstOrDefault(x => x.ExpropriationPaymentId.Equals(expropriationPaymentId)) ?? throw new KeyNotFoundException();
        }
    }
}
