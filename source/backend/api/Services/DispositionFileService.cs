using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Api.Constants;
using Pims.Dal.Constants;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class DispositionFileService : IDispositionFileService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IDispositionFileRepository _dispositionFileRepository;
        private readonly IDispositionFilePropertyRepository _dispositionFilePropertyRepository;
        private readonly ICoordinateTransformService _coordinateService;

        public DispositionFileService(
            ClaimsPrincipal user,
            ILogger<DispositionFileService> logger,
            IDispositionFileRepository dispositionFileRepository,
            IDispositionFilePropertyRepository dispositionFilePropertyRepository,
            ICoordinateTransformService coordinateService)
        {
            _user = user;
            _logger = logger;
            _dispositionFileRepository = dispositionFileRepository;
            _dispositionFilePropertyRepository = dispositionFilePropertyRepository;
            _coordinateService = coordinateService;
        }

        public PimsDispositionFile Add(PimsDispositionFile dispositionFile)
        {
            _logger.LogInformation("Creating Disposition File {dispositionFile}", dispositionFile);
            _user.ThrowIfNotAuthorized(Permissions.DispositionAdd);

            dispositionFile.DispositionStatusTypeCode ??= EnumDispositionStatusTypeCode.UNKNOWN.ToString();
            dispositionFile.DispositionFileStatusTypeCode ??= EnumDispositionFileStatusTypeCode.ACTIVE.ToString();

            _dispositionFileRepository.Add(dispositionFile);
            _dispositionFilePropertyRepository.CommitTransaction();

            return dispositionFile;
        }

        public PimsDispositionFile GetById(long id)
        {
            _logger.LogInformation("Getting disposition file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            var dispositionFile = _dispositionFileRepository.GetById(id);

            return dispositionFile;
        }

        public LastUpdatedByModel GetLastUpdateInformation(long dispositionFileId)
        {
            _logger.LogInformation("Retrieving last updated-by information...");
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            return _dispositionFileRepository.GetLastUpdateBy(dispositionFileId);
        }

        public IEnumerable<PimsPropertyDispositionFile> GetProperties(long id)
        {
            _logger.LogInformation("Getting disposition file properties with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            var properties = _dispositionFilePropertyRepository.GetPropertiesByDispositionFileId(id);
            ReprojectPropertyLocationsToWgs84(properties);
            return properties;
        }

        private void ReprojectPropertyLocationsToWgs84(IEnumerable<PimsPropertyDispositionFile> dispositionPropertyFiles)
        {
            foreach (var dispositionProperty in dispositionPropertyFiles)
            {
                if (dispositionProperty.Property.Location != null)
                {
                    var oldCoords = dispositionProperty.Property.Location.Coordinate;
                    var newCoords = _coordinateService.TransformCoordinates(SpatialReference.BCALBERS, SpatialReference.WGS84, oldCoords);
                    dispositionProperty.Property.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.WGS84);
                }
            }
        }
    }
}
