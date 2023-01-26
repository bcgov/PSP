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
    public class FinancialActivityCodeRepository : IdentityBaseRepository<PimsFinancialActivityCode>, IFinancialActivityCodeRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a FinancialActivityCodeRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public FinancialActivityCodeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<FinancialActivityCodeRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves all financial activity codes.
        /// </summary>
        /// <returns></returns>
        public IList<PimsFinancialActivityCode> GetAllFinancialActivityCodes()
        {
            return this.Context.PimsFinancialActivityCodes.AsNoTracking()
                .ToList();
        }

        public PimsFinancialActivityCode Add(PimsFinancialActivityCode pimsCode)
        {
            pimsCode.ThrowIfNull(nameof(pimsCode));

            // Check for uniqueness on save. At any given point an active code should be unique per code type.
            var existingCodes = Context.PimsFinancialActivityCodes.Where(
                    c => EF.Functions.Collate(c.Code, SqlCollation.LATIN_GENERAL_CASE_INSENSITIVE) == pimsCode.Code)
                .ToList();

            var now = DateTime.UtcNow;
            var newCodeIsActive = !pimsCode.ExpiryDate.HasValue || pimsCode.ExpiryDate?.Date > now.Date;

            // Active codes have no expiry date or they expire in the future.
            var isDuplicate = existingCodes.Any(c => !c.ExpiryDate.HasValue || c.ExpiryDate?.Date > now.Date);
            if (newCodeIsActive && isDuplicate)
            {
                throw new DuplicateEntityException("Duplicate financial activity code found");
            }

            Context.PimsFinancialActivityCodes.Add(pimsCode);
            return pimsCode;
        }

        public PimsFinancialActivityCode Update(PimsFinancialActivityCode pimsCode)
        {
            throw new NotImplementedException();
        }

        public bool IsDuplicate(PimsFinancialActivityCode pimsCode)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
