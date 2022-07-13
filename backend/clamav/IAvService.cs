using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Pims.Av
{
    public interface IAvService
    {
        public Task Scan(IFormFile file);
        public Task Scan(byte[] fileData);
    }
}
