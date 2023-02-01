using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with products within the datasource.
    /// </summary>
    public class ProductRepository : BaseRepository<PimsProduct>, IProductRepository
    {
        /// <summary>
        /// Creates a new instance of a ProductRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ProductRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ProductRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        /// <summary>
        /// Retrieves the products for the project with the given id.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IList<PimsProduct> GetByProject(int projectId)
        {
            return this.Context.PimsProducts.AsNoTracking()
                .Where(p => p.ParentProjectId == projectId)
                .Include(p => p.PimsAcquisitionFiles)
                .OrderBy(p => p.Code)
                .ToArray();
        }
    }
}
