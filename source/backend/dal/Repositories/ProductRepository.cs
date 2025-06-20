using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
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
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates a new instance of a ProductRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ProductRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ProductRepository> logger, IMapper mapper)
            : base(dbContext, user, logger)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves the products for the project with the given id.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IList<PimsProduct> GetByProject(long projectId)
        {
            return this.Context.PimsProducts.AsNoTracking()
                .Where(p => p.PimsProjectProducts.All(x => x.ProjectId == projectId))
                .Include(p => p.PimsAcquisitionFiles)
                .OrderBy(p => p.Code)
                .ToArray();
        }

        /// <summary>
        /// Retrieves the products for the project with the given id.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IList<PimsProjectProduct> GetProjectProductsByProject(long projectId)
        {
            return this.Context.PimsProjectProducts.AsNoTracking()
                .Where(p => p.ProjectId == projectId)
                .Include(p => p.Product)
                .ToList();
        }

        /// <summary>
        /// Using a list of products, find a matching list of products with the same code and description.
        /// </summary>
        /// <param name="incomingProducts"></param>
        /// <returns></returns>
        public IEnumerable<PimsProduct> GetProducts(IEnumerable<PimsProduct> incomingProducts)
        {
            var incomingCodes = incomingProducts.Select(ip => ip.Code);
            return this.Context.PimsProducts.AsNoTracking().Where(p => incomingCodes.Contains(p.Code)).ToArray();
        }

        /// <summary>
        /// Using a list of products, find a matching list of products with the same code and description.
        /// Ignore any products that are being "replaced" in the project referred to by the passed projectId, as those products will be removed and therefore cannot be matches to the incoming products.
        /// </summary>
        /// <param name="incomingProducts"></param>
        /// <returns></returns>
        public IEnumerable<PimsProduct> GetByProductBatch(IEnumerable<PimsProduct> incomingProducts, long projectId)
        {
            var matchingProductCodes = this.GetProducts(incomingProducts);
            var ignoreProductCodeIds = GetByProject(projectId).Where(p => !incomingProducts.Any(ip => p.Id == ip.Id)).Select(p => p.Id); // These codes are being removed, so should not be treated as duplicates.
            return matchingProductCodes.Where(mc => incomingProducts.Any(ip => ip.Id != mc.Id && ip.Description == mc.Description && ip.Code == mc.Code) && !ignoreProductCodeIds.Contains(mc.Id));
        }

        public PimsProduct GetProductAtTime(long productId, DateTime time)
        {
            var productHist = Context
                .PimsProductHists.AsNoTracking()
                .Where(pacr => pacr.Id == productId)
                .Where(pacr => pacr.EffectiveDateHist <= time
                    && (pacr.EndDateHist == null || pacr.EndDateHist > time))
                .GroupBy(pacr => pacr.Id)
                .Select(gpacr => gpacr.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault())
                .FirstOrDefault();

            return _mapper.Map<PimsProduct>(productHist);
        }
    }
}
