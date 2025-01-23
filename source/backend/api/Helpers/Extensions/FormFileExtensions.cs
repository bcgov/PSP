using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Pims.Api.Helpers.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }
    }
}
