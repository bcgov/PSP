using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Research.Controllers;
using Pims.Api.Areas.Research.Models.Search;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;
using SModel = Pims.Api.Areas.Research.Models.Search;

namespace Pims.Api.Test.Controllers.Research
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "research")]
    [ExcludeFromCodeCoverage]
    public class SearchControllerTest
    {
        #region Variables
        public readonly static IEnumerable<object[]> ResearchFilters = new List<object[]>()
        {
            new object [] { new SModel.ResearchFilterModel() { RegionCode = 1 } },
            new object [] { new SModel.ResearchFilterModel() { RFileNumber = "1234" } },
            new object [] { new SModel.ResearchFilterModel() { Name = "rname" } },
        };

        public readonly static IEnumerable<object[]> ResearchQueryFilters = new List<object[]>()
        {
            new object [] { new Uri("http://host/api/research/search?RegionCode=1") },
            new object [] { new Uri("http://host/api/research/search?RFileNumber=100-000-000") },
            new object [] { new Uri("http://host/api/research/search?Name=rname") },
        };

        private readonly TestHelper _helper;
        #endregion

        public SearchControllerTest()
        {
            this._helper = new TestHelper();
        }

        #region Tests
        #region GetResearchFiles
        /// <summary>
        /// Make a successful request that includes the research filter.
        /// </summary>
        [Theory]
        [MemberData(nameof(ResearchFilters))]
        public void GetResearchFiles_All_Success(SModel.ResearchFilterModel filter)
        {
            // Arrange
            var controller = this._helper.CreateController<SearchController>(Permissions.ResearchFileView);

            var researchFiles = new[] { EntityHelper.CreateResearchFile(1) };

            var service = this._helper.GetService<Mock<IResearchFileService>>();
            var mapper = this._helper.GetService<IMapper>();

            service.Setup(m => m.GetPage(It.IsAny<ResearchFilter>())).Returns(new Paged<Entity.PimsResearchFile>(researchFiles));

            // Act
            var result = controller.GetResearchFiles(filter);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Models.PageModel<ResearchFileModel>>(actionResult.Value);
            var expectedResult = mapper.Map<Models.PageModel<ResearchFileModel>>(new Paged<Entity.PimsResearchFile>(researchFiles));
            expectedResult.Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.GetPage(It.IsAny<ResearchFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(ResearchQueryFilters))]
        public void GetResearchFiles_Query_Success(Uri uri)
        {
            // Arrange
            var controller = this._helper.CreateController<SearchController>(Permissions.ResearchFileView, uri);

            var researchFiles = new[] { EntityHelper.CreateResearchFile(1) };

            var service = this._helper.GetService<Mock<IResearchFileService>>();
            var mapper = this._helper.GetService<IMapper>();

            service.Setup(m => m.GetPage(It.IsAny<ResearchFilter>())).Returns(new Paged<Entity.PimsResearchFile>(researchFiles));

            // Act
            var result = controller.GetResearchFiles();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Models.PageModel<ResearchFileModel>>(actionResult.Value);
            var expectedResult = mapper.Map<Models.PageModel<ResearchFileModel>>(new Paged<Entity.PimsResearchFile>(researchFiles));
            expectedResult.Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.GetPage(It.IsAny<ResearchFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a failed request because the query doesn't contain filter values.
        /// </summary>
        [Fact]
        public void GetResearchFiles_Query_NoFilter_BadRequest()
        {
            // Arrange
            var controller = this._helper.CreateController<SearchController>(Permissions.ResearchFileView);
            var request = this._helper.GetService<Mock<HttpRequest>>();
            request.Setup(m => m.QueryString).Returns(new QueryString("?page=0"));

            var service = this._helper.GetService<Mock<IResearchFileService>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.GetResearchFiles());
            service.Verify(m => m.GetPage(It.IsAny<Entity.Models.ResearchFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a fitler object.
        /// </summary>
        [Fact]
        public void GetResearchFiles_NoFilter_BadRequest()
        {
            // Arrange
            var controller = this._helper.CreateController<SearchController>(Permissions.ResearchFileView);

            var service = this._helper.GetService<Mock<IResearchFileService>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.GetResearchFiles(null));
            service.Verify(m => m.GetPage(It.IsAny<Entity.Models.ResearchFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a fitler object.
        /// </summary>
        [Fact]
        public void GetResearchFiles_InvalidFilter_BadRequest()
        {
            // Arrange
            var controller = this._helper.CreateController<SearchController>(Permissions.ResearchFileView);

            var service = this._helper.GetService<Mock<IResearchFileService>>();
            var filter = new ResearchFilterModel() { CreatedOnStartDate = DateTime.Now, CreatedOnEndDate = DateTime.Now.AddDays(-1) };

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.GetResearchFiles(filter));
            service.Verify(m => m.GetPage(It.IsAny<Entity.Models.ResearchFilter>()), Times.Never());
        }
        #endregion
        #endregion
    }
}
