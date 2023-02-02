using System.Collections.Generic;
using Pims.Api.Models.Concepts;

namespace Pims.Api.Services
{
    public interface IFinancialCodeService
    {
        IList<FinancialCodeModel> GetAllFinancialCodes();

        FinancialCodeModel GetById(FinancialCodeTypes type, long codeId);

        FinancialCodeModel Add(FinancialCodeTypes type, FinancialCodeModel model);

        FinancialCodeModel Update(FinancialCodeTypes type, FinancialCodeModel model);
    }
}
