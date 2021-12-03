using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    /// <summary>
    /// Provides a service layer to administrate persons within the datasource.
    /// </summary>
    public class PersonService : BaseService<PimsPerson>, IPersonService
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a PersonService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public PersonService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<PersonService> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
        #endregion

        #region Methods
        /// <summary>
        /// Get a page of persons from the datasource.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IEnumerable<PimsPerson> GetAll()
        {
            return this.Context.PimsPerson.AsNoTracking().ToArray();
        }

        /// <summary>
        /// Get the person for the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="KeyNotFoundException">Person does not exists for the specified 'id'.</exception>
        /// <returns></returns>
        public PimsPerson Get(long id)
        {
            return this.Context.PimsPerson
                .Include(p => p.PimsPersonAddresses)
                    .ThenInclude(pa => pa.Address)
                    .ThenInclude(a => a.Country)
                .Include(p => p.PimsPersonAddresses)
                    .ThenInclude(pa => pa.Address)
                    .ThenInclude(a => a.ProvinceState)
                .Include(p => p.PimsPersonAddresses)
                    .ThenInclude(pa => pa.AddressUsageTypeCodeNavigation)
                .Include(p => p.PimsPersonOrganizations)
                    .ThenInclude(o => o.Organization)
                .Include(p => p.PimsContactMethods)
                    .ThenInclude(cm => cm.ContactMethodTypeCodeNavigation)
                .Where(p => p.PersonId == id)
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }
        #endregion
    }
}
