using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Pims.Av
{
    public interface IAvService
    {
        public Task ScanAsync(IFormFile file);

        public Task ScanAsync(byte[] fileData);
    }
}
