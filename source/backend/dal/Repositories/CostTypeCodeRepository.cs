using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Constants;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with financial codes within the datasource.
    /// </summary>
    public class CostTypeCodeRepository : IdentityBaseRepository<PimsCostTypeCode>, ICostTypeCodeRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a CostTypeCodeRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public CostTypeCodeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<CostTypeCodeRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves all cost type codes.
        /// </summary>
        /// <returns></returns>
        public IList<PimsCostTypeCode> GetAllCostTypeCodes()
        {
            return this.Context.PimsCostTypeCodes.AsNoTracking()
                .ToList();
        }

        public PimsCostTypeCode Add(PimsCostTypeCode pimsCode)
        {
            pimsCode.ThrowIfNull(nameof(pimsCode));

            // Check for uniqueness on save. At any given point an active code should be unique per code type.
            var existingCodes = Context.PimsCostTypeCodes.Where(
                    c => EF.Functions.Collate(c.Code, SqlCollation.LATIN_GENERAL_CASE_INSENSITIVE) == pimsCode.Code)
                .ToList();

            var now = DateTime.UtcNow;
            var newCodeIsActive = !pimsCode.ExpiryDate.HasValue || pimsCode.ExpiryDate?.Date > now.Date;

            // Active codes have no expiry date or they expire in the future.
            var isDuplicate = existingCodes.Any(c => !c.ExpiryDate.HasValue || c.ExpiryDate?.Date > now.Date);
            if (newCodeIsActive && isDuplicate)
            {
                throw new DuplicateEntityException("Duplicate cost type code found");
            }

            Context.PimsCostTypeCodes.Add(pimsCode);
            return pimsCode;
        }

        public PimsCostTypeCode Update(PimsCostTypeCode pimsCode)
        {
            throw new NotImplementedException();
        }

        public bool IsDuplicate(PimsCostTypeCode pimsCode)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
