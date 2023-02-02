using System.Collections.Generic;
using Pims.Api.Models.Concepts;

namespace Pims.Api.Services
{
    public interface IFinancialCodeService
    {
        IList<FinancialCodeModel> GetAllFinancialCodes();
    }
}
