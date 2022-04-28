using System.Diagnostics.CodeAnalysis;
using Moq;
using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "research")]
    [ExcludeFromCodeCoverage]
    public class ResearchServiceTest
    {

        #region Tests
        #region GetPage
        [Fact]
        public void GetPage()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileView);

            var researchFile = EntityHelper.CreateResearchFile(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(researchFile);

            var service = helper.Create<ResearchFileService>();
            var researchRepository = helper.GetService<Mock<Repositories.IResearchFileRepository>>();
            researchRepository.Setup(x => x.GetPage(It.IsAny<ResearchFilter>()));

            // Act
            var updatedLease = service.GetPage(new ResearchFilter());

            // Assert
            researchRepository.Verify(x => x.GetPage(It.IsAny<ResearchFilter>()), Times.Once);
        }

        [Fact]
        public void GetPage_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var researchFile = EntityHelper.CreateResearchFile(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(researchFile);

            var service = helper.Create<ResearchFileService>();
            var researchRepository = helper.GetService<Mock<Repositories.IResearchFileRepository>>();
            researchRepository.Setup(x => x.GetPage(It.IsAny<ResearchFilter>()));

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.GetPage(new ResearchFilter()));
            researchRepository.Verify(x => x.GetPage(It.IsAny<ResearchFilter>()), Times.Never);
        }
        
        #endregion
        #endregion
    }
}
