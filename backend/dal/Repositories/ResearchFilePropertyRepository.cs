using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with research file properties within the datasource.
    /// </summary>
    public class ResearchFilePropertyRepository : BaseRepository<PimsPropertyResearchFile>, IResearchFilePropertyRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ResearchFilePropertyRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public ResearchFilePropertyRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<ResearchFilePropertyRepository> logger, IMapper mapper)
            : base(dbContext, user, service, logger, mapper) { }

        #endregion

        #region Methods

        public List<PimsPropertyResearchFile> GetByResearchFileId(long researchFileId)
        {
            return Context.PimsPropertyResearchFiles
                .Where(x => x.ResearchFileId == researchFileId)
                .Include(rp => rp.Property)
                .AsNoTracking()
                .ToList();
        }

        public int GetResearchFilePropertyRelatedCount(long propertyId)
        {
            return Context.PimsPropertyResearchFiles
                .Where(x => x.PropertyId == propertyId)
                .AsNoTracking()
                .Count();
        }

        public PimsPropertyResearchFile Add(PimsPropertyResearchFile propertyResearchFile)
        {
            // Mark the property not to be changed if it did not exist already.
            if (propertyResearchFile.PropertyId != 0)
            {
                Context.Entry(propertyResearchFile.Property).State = EntityState.Unchanged;
            }

            Context.PimsPropertyResearchFiles.Add(propertyResearchFile);
            return propertyResearchFile;
        }

        public void Delete(PimsPropertyResearchFile propertyResearchFile)
        {
            // Mark the property not to be changed if it did not exist already.
            if (propertyResearchFile.Property != null)
            {
                Context.Entry(propertyResearchFile.Property).State = EntityState.Unchanged;
            }

            Context.PimsPropertyResearchFiles.Remove(propertyResearchFile);
        }

        public PimsPropertyResearchFile Update(PimsPropertyResearchFile propertyResearchFile)
        {
            // Mark the property not to be changed it was being tracked.
            if (propertyResearchFile.Property != null)
            {
                Context.Entry(propertyResearchFile.Property).State = EntityState.Unchanged;
            }

            // Retrieve the existing property research purpose types for the property
            // Note: This is needed given the research file properties purpose types may not have the corresponging id, but corresponding code.
            var currentTypes = Context.PimsPropertyResearchFiles
                .SelectMany(x => x.PimsPrfPropResearchPurposeTypes)
                .Where(x => x.PropertyResearchFileId == propertyResearchFile.Id)
                .AsNoTracking()
                .ToList();

            List<PimsPrfPropResearchPurposeType> propertyPurposeTypes = new List<PimsPrfPropResearchPurposeType>();
            foreach (var selectedType in propertyResearchFile.PimsPrfPropResearchPurposeTypes)
            {
                var currentType = currentTypes.FirstOrDefault(x => x.PropResearchPurposeTypeCode == selectedType.PropResearchPurposeTypeCode);

                // If the code is already on the list, add the existing one, otherwise add the incomming one
                if (currentType != null)
                {
                    propertyPurposeTypes.Add(currentType);
                    Context.Entry(currentType).State = EntityState.Unchanged;
                }
                else
                {
                    propertyPurposeTypes.Add(selectedType);
                    Context.Entry(selectedType).State = EntityState.Added;
                }
            }

            // The ones not on the new set should be deleted
            List<PimsPrfPropResearchPurposeType> differenceSet = currentTypes.Where(x => !propertyPurposeTypes.Any(y => y.PropResearchPurposeTypeCode == x.PropResearchPurposeTypeCode)).ToList();
            foreach (var deletedType in differenceSet)
            {
                propertyPurposeTypes.Add(deletedType);
                Context.Entry(deletedType).State = EntityState.Deleted;
            }

            propertyResearchFile.PimsPrfPropResearchPurposeTypes = propertyPurposeTypes;

            this.Context.PimsPropertyResearchFiles.Update(propertyResearchFile);
            return propertyResearchFile;
        }

        #endregion
    }
}
