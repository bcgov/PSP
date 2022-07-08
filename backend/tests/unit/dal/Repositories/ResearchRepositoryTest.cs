using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "research")]
    [ExcludeFromCodeCoverage]
    public class ResearchRepositoryTest
    {
        #region Data
        public static IEnumerable<object[]> ResearchFilterData =>
            new List<object[]>
            {
                new object[] { new ResearchFilter() { RegionCode = 1 }, 1 },
                new object[] { new ResearchFilter() { RegionCode = 2 }, 0 },
                new object[] { new ResearchFilter() { ResearchFileStatusTypeCode = "Active" }, 1 },
                new object[] { new ResearchFilter() { ResearchFileStatusTypeCode = "Fake" }, 0 },
                new object[] { new ResearchFilter() { Name = "research file name" }, 1 },
                new object[] { new ResearchFilter() { Name = "a fake name" }, 0 },
                new object[] { new ResearchFilter() { RFileNumber = "100-000-000" }, 1 },
                new object[] { new ResearchFilter() { RFileNumber = "123-456-789" }, 0 },
                new object[] { new ResearchFilter() { RoadOrAlias = "a road name or alias" }, 1 },
                new object[] { new ResearchFilter() { RoadOrAlias = "a non-existent alias" }, 0 },
                new object[] { new ResearchFilter() { CreatedOnStartDate = DateTime.Now.Date }, 1 },
                new object[] { new ResearchFilter() { CreatedOnStartDate = DateTime.Now.Date.AddDays(2) }, 0 },
                new object[] { new ResearchFilter() { UpdatedOnStartDate = DateTime.Now.Date }, 1 },
                new object[] { new ResearchFilter() { UpdatedOnStartDate = DateTime.Now.Date.AddDays(2) }, 0 },
                new object[] { new ResearchFilter() { CreatedOnEndDate = DateTime.UtcNow }, 1 },
                new object[] { new ResearchFilter() { CreatedOnEndDate = DateTime.Now.Date.AddDays(-2) }, 0 },
                new object[] { new ResearchFilter() { UpdatedOnEndDate = DateTime.UtcNow }, 1 },
                new object[] { new ResearchFilter() { UpdatedOnEndDate = DateTime.Now.Date.AddDays(-2) }, 0 },
                new object[] { new ResearchFilter() { AppCreateUserid = "service" }, 1 },
                new object[] { new ResearchFilter() { AppCreateUserid = "invalid" }, 0 },
                new object[] { new ResearchFilter() { AppLastUpdateUserid = "service" }, 1 },
                new object[] { new ResearchFilter() { AppLastUpdateUserid = "invalid" }, 0 },

            };
        #endregion

        #region Tests
        #region GetPage
        [Theory]
        [MemberData(nameof(ResearchFilterData))]
        public void Get_ResearchFiles_Paged(ResearchFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileView);
            var eResearch = EntityHelper.CreateResearchFile(rfileNumber: "100-000-000");
            eResearch.RoadAlias = "a road name or alias";
            eResearch.RoadName = "a road name or alias";
            eResearch.Name = "research file name";
            eResearch.ResearchFileStatusTypeCodeNavigation = new Entity.PimsResearchFileStatusType() { Id = "Active" };
            eResearch.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>() { new PimsPropertyResearchFile() { Property = new PimsProperty() { RegionCode = 1 } } };

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(eResearch);

            var repository = helper.CreateRepository<ResearchFileRepository>(user);

            // Act
            var result = repository.GetPage(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<PimsResearchFile>>(result);
            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public void Get_ResearchFiles_NullFilter()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileView);

            var service = helper.CreateRepository<ResearchFileRepository>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.GetPage(null));
        }

        [Fact]
        public void Get_ResearchFiles_InvalidFilter()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileView);

            var service = helper.CreateRepository<ResearchFileRepository>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() =>
                service.GetPage(new ResearchFilter() { Page = -1 }));
        }

        #endregion

        #endregion
    }
}
