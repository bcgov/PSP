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

        #region GetBy_Id
        [Fact]
        public void GetById_Success()
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

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(eResearch);
            var repository = helper.CreateRepository<ResearchFileRepository>(user);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsResearchFile>();
            result.Name.Should().Be("research file name");
            result.RfileNumber.Should().Be(eResearch.RfileNumber);
        }

        [Fact]
        public void GetById_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<ResearchFileRepository>(user);

            // Act
            Action act = () => repository.GetById(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region Add
        [Fact(Skip = "Seq execution error")]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileAdd);
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
            var result = repository.Add(eResearch);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsResearchFile>();
            result.Name.Should().Be("research file name");
            result.RfileNumber.Should().Be("100-000-000");
        }

        [Fact]
        public void Add_ThrowIf_Null()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileView);
            var repository = helper.CreateRepository<ResearchFileRepository>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                repository.Add(null));
        }

        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileEdit);

            var eResearch = EntityHelper.CreateResearchFile(rfileNumber: "100-000-000");
            eResearch.RoadAlias = "a road name or alias";
            eResearch.RoadName = "a road name or alias";
            eResearch.Name = "research file name";
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(eResearch);
            var pimsResearchFilePurpose = new PimsResearchFilePurpose() { ResearchFileId = 1, ResearchFilePurposeId = 1, ResearchPurposeTypeCode = "CODE" };
            var purposes = new List<PimsResearchFilePurpose>();
            purposes.Add(pimsResearchFilePurpose);
            var repository = helper.CreateRepository<ResearchFileRepository>(user);

            // Act
            eResearch.RoadAlias = "a road name or alias updated";
            eResearch.PimsResearchFilePurposes = purposes;
            var result = repository.Update(eResearch);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsResearchFile>();
            result.PimsResearchFilePurposes.Should().NotBeNull();
            result.PimsResearchFilePurposes.Count.Should().Be(1);
            result.RoadAlias.Should().Be("a road name or alias updated");
        }

        [Fact]
        public void Update_Success_With_Purpose_Delete()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileEdit);
            var pimsResearchFilePurpose = new PimsResearchFilePurpose() { ResearchFileId = 1, ResearchFilePurposeId = 1, ResearchPurposeTypeCode = "CODE" };
            var eResearch = EntityHelper.CreateResearchFile(rfileNumber: "100-000-000");
            eResearch.RoadAlias = "a road name or alias";
            eResearch.RoadName = "a road name or alias";
            eResearch.Name = "research file name";
            var purposes = new List<PimsResearchFilePurpose>();
            purposes.Add(pimsResearchFilePurpose);
            eResearch.PimsResearchFilePurposes = purposes;
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(eResearch);

            var repository = helper.CreateRepository<ResearchFileRepository>(user);

            // Act
            eResearch.RoadAlias = "a road name or alias updated";
            var newPimsResearchFilePurpose = new PimsResearchFilePurpose() { ResearchFileId = 1, ResearchFilePurposeId = 2, ResearchPurposeTypeCode = "CODE2" };
            var newPurposes = new List<PimsResearchFilePurpose>();
            newPurposes.Add(newPimsResearchFilePurpose);
            eResearch.PimsResearchFilePurposes = newPurposes;
            context.ChangeTracker.Clear();
            var result = repository.Update(eResearch);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsResearchFile>();
            result.PimsResearchFilePurposes.Should().NotBeNull();
            result.PimsResearchFilePurposes.Count.Should().Be(1);
            result.PimsResearchFilePurposes.Where(x => x.ResearchPurposeTypeCode == "CODE2").Count().Should().Be(1);
            result.RoadAlias.Should().Be("a road name or alias updated");
        }

        [Fact]
        public void Update_Null()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileEdit);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<ResearchFileRepository>(user);

            // Act
            Action act = () => repository.Update(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion
        #endregion
    }
}
