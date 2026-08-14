using System.Threading.Tasks;
using Pims.Api.Models.Concepts.Property;

namespace Pims.Api.Repositories.PropertyLookup
{
    public interface IPropertyPmbcRepository
    {
        Task<PropertyModel> GetByPidAsync(string pid);
    }
}
