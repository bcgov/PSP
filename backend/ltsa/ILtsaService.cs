using System.Threading.Tasks;
using Pims.Ltsa.Models;

namespace Pims.Ltsa
{
    public interface ILtsaService
    {
        Task<TitleSummariesResponse> GetTitleSummariesAsync(int pid);

        Task<OrderWrapper<OrderParent<Title>>> PostTitleOrder(string titleNumber, string landTitleDistrictCode);

        Task<OrderWrapper<OrderParent<ParcelInfo>>> PostParcelInfoOrder(string pid);

        Task<OrderWrapper<OrderParent<StrataPlanCommonProperty>>> PostSpcpOrder(string strataPlanNumber);

        Task<LtsaOrders> PostLtsaFields(string pid);
    }
}
