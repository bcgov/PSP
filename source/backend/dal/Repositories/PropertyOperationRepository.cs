using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
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
        /// Retrieves the property operations for the given operation number.
        /// </summary>
        /// <param name="operationNumber"></param>
        /// <returns></returns>
        public IList<PimsPropertyOperation> GetByOperationNumber(long operationNumber)
        {
            return this.Context.PimsPropertyOperations.AsNoTracking()
                .Where(x => x.PropertyOperationNo == operationNumber)
                .Include(op => op.PropertyOperationTypeCodeNavigation)
                .ToList();
        }

        /// <summary>
        /// Retrieves the property operations for the given property id.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public IList<PimsPropertyOperation> GetByPropertyId(long propertyId)
        {
            var operations = this.Context.PimsPropertyOperations.AsNoTracking()
                .Where(x => x.SourcePropertyId == propertyId || x.DestinationPropertyId == propertyId).Select(x => x.PropertyOperationNo).Distinct().ToList();

            var operationResults = new List<PimsPropertyOperation>();
            if (operations == null)
            {
                return operationResults;
            }

            foreach (var operation in operations)
            {
                operationResults.AddRange(GetByOperationNumber(operation));
            }

            return operationResults;
        }

        /// <summary>
        /// Retrieves the row version of the property operation with the specified id.
        /// </summary>
        /// <param name="id">The operation id.</param>
        /// <returns>The row version.</returns>
        public long GetRowVersion(long id)
        {
            return this.Context.PimsPropertyOperations.AsNoTracking()
                .Where(n => n.PropertyOperationId == id)?
                .Select(n => n.ConcurrencyControlNumber)?
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        public int Count()
        {
            return this.Context.PimsPropertyOperations.Count();
        }

        /// <summary>
        /// Add the new property operations to Context.
        /// </summary>
        /// <param name="operations"></param>
        /// <returns></returns>
        public IEnumerable<PimsPropertyOperation> AddRange(IEnumerable<PimsPropertyOperation> operations)
        {
            using var scope = Logger.QueryScope();
            operations.ThrowIfNull(nameof(operations));

            long operationNo = _sequenceRepository.GetNextSequenceValue(PROPERTYOPERATIONNOSEQUENCE);
            DateTime dateTime = DateTime.UtcNow;

            foreach (var operation in operations)
            {
                operation.PropertyOperationNo = operationNo;
                operation.OperationDt = dateTime;
            }

            Context.PimsPropertyOperations.AddRange(operations);
            return operations;
        }

        PimsPropertyOperation IRepository<PimsPropertyOperation>.Find(params object[] keyValues)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
