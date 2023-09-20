using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Pims.Api.Areas.Projects.Controllers;
using Pims.Api.Areas.Projects.Models;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Xunit;
using ApiModel = Pims.Api.Areas.Projects.Models;
using Entity = Pims.Dal.Entities.Models;

namespace Pims.Api.Test.Controllers.Projects
{
    [Trait("category", "unit")]
    [Trait("category", "controller")]
    [Trait("group", "project")]
    public class SearchControllerTest
    {
        [Fact]
        public async void SearProject_Page_Sucess()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<SearchController>(Permissions.ProjectView);
            var service = helper.GetService<Mock<IProjectService>>();

            service.Setup(m => m.GetPage(It.IsAny<Entity.ProjectFilter>()))
                .ReturnsAsync(new Entity.Paged<PimsProject>());

            // Act
            var result = await controller.GetProject(new ProjectFilterModel());

            // Assert
            service.Verify(m => m.GetPage(It.IsAny<Entity.ProjectFilter>()), Times.Once());
        }
    }
}
