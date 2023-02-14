using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "research")]
    [ExcludeFromCodeCoverage]
    public class ResearchFilePropertyRepositoryTest
    {
        #region Data
        #endregion

        #region Tests
        #region Delete
        [Fact]
        public void Delete_PropertyResearchFile()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileView);
            var pimsPropertyResearchFile = new PimsPropertyResearchFile() { Property = new PimsProperty() { RegionCode = 1 } };
            pimsPropertyResearchFile.PimsPrfPropResearchPurposeTypes = new List<PimsPrfPropResearchPurposeType>() { new PimsPrfPropResearchPurposeType() { } };

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(pimsPropertyResearchFile);
            context.ChangeTracker.Clear();

            var repository = helper.CreateRepository<ResearchFilePropertyRepository>(user);

            // Act
            repository.Delete(pimsPropertyResearchFile);

            // Assert
            var result = repository.GetAllByResearchFileId(pimsPropertyResearchFile.PropertyResearchFileId);
            result.Should().BeEmpty();
        }

        #endregion

        #endregion
    }
}
