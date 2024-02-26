
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with property operations within the datasource.
    /// </summary>
    public class PropertyOperationRepository : BaseRepository<PimsPropertyOperation>, IPropertyOperationRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a NoteRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public PropertyOperationRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<PropertyOperationRepository> logger)
            : base(dbContext, user, logger)
        {
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
                .Where(x => x.PropertyOperationNo == operationNumber).ToList();
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
            return this.Context.PimsNotes.Count();
        }
        #endregion
    }
}
