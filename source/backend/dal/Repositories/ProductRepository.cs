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
        public IList<PimsProduct> GetByProject(long projectId)
        {
            return this.Context.PimsProducts.AsNoTracking()
                .Where(p => p.ParentProjectId == projectId)
                .Include(p => p.PimsAcquisitionFiles)
                .OrderBy(p => p.Code)
                .ToArray();
        }

        /// <summary>
        /// Using a list of products, find a matching list of products with the same code and description.
        /// Ignore any products that are being "replaced" in the project referred to by the passed projectId, as those products will be removed and therefore cannot be matches to the incoming products.
        /// </summary>
        /// <param name="incomingProducts"></param>
        /// <returns></returns>
        public IEnumerable<PimsProduct> GetByProductBatch(IEnumerable<PimsProduct> incomingProducts, long projectId)
        {
            var incomingCodes = incomingProducts.Select(ip => ip.Code);
            var matchingProductCodes = this.Context.PimsProducts.AsNoTracking().Where(databaseProduct => incomingCodes.Contains(databaseProduct.Code)).ToArray();
            var ignoreProductCodeIds = GetByProject(projectId).Where(p => !incomingProducts.Any(ip => p.Id == ip.Id)).Select(p => p.Id); // These codes are being removed, so should not be treated as duplicates.
            return matchingProductCodes.Where(mc => incomingProducts.Any(ip => ip.Id != mc.Id && ip.Description == mc.Description && ip.Code == mc.Code) && !ignoreProductCodeIds.Contains(mc.Id));
        }
    }
}
