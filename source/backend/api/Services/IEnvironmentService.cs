using Pims.Api.Models.Health;

namespace Pims.Api.Services
{
    public interface IEnvironmentService
    {
        EnvModel GetEnvironmentVariables();
    }
}
