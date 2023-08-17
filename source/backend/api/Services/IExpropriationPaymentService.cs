using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IExpropriationPaymentService
    {
        PimsExpropriationPayment GetById(long id);

        PimsExpropriationPayment Update(PimsExpropriationPayment expropriationPayment);

        bool Delete(long id);
    }
}
