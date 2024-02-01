using System;
using System.Collections.Generic;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts.DispositionFile;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Disposition.Controllers
{
    /// <summary>
    /// DispositionFileController class, provides endpoints for interacting with disposition files.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("dispositionfiles")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class DispositionFileController : ControllerBase
    {
        #region Variables
        private readonly IDispositionFileService _dispositionService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a DispositionFileController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="dispositionService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public DispositionFileController(IDispositionFileService dispositionService, IMapper mapper, ILogger<DispositionFileController> logger)
        {
            _dispositionService = dispositionService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Gets the specified disposition file.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [HasPermission(Permissions.DispositionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DispositionFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDispositionFile(long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(GetDispositionFile),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            var dispositionFile = _dispositionService.GetById(id);
            return new JsonResult(_mapper.Map<DispositionFileModel>(dispositionFile));
        }

        /// <summary>
        /// Creates a new Disposition File entity.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(NullJsonResultFilter))]
        [HasPermission(Permissions.DispositionAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DispositionFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddDispositionFile([FromBody] DispositionFileModel model, [FromQuery] string[] userOverrideCodes)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(AddDispositionFile),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            var dispositionFileEntity = _mapper.Map<Dal.Entities.PimsDispositionFile>(model);
            var dispositionFile = _dispositionService.Add(dispositionFileEntity, userOverrideCodes.Select(oc => UserOverrideCode.Parse(oc)));

            return new JsonResult(_mapper.Map<DispositionFileModel>(dispositionFile));
        }

        [HttpPut("{id:long}")]
        [TypeFilter(typeof(NullJsonResultFilter))]
        [HasPermission(Permissions.DispositionEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DispositionFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateDispositionFile([FromRoute]long id, [FromBody] DispositionFileModel model, [FromQuery] string[] userOverrideCodes)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(UpdateDispositionFile),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            var dispositionFileEntity = _mapper.Map<Dal.Entities.PimsDispositionFile>(model);
            var dispositionFile = _dispositionService.Update(id, dispositionFileEntity, userOverrideCodes.Select(oc => UserOverrideCode.Parse(oc)));

            return new JsonResult(_mapper.Map<DispositionFileModel>(dispositionFile));
        }

        /// <summary>
        /// Gets the specified disposition file last updated-by information.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}/updateInfo")]
        [HasPermission(Permissions.DispositionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Dal.Entities.Models.LastUpdatedByModel), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetLastUpdatedBy(long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(GetLastUpdatedBy),
                User.GetUsername(),
                DateTime.Now);

            var lastUpdated = _dispositionService.GetLastUpdateInformation(id);
            return new JsonResult(lastUpdated);
        }

        /// <summary>
        /// Get the disposition file properties.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}/properties")]
        [HasPermission(Permissions.DispositionView, Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DispositionFilePropertyModel>), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDispositionFileProperties(long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(GetDispositionFileProperties),
                User.GetUsername(),
                DateTime.Now);

            var dispositionfileProperties = _dispositionService.GetProperties(id);

            return new JsonResult(_mapper.Map<IEnumerable<DispositionFilePropertyModel>>(dispositionfileProperties));
        }

        /// <summary>
        /// Get all unique persons and organizations that belong to at least one disposition file as a team member.
        /// </summary>
        /// <returns></returns>
        [HttpGet("team-members")]
        [HasPermission(Permissions.DispositionView)]
        [HasPermission(Permissions.ContactView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DispositionFileTeamModel>), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDispositionTeamMembers()
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(GetDispositionTeamMembers),
                User.GetUsername(),
                DateTime.Now);

            var team = _dispositionService.GetTeamMembers();

            return new JsonResult(_mapper.Map<IEnumerable<DispositionFileTeamModel>>(team));
        }

        [HttpGet("{id:long}/offers")]
        [HasPermission(Permissions.DispositionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DispositionFileOfferModel>), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDispositionFileOffers([FromRoute]long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(GetDispositionFileOffers),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            var dispositionOffers = _dispositionService.GetOffers(id);
            return new JsonResult(_mapper.Map<IEnumerable<DispositionFileOfferModel>>(dispositionOffers));
        }

        [HttpGet("{id:long}/offers/{offerId:long}")]
        [HasPermission(Permissions.DispositionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DispositionFileOfferModel), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDispositionFileOfferById([FromRoute]long id, [FromRoute]long offerId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(GetDispositionFileOfferById),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            var dispositionOffer = _dispositionService.GetDispositionOfferById(id, offerId);

            return new JsonResult(_mapper.Map<DispositionFileOfferModel>(dispositionOffer));
        }

        [HttpPost("{id:long}/offers")]
        [HasPermission(Permissions.DispositionEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DispositionFileOfferModel), 201)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddDispositionFileOffer([FromRoute]long id, [FromBody]DispositionFileOfferModel dispositionFileOffer)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(AddDispositionFileOffer),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            try
            {
                var dispositionOfferEntity = _mapper.Map<Dal.Entities.PimsDispositionOffer>(dispositionFileOffer);
                var newDispositionOffer = _dispositionService.AddDispositionFileOffer(id, dispositionOfferEntity);

                return new JsonResult(_mapper.Map<DispositionFileOfferModel>(newDispositionOffer));
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut("{id:long}/offers/{offerId:long}")]
        [HasPermission(Permissions.DispositionEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DispositionFileOfferModel), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateDispositionFileOffer([FromRoute]long id, [FromRoute]long offerId, [FromBody]DispositionFileOfferModel dispositionFileOffer)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(UpdateDispositionFileOffer),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            try
            {
                var dispositionOfferEntity = _mapper.Map<Dal.Entities.PimsDispositionOffer>(dispositionFileOffer);
                var updatedOffer = _dispositionService.UpdateDispositionFileOffer(id, offerId, dispositionOfferEntity);

                return new JsonResult(_mapper.Map<DispositionFileOfferModel>(updatedOffer));
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpDelete("{id:long}/offers/{offerId:long}")]
        [HasPermission(Permissions.DispositionEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeleteDispositionFileOffer([FromRoute] long id, [FromRoute] long offerId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(DeleteDispositionFileOffer),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            var result = _dispositionService.DeleteDispositionFileOffer(id, offerId);
            return new JsonResult(result);
        }

        [HttpGet("{id:long}/sale")]
        [HasPermission(Permissions.DispositionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DispositionFileSaleModel), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDispositionFileSales([FromRoute]long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(GetDispositionFileSales),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            var dispositionSale = _dispositionService.GetDispositionFileSale(id);
            return new JsonResult(_mapper.Map<DispositionFileSaleModel>(dispositionSale));
        }

        [HttpPost("{id:long}/sale")]
        [HasPermission(Permissions.DispositionEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DispositionFileSaleModel), 201)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddDispositionFileSale([FromRoute] long id, [FromBody] DispositionFileSaleModel dispositionFileSale)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(AddDispositionFileSale),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            try
            {
                if (id != dispositionFileSale.DispositionFileId)
                {
                    throw new BadRequestException("Invalid dispositionFileId.");
                }

                var dispositionSaleEntity = _mapper.Map<Dal.Entities.PimsDispositionSale>(dispositionFileSale);
                var newDispositionSale = _dispositionService.AddDispositionFileSale(dispositionSaleEntity);

                return new JsonResult(_mapper.Map<DispositionFileSaleModel>(newDispositionSale));
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut("{id:long}/sale/{saleId:long}")]
        [HasPermission(Permissions.DispositionEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DispositionFileSaleModel), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateDispositionFileSale([FromRoute]long id, [FromRoute]long saleId, [FromBody] DispositionFileSaleModel dispositionFileSale)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(UpdateDispositionFileSale),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            if (id != dispositionFileSale.DispositionFileId || dispositionFileSale.Id != saleId)
            {
                throw new BadRequestException("Invalid dispositionFileId.");
            }

            var dispositionSaleEntity = _mapper.Map<Dal.Entities.PimsDispositionSale>(dispositionFileSale);
            var updatedSale = _dispositionService.UpdateDispositionFileSale(dispositionSaleEntity);

            return new JsonResult(_mapper.Map<DispositionFileSaleModel>(updatedSale));
        }

        [HttpGet("{id:long}/appraisal")]
        [HasPermission(Permissions.DispositionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DispositionFileAppraisalModel), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDispositionFileAppraisal([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(GetDispositionFileAppraisal),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            var dispositionSale = _dispositionService.GetDispositionFileAppraisal(id);
            return new JsonResult(_mapper.Map<DispositionFileAppraisalModel>(dispositionSale));
        }

        [HttpPost("{id:long}/appraisal")]
        [HasPermission(Permissions.DispositionEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DispositionFileAppraisalModel), 201)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddDispositionFileAppraisal([FromRoute] long id, [FromBody] DispositionFileAppraisalModel dispositionFileAppraisal)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(AddDispositionFileAppraisal),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            try
            {
                var dispositionAppraisalEntity = _mapper.Map<Dal.Entities.PimsDispositionAppraisal>(dispositionFileAppraisal);
                var newDispositionAppraisal = _dispositionService.AddDispositionFileAppraisal(id, dispositionAppraisalEntity);

                return new JsonResult(_mapper.Map<DispositionFileAppraisalModel>(newDispositionAppraisal));
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut("{id:long}/appraisal/{appraisalId:long}")]
        [HasPermission(Permissions.DispositionEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DispositionFileAppraisalModel), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateDispositionFileAppraisal([FromRoute] long id, [FromRoute] long appraisalId, [FromBody] DispositionFileAppraisalModel dispositionFileAppraisal)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(UpdateDispositionFileAppraisal),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            var dispositionAppraisalEntity = _mapper.Map<Dal.Entities.PimsDispositionAppraisal>(dispositionFileAppraisal);
            var updatedOffer = _dispositionService.UpdateDispositionFileAppraisal(id, appraisalId, dispositionAppraisalEntity);

            return new JsonResult(_mapper.Map<DispositionFileAppraisalModel>(updatedOffer));
        }

        #endregion
    }
}
