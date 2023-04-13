using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with research files within the datasource.
    /// </summary>
    public class FormTypeRepository : BaseRepository<PimsFormType>, IFormTypeRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ResearchFileRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public FormTypeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ResearchFileRepository> logger, IMapper mapper)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        public IList<PimsFormType> GetAllFormTypes()
        {
            return this.Context.PimsFormTypes.AsNoTracking().ToList();
        }

        public PimsFormType GetByFormTypeCode(string formTypeCode)
        {
            return this.Context.PimsFormTypes.AsNoTracking()
                .Include(ft => ft.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(ft => ft.Document)
                    .ThenInclude(d => d.DocumentType)
                .FirstOrDefault(x => x.FormTypeCode == formTypeCode);
        }

        public PimsFormType SetFormTypeDocument(PimsFormType formType)
        {
            using var queryScope = Logger.QueryScope();

            var existingFormType = Context.PimsFormTypes
                .FirstOrDefault(x => x.FormTypeCode == formType.FormTypeCode) ?? throw new KeyNotFoundException();

            Context.Entry(existingFormType).CurrentValues.SetValues(formType);
            Context.Entry(existingFormType).State = EntityState.Modified;

            return existingFormType;
        }

        #endregion
    }
}
