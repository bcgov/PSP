using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with acquisition file forms within the datasource.
    /// </summary>
    public class AcquisitionFileFormRepository : BaseRepository<PimsAcquisitionFileForm>, IAcquisitionFileFormRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionFileFormRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public AcquisitionFileFormRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<AcquisitionFileFormRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the form with the requested id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PimsAcquisitionFileForm GetById(long id)
        {
            return this.Context.PimsAcquisitionFileForms
                .Include(r => r.FormTypeCodeNavigation)
                .AsNoTracking()
                .FirstOrDefault(x => x.AcquisitionFileFormId == id) ?? throw new KeyNotFoundException();
        }

        public PimsAcquisitionFileForm Add(PimsAcquisitionFileForm fileForm)
        {
            fileForm.ThrowIfNull(nameof(fileForm));
            if (fileForm.FormTypeCodeNavigation != null)
            {
                Context.Entry(fileForm.FormTypeCodeNavigation).State = EntityState.Unchanged;
            }
            this.Context.PimsAcquisitionFileForms.Add(fileForm);
            return fileForm;
        }

        public IEnumerable<PimsAcquisitionFileForm> GetAllByAcquisitionFileId(long acquisitionFileId)
        {
            return Context.PimsAcquisitionFileForms.Include(af => af.FormTypeCodeNavigation).AsNoTracking().Where(af => af.AcquisitionFileId == acquisitionFileId);
        }

        public PimsAcquisitionFileForm GetByAcquisitionFileFormId(long acquisitionFileFormId)
        {
            return Context.PimsAcquisitionFileForms.Include(af => af.FormTypeCodeNavigation).AsNoTracking().FirstOrDefault(af => af.AcquisitionFileFormId == acquisitionFileFormId)
                ?? throw new KeyNotFoundException($"Failed to find acquisition file form with id ${acquisitionFileFormId}");
        }

        public bool TryDelete(long acquisitionFileFormId)
        {
            var deletedEntity = Context.PimsAcquisitionFileForms.FirstOrDefault(af => af.AcquisitionFileFormId == acquisitionFileFormId);
            if (deletedEntity != null)
            {
                Context.Remove(deletedEntity);
                return true;
            }
            return false;
        }

        #endregion
    }
}
