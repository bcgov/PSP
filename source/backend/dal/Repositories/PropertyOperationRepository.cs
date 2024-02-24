using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// PropertyOperationRepository class, provides a service layer to interact with properties within the datasource.
    /// </summary>
    public class PropertyOperationRepository : BaseRepository<PimsProperty>, IPropertyOperationRepository
    {
        #region Constructors

        private const string PROPERTYOPERATIONNOSEQUENCE = "dbo.PIMS_PROPERTY_OPERATION_NO_SEQ";

        private readonly ISequenceRepository _sequenceRepository;

        /// <summary>
        /// Creates a new instance of a PropertyOperationRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public PropertyOperationRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<PropertyOperationRepository> logger, ISequenceRepository repository)
            : base(dbContext, user, logger)
        {
            _sequenceRepository = repository;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Add the new property operations to Context.
        /// </summary>
        /// <param name="operations"></param>
        /// <returns></returns>
        public IEnumerable<PimsPropertyOperation> AddRange(IEnumerable<PimsPropertyOperation> operations)
        {
            using var scope = Logger.QueryScope();
            operations.ThrowIfNull(nameof(operations));

            Context.PimsPropertyOperations.AddRange(operations);

            long operationNo = _sequenceRepository.GetNextSequenceValue(PROPERTYOPERATIONNOSEQUENCE);
            DateTime dateTime = DateTime.UtcNow;

            foreach (var operation in operations)
            {
                operation.PropertyOperationNo = operationNo;
                operation.OperationDt = dateTime;
            }
            return operations;
        }

        PimsPropertyOperation IRepository<PimsPropertyOperation>.Find(params object[] keyValues)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
