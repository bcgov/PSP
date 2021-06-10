using Pims.Ltsa.Models;
using System.Threading.Tasks;

namespace Pims.Ltsa
{
    public interface ILtsaService
    {
        Task<TitleSummariesResponse> GetTitleSummariesAsync(int pid);
        Task<OrderWrapper<TitleOrder>> PostTitleOrder(string titleNumber, string landTitleDistrictCode);
        Task<OrderWrapper<ParcelInfoOrder>> PostParcelInfoOrder(string pid);
        Task<OrderWrapper<SpcpOrder>> PostSpcpOrder(string strataPlanNumber);
        Task<LtsaOrders> PostLtsaFields(string pid);
    }
}
