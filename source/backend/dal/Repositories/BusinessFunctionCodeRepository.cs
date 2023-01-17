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
    public class BusinessFunctionCodeRepository : BaseRepository<PimsBusinessFunctionCode>, IBusinessFunctionCodeRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a BusinessFunctionCodeRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public BusinessFunctionCodeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<BusinessFunctionCodeRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves all business function codes.
        /// </summary>
        /// <returns></returns>
        public IList<PimsBusinessFunctionCode> GetAllBusinessFunctionCodes()
        {
            return this.Context.PimsBusinessFunctionCodes.AsNoTracking()
                .ToList();
        }

        public PimsBusinessFunctionCode Add(PimsBusinessFunctionCode pimsCode)
        {
            pimsCode.ThrowIfNull(nameof(pimsCode));

            // Check for uniqueness on save. At any given point an active code should be unique per code type.
            var existingCodes = Context.PimsBusinessFunctionCodes.Where(
                    c => EF.Functions.Collate(c.Code, SqlCollation.LATIN_GENERAL_CASE_INSENSITIVE) == pimsCode.Code)
                .ToList();

            // Active codes have no expiry date or they expire in the future.
            var now = DateTime.UtcNow;
            var isDuplicate = existingCodes.Any(c => !c.ExpiryDate.HasValue || c.ExpiryDate?.Date > now.Date);
            if (isDuplicate)
            {
                throw new DuplicateEntityException("Duplicate business function code found");
            }

            Context.PimsBusinessFunctionCodes.Add(pimsCode);
            return pimsCode;
        }

        #endregion
    }
}
