using Microsoft.AspNetCore.Hosting;
using Pims.Api.Models.Health;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ISystemConstantRepository _systemConstantRepository;

        public EnvironmentService(IWebHostEnvironment environment, ISystemConstantRepository systemConstantRepository)
        {
            _environment = environment;
            _systemConstantRepository = systemConstantRepository;
        }

        public EnvModel GetEnvironmentVariables()
        {
            EnvModel environment = new(_environment)
            {
                DBVersion = _systemConstantRepository.GetDataBaseVersion(),
            };

            return environment;
        }
    }
}
