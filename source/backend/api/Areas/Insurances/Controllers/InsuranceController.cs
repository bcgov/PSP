using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Areas.Insurance.Models;
using Pims.Api.Policies;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Insurance.Controllers
{
    /// <summary>
    /// InsuranceController class, provides endpoints for interacting with insurances.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("insurances")]
    [Route("v{version:apiVersion}/leases/{leaseId}/[area]")]
    [Route("/leases/{leaseId}/[area]")]
    public class InsuranceController : ControllerBase
    {
        #region Variables
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a InsuranceController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="insuranceRepository"></param>
        /// <param name="mapper"></param>
        ///
        public InsuranceController(IInsuranceRepository insuranceRepository, IMapper mapper)
        {
            _insuranceRepository = insuranceRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Updates a list of insurances for a lease.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Api.Models.BatchUpdate.Reply<InsuranceModel>), 200)]
        [SwaggerOperation(Tags = new[] { "insurance" })]
        public IActionResult BatchUpdate(int leaseId, Api.Models.BatchUpdate.Request<InsuranceModel> batchUpdate)
        {
            IList<Dal.Entities.PimsInsurance> updatedEntities = new List<Dal.Entities.PimsInsurance>();
            foreach (Api.Models.BatchUpdate.EntryModification<InsuranceModel> modification in batchUpdate.Payload)
            {
                Dal.Entities.PimsInsurance insuranceEntity = _mapper.Map<Dal.Entities.PimsInsurance>(modification.Entry);
                insuranceEntity.LeaseId = leaseId;
                if (modification.Operation == Constants.UpdateType.Update)
                {
                    var insurance = _insuranceRepository.Update(insuranceEntity, false);
                    updatedEntities.Add(insurance);
                }
                else if (modification.Operation == Constants.UpdateType.Add)
                {
                    var insurance = _insuranceRepository.Add(insuranceEntity, false);
                    updatedEntities.Add(insurance);
                }
                else if (modification.Operation == Constants.UpdateType.Delete)
                {
                    var insurance = _insuranceRepository.Delete(insuranceEntity, false);
                    updatedEntities.Add(insurance);
                }
            }

            _insuranceRepository.CommitTransaction();

            var insuranceModels = _mapper.Map<IList<Models.InsuranceModel>>(updatedEntities);

            return new JsonResult(new Api.Models.BatchUpdate.Reply<InsuranceModel>(insuranceModels));
        }
        #endregion
    }
}
