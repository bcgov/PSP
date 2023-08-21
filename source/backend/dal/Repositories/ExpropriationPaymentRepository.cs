using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

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
                    .ThenInclude(y => y.Person)
                .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.Organization)
                .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.InterestHolderTypeCodeNavigation)
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
                    .ThenInclude(y => y.Person)
                    .ThenInclude(z => z.PimsPersonAddresses)
                    .ThenInclude(u => u.AddressUsageTypeCodeNavigation)
                .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.Person)
                    .ThenInclude(z => z.PimsPersonAddresses)
                    .ThenInclude(a => a.Address)
                    .ThenInclude(c => c.Country)
                 .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.Person)
                    .ThenInclude(z => z.PimsPersonAddresses)
                    .ThenInclude(a => a.Address)
                    .ThenInclude(c => c.ProvinceState)
                .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.Organization)
                    .ThenInclude(z => z.PimsOrganizationAddresses)
                    .ThenInclude(u => u.AddressUsageTypeCodeNavigation)
                .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.Organization)
                    .ThenInclude(z => z.PimsOrganizationAddresses)
                    .ThenInclude(a => a.Address)
                    .ThenInclude(c => c.Country)
                .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.Organization)
                    .ThenInclude(z => z.PimsOrganizationAddresses)
                    .ThenInclude(a => a.Address)
                    .ThenInclude(c => c.ProvinceState)
                .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.InterestHolderTypeCodeNavigation)
                .Include(x => x.AcquisitionOwner)
                    .ThenInclude(y => y.Address)
                    .ThenInclude(z => z.Country)
                .Include(x => x.AcquisitionOwner)
                    .ThenInclude(y => y.Address)
                    .ThenInclude(z => z.ProvinceState)
                .Include(x => x.ExpropriatingAuthorityNavigation)
                    .ThenInclude(y => y.PimsOrganizationAddresses)
                    .ThenInclude(z => z.Address)
                    .ThenInclude(c => c.Country)
                .Include(x => x.ExpropriatingAuthorityNavigation)
                    .ThenInclude(y => y.PimsOrganizationAddresses)
                    .ThenInclude(z => z.Address)
                    .ThenInclude(c => c.ProvinceState)
                .Include(x => x.ExpropriatingAuthorityNavigation)
                    .ThenInclude(y => y.PimsOrganizationAddresses)
                    .ThenInclude(z => z.AddressUsageTypeCodeNavigation)
                .AsNoTracking()
                .FirstOrDefault(x => x.ExpropriationPaymentId.Equals(expropriationPaymentId)) ?? throw new KeyNotFoundException();
        }

        public PimsExpropriationPayment Update(PimsExpropriationPayment expropriationPayment)
        {
            var existingExpPayment = Context.PimsExpropriationPayments
                .FirstOrDefault(x => x.ExpropriationPaymentId.Equals(expropriationPayment.ExpropriationPaymentId)) ?? throw new KeyNotFoundException();

            Context.Entry(existingExpPayment).CurrentValues.SetValues(expropriationPayment);
            Context.UpdateChild<PimsExpropriationPayment, long, PimsExpropPmtPmtItem, long>(a => a.PimsExpropPmtPmtItems, expropriationPayment.ExpropriationPaymentId, expropriationPayment.PimsExpropPmtPmtItems.ToArray(), true);

            return existingExpPayment;
        }

        public bool TryDelete(long id)
        {
            var deletedEntity = Context.PimsExpropriationPayments
                .Include(x => x.PimsExpropPmtPmtItems)
                .AsNoTracking()
                .FirstOrDefault(c => c.ExpropriationPaymentId == id);

            if (deletedEntity != null)
            {
                foreach (var item in deletedEntity.PimsExpropPmtPmtItems)
                {
                    Context.PimsExpropPmtPmtItems.Remove(new PimsExpropPmtPmtItem() { ExpropPmtPmtItemId = item.ExpropPmtPmtItemId });
                }

                Context.CommitTransaction(); // TODO: required to enforce delete order. Can be removed when cascade deletes are implemented.

                Context.PimsExpropriationPayments.Remove(new PimsExpropriationPayment() { ExpropriationPaymentId = deletedEntity.ExpropriationPaymentId });
                return true;
            }

            return false;
        }
    }
}
