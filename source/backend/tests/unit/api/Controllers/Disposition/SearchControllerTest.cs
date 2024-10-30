using System.Diagnostics.CodeAnalysis;
using MapsterMapper;
using Moq;
using Pims.Api.Areas.Disposition.Controllers;
using Pims.Api.Areas.Disposition.Models.Search;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "disposition")]
    [ExcludeFromCodeCoverage]
    public class SearchControllerTest
    {
        #region Variables
        private Mock<IDispositionFileService> _service;
        private SearchController _controller;
        private IMapper _mapper;
        #endregion

        public SearchControllerTest()
        {
            var helper = new TestHelper();
            this._controller = helper.CreateController<SearchController>(Permissions.DispositionView);
            this._mapper = helper.GetService<IMapper>();
            this._service = helper.GetService<Mock<IDispositionFileService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request to search disposition files.
        /// </summary>
        [Fact]
        public void GetDispositionFiles_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetPage(It.IsAny<DispositionFilter>())).Returns(new Paged<PimsDispositionFile>());

            // Act
            var result = this._controller.GetDispositionFiles(new DispositionFilterModel());

            // Assert
            this._service.Verify(m => m.GetPage(It.IsAny<DispositionFilter>()), Times.Once());
        }
        #endregion
    }
}
