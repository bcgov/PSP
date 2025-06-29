using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoreLinq;
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
        /// <param name="logger"></param>
        public ResearchFilePropertyRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ResearchFilePropertyRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        public List<PimsPropertyResearchFile> GetAllByResearchFileId(long researchFileId)
        {
            return Context.PimsPropertyResearchFiles.AsNoTracking()

                .Include(rp => rp.PimsPrfPropResearchPurposeTyps)
                    .ThenInclude(rp => rp.PropResearchPurposeTypeCodeNavigation)
                .Include(rp => rp.Property)
                    .ThenInclude(rp => rp.RegionCodeNavigation)
                .Include(rp => rp.Property)
                    .ThenInclude(rp => rp.DistrictCodeNavigation)
                .Include(rp => rp.Property)
                    .ThenInclude(rp => rp.Address)
                .Where(rp => rp.ResearchFileId == researchFileId)
                .OrderBy(rp => rp.DisplayOrder)
                .ToList();
        }

        public PimsPropertyResearchFile Add(PimsPropertyResearchFile propertyResearchFile)
        {
            // Mark the property not to be changed if it did not exist already.
            if (propertyResearchFile.PropertyId != 0)
            {
                propertyResearchFile.Property = null;
            }

            Context.PimsPropertyResearchFiles.Add(propertyResearchFile);
            return propertyResearchFile;
        }

        public void Delete(PimsPropertyResearchFile propertyResearchFile)
        {
            var existingPropertyResearchFile = Context.PimsPropertyResearchFiles.AsNoTracking()
                .Where(x => x.PropertyResearchFileId == propertyResearchFile.Internal_Id)
                .Include(rp => rp.Property)
                .Include(rp => rp.PimsPrfPropResearchPurposeTyps)
                .FirstOrDefault() ?? throw new KeyNotFoundException();

            // Delete any Property research purpose type associations
            existingPropertyResearchFile.PimsPrfPropResearchPurposeTyps.ForEach(purposeType => Context.PimsPrfPropResearchPurposeTyps.Remove(new PimsPrfPropResearchPurposeTyp() { Internal_Id = purposeType.Internal_Id }));

            Context.Remove(new PimsPropertyResearchFile() { Internal_Id = propertyResearchFile.Internal_Id });
        }

        public PimsPropertyResearchFile Update(PimsPropertyResearchFile propertyResearchFile)
        {
            // Do not allow this method to make any updates to the related property entity.
            propertyResearchFile.Property = null;

            // Retrieve the existing property research purpose types for the property
            // Note: This is needed given the research file properties purpose types may not have the corresponging id, but corresponding code.
            var currentTypes = Context.PimsPropertyResearchFiles
                .SelectMany(x => x.PimsPrfPropResearchPurposeTyps)
                .Where(x => x.PropertyResearchFileId == propertyResearchFile.Internal_Id)
                .AsNoTracking()
                .ToList();

            List<PimsPrfPropResearchPurposeTyp> propertyPurposeTypes = new List<PimsPrfPropResearchPurposeTyp>();
            foreach (var selectedType in propertyResearchFile.PimsPrfPropResearchPurposeTyps)
            {
                var currentType = currentTypes.FirstOrDefault(x => x.PropResearchPurposeTypeCode == selectedType.PropResearchPurposeTypeCode);

                // If the code is already on the list, add the existing one, otherwise add the incoming one
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
            List<PimsPrfPropResearchPurposeTyp> differenceSet = currentTypes.Where(x => !propertyPurposeTypes.Any(y => y.PropResearchPurposeTypeCode == x.PropResearchPurposeTypeCode)).ToList();
            foreach (var deletedType in differenceSet)
            {
                propertyPurposeTypes.Add(deletedType);
                Context.Entry(deletedType).State = EntityState.Deleted;
            }

            propertyResearchFile.PimsPrfPropResearchPurposeTyps = propertyPurposeTypes;

            this.Context.PimsPropertyResearchFiles.Update(propertyResearchFile);
            return propertyResearchFile;
        }

        #endregion
    }
}
