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
    public class YearlyFinancialCodeRepository : IdentityBaseRepository<PimsYearlyFinancialCode>, IYearlyFinancialCodeRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a YearlyFinancialCodeRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public YearlyFinancialCodeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<YearlyFinancialCodeRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves all yearly financial codes.
        /// </summary>
        /// <returns></returns>
        public IList<PimsYearlyFinancialCode> GetAllYearlyFinancialCodes()
        {
            return this.Context.PimsYearlyFinancialCodes.AsNoTracking()
                .ToList();
        }

        public PimsYearlyFinancialCode Add(PimsYearlyFinancialCode pimsCode)
        {
            pimsCode.ThrowIfNull(nameof(pimsCode));

            if (IsDuplicate(pimsCode))
            {
                throw new DuplicateEntityException("Duplicate yearly financial code found");
            }

            Context.PimsYearlyFinancialCodes.Add(pimsCode);
            return pimsCode;
        }

        public PimsYearlyFinancialCode Update(PimsYearlyFinancialCode pimsCode)
        {
            pimsCode.ThrowIfNull(nameof(pimsCode));

            var currentCode = Context.PimsYearlyFinancialCodes
                .FirstOrDefault(x => x.Id == pimsCode.Id) ?? throw new KeyNotFoundException();

            if (IsDuplicate(pimsCode))
            {
                throw new DuplicateEntityException("Duplicate yearly financial code found");
            }

            Context.Entry(currentCode).CurrentValues.SetValues(pimsCode);
            return pimsCode;
        }

        public bool IsDuplicate(PimsYearlyFinancialCode pimsCode)
        {
            // Check for uniqueness on save. At any given point an active code should be unique per code type.
            var existingCodes = Context.PimsYearlyFinancialCodes.Where(
                    c => EF.Functions.Collate(c.Code, SqlCollation.LATINGENERALCASEINSENSITIVE) == pimsCode.Code)
                .ToList();

            // Need to remove the entity from existing codes when updating it. We only want to compare against other entities.
            if (pimsCode.Id > 0)
            {
                existingCodes.RemoveAll(c => c.Id == pimsCode.Id);
            }

            var now = DateTime.UtcNow;
            var newCodeIsActive = !pimsCode.ExpiryDate.HasValue || pimsCode.ExpiryDate?.Date > now.Date;

            // Active codes have no expiry date or they expire in the future.
            var isDuplicate = existingCodes.Any(c => !c.ExpiryDate.HasValue || c.ExpiryDate?.Date > now.Date);
            return newCodeIsActive && isDuplicate;
        }
        #endregion
    }
}
