using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Core.Exceptions;
using Pims.Dal.Constants;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
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
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyService _propertyService;

        public DispositionFileService(
            ClaimsPrincipal user,
            ILogger<DispositionFileService> logger,
            IDispositionFileRepository dispositionFileRepository,
            IDispositionFilePropertyRepository dispositionFilePropertyRepository,
            ICoordinateTransformService coordinateService,
            IPropertyRepository propertyRepository,
            IPropertyService propertyService)
        {
            _user = user;
            _logger = logger;
            _dispositionFileRepository = dispositionFileRepository;
            _dispositionFilePropertyRepository = dispositionFilePropertyRepository;
            _coordinateService = coordinateService;
            _propertyRepository = propertyRepository;
            _propertyService = propertyService;
        }

        public PimsDispositionFile Add(PimsDispositionFile dispositionFile, IEnumerable<UserOverrideCode> userOverrides)
        {
            _logger.LogInformation("Creating Disposition File {dispositionFile}", dispositionFile);
            _user.ThrowIfNotAuthorized(Permissions.DispositionAdd);

            dispositionFile.DispositionStatusTypeCode ??= EnumDispositionStatusTypeCode.UNKNOWN.ToString();
            dispositionFile.DispositionFileStatusTypeCode ??= EnumDispositionFileStatusTypeCode.ACTIVE.ToString();

            MatchProperties(dispositionFile, userOverrides);

            var newDispositionFile = _dispositionFileRepository.Add(dispositionFile);
            _dispositionFileRepository.CommitTransaction();

            return newDispositionFile;
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

        public Paged<PimsDispositionFile> GetPage(DispositionFilter filter)
        {
            _logger.LogInformation("Searching for disposition files...");
            _logger.LogDebug("Disposition file search with filter: {filter}", filter);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            return _dispositionFileRepository.GetPageDeep(filter);
        }

        public IEnumerable<PimsDispositionFileProperty> GetProperties(long id)
        {
            _logger.LogInformation("Getting disposition file properties with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            var properties = _dispositionFilePropertyRepository.GetPropertiesByDispositionFileId(id);
            ReprojectPropertyLocationsToWgs84(properties);
            return properties;
        }

        public IEnumerable<PimsDispositionFileTeam> GetTeamMembers()
        {
            _logger.LogInformation("Getting disposition team members");
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);
            _user.ThrowIfNotAuthorized(Permissions.ContactView);

            var teamMembers = _dispositionFileRepository.GetTeamMembers();

            var persons = teamMembers.Where(x => x.Person != null).GroupBy(x => x.PersonId).Select(x => x.First());
            var organizations = teamMembers.Where(x => x.Organization != null).GroupBy(x => x.OrganizationId).Select(x => x.First());

            List<PimsDispositionFileTeam> teamFilterOptions = new();
            teamFilterOptions.AddRange(persons);
            teamFilterOptions.AddRange(organizations);

            return teamFilterOptions;
        }

        public IEnumerable<PimsDispositionOffer> GetOffers(long dispositionFileId)
        {
            _logger.LogInformation("Getting disposition file offers with DispositionFileId: {id}", dispositionFileId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            return _dispositionFileRepository.GetDispositionOffers(dispositionFileId);
        }

        public PimsDispositionOffer GetDispositionOfferById(long dispositionFileId, long dispositionOfferId)
        {
            _logger.LogInformation("Getting disposition file offers with Id: {id}", dispositionOfferId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            var dispositionFileParent = _dispositionFileRepository.GetById(dispositionFileId);
            if (dispositionFileParent is null
                || (dispositionFileParent is not null && !dispositionFileParent.PimsDispositionOffers.Any(x => x.DispositionOfferId.Equals(dispositionOfferId))))
            {
                throw new BadRequestException("Invalid dispositionFileId.");
            }

            return _dispositionFileRepository.GetDispositionOfferById(dispositionFileId, dispositionOfferId);
        }

        public PimsDispositionOffer AddDispositionFileOffer(long dispositionFileId, PimsDispositionOffer dispositionOffer)
        {
            _logger.LogInformation("Getting disposition file offers with DispositionFileId: {id}", dispositionFileId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionEdit);

            var dispositionFileParent = _dispositionFileRepository.GetById(dispositionFileId);
            if (dispositionFileId != dispositionOffer.DispositionFileId || dispositionFileParent is null)
            {
                throw new BadRequestException("Invalid dispositionFileId.");
            }

            ValidateDispositionOfferStatus(dispositionFileParent, dispositionOffer);

            var newOffer = _dispositionFileRepository.AddDispositionOffer(dispositionOffer);
            _dispositionFileRepository.CommitTransaction();

            return newOffer;
        }

        public PimsDispositionOffer UpdateDispositionFileOffer(long dispositionFileId, long offerId, PimsDispositionOffer dispositionOffer)
        {
            _logger.LogInformation("Getting disposition file offers with DispositionFileId: {id}", dispositionFileId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionEdit);

            var dispositionFileParent = _dispositionFileRepository.GetById(dispositionFileId);
            if (dispositionFileId != dispositionOffer.DispositionFileId || dispositionOffer.DispositionOfferId != offerId || dispositionFileParent is null)
            {
                throw new BadRequestException("Invalid dispositionFileId.");
            }

            ValidateDispositionOfferStatus(dispositionFileParent, dispositionOffer);

            var updatedOffer = _dispositionFileRepository.UpdateDispositionOffer(dispositionOffer);
            _dispositionFileRepository.CommitTransaction();

            return updatedOffer;
        }

        public PimsDispositionSale GetDispositionFileSale(long dispositionFileId)
        {
            _logger.LogInformation("Getting disposition file sales with DispositionFileId: {id}", dispositionFileId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            return _dispositionFileRepository.GetDispositionFileSale(dispositionFileId);
        }

        private static void ValidateDispositionOfferStatus(PimsDispositionFile dispositionFile, PimsDispositionOffer newOffer)
        {
            bool offerAlreadyAccepted = dispositionFile.PimsDispositionOffers.Any(x => x.DispositionOfferStatusTypeCode == EnumDispositionOfferStatusTypeCode.ACCCEPTED.ToString() && x.DispositionOfferId != newOffer.DispositionOfferId);
            if (offerAlreadyAccepted && !string.IsNullOrEmpty(newOffer.DispositionOfferStatusTypeCode) && newOffer.DispositionOfferStatusTypeCode == EnumDispositionOfferStatusTypeCode.ACCCEPTED.ToString())
            {
                throw new DuplicateEntityException("Invalid Disposition Offer, an Offer has been already accepted for this Disposition File");
            }
        }

        private void ReprojectPropertyLocationsToWgs84(IEnumerable<PimsDispositionFileProperty> dispositionPropertyFiles)
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

        private void MatchProperties(PimsDispositionFile dispositionFile, IEnumerable<UserOverrideCode> overrideCodes)
        {
            foreach (var dispProperty in dispositionFile.PimsDispositionFileProperties)
            {
                if (dispProperty.Property.Pid.HasValue)
                {
                    var pid = dispProperty.Property.Pid.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPid(pid);
                        dispProperty.PropertyId = foundProperty.Internal_Id;
                        _propertyService.UpdateLocation(dispProperty.Property, ref foundProperty, overrideCodes);
                        dispProperty.Property = null;
                    }
                    catch (KeyNotFoundException)
                    {
                        if (overrideCodes.Contains(UserOverrideCode.DisposingPropertyNotInventoried))
                        {
                            _logger.LogDebug("Adding new property with pid:{pid}", pid);
                            dispProperty.Property = _propertyService.PopulateNewProperty(dispProperty.Property);
                        }
                        else
                        {
                            throw new UserOverrideException(UserOverrideCode.DisposingPropertyNotInventoried, "You have added one or more properties to the disposition file that are not in the MoTI Inventory. Do you want to proceed?");
                        }
                    }
                }
                else if (dispProperty.Property.Pin.HasValue)
                {
                    var pin = dispProperty.Property.Pin.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPin(pin);
                        dispProperty.PropertyId = foundProperty.Internal_Id;
                        _propertyService.UpdateLocation(dispProperty.Property, ref foundProperty, overrideCodes);
                        dispProperty.Property = null;
                    }
                    catch (KeyNotFoundException)
                    {
                        if (overrideCodes.Contains(UserOverrideCode.DisposingPropertyNotInventoried))
                        {
                            _logger.LogDebug("Adding new property with pin:{pin}", pin);
                            dispProperty.Property = _propertyService.PopulateNewProperty(dispProperty.Property);
                        }
                        else
                        {
                            throw new UserOverrideException(UserOverrideCode.DisposingPropertyNotInventoried, "You have added one or more properties to the disposition file that are not in the MoTI Inventory. Do you want to proceed?");
                        }
                    }
                }
                else
                {
                    if (overrideCodes.Contains(UserOverrideCode.DisposingPropertyNotInventoried))
                    {
                        _logger.LogDebug("Adding new property without a pid");
                        dispProperty.Property = _propertyService.PopulateNewProperty(dispProperty.Property);
                    }
                    else
                    {
                        throw new UserOverrideException(UserOverrideCode.DisposingPropertyNotInventoried, "You have added one or more properties to the disposition file that are not in the MoTI Inventory. Do you want to proceed?");
                    }
                }
            }
        }
    }
}
