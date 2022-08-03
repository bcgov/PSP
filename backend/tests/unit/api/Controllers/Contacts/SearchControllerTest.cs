using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Contact.Controllers;
using Pims.Api.Helpers.Exceptions;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Entity = Pims.Dal.Entities;
using SModel = Pims.Api.Areas.Contact.Models.Search;
using FluentAssertions;

namespace Pims.Api.Test.Controllers.Contact
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "contact")]
    [ExcludeFromCodeCoverage]
    public class SearchControllerTest
    {
        #region Variables
        public readonly static IEnumerable<object[]> ContactFilters = new List<object[]>()
        {
            new object [] { new SModel.ContactFilterModel() { SearchBy = "persons" } },
            new object [] { new SModel.ContactFilterModel() { SearchBy = "organizations" } },
            new object [] { new SModel.ContactFilterModel() { SearchBy = "all" } },
            new object [] { new SModel.ContactFilterModel() { Summary = "person" } },
            new object [] { new SModel.ContactFilterModel() { Summary = "organization" } },
            new object [] { new SModel.ContactFilterModel() { Municipality = "victoria" } },
            new object [] { new SModel.ContactFilterModel() { ActiveContactsOnly = false } },
        };

        public readonly static IEnumerable<object[]> ContactQueryFilters = new List<object[]>()
        {
            new object [] { new Uri("http://host/api/contact/search?searchBy=persons") },
            new object [] { new Uri("http://host/api/contact/search?searchBy=organizations")},
            new object [] { new Uri("http://host/api/contact/search?searchBy=all") },
            new object [] { new Uri("http://host/api/contact/search?summary=person")},
            new object [] { new Uri("http://host/api/contact/search?summary=organization") },
            new object [] { new Uri("http://host/api/contact/search?municipality=victoria") },
            new object [] { new Uri("http://host/api/contact/search?activeContactsOnly=false") },
        };
        #endregion

        #region Tests
        #region GetProperties
        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(ContactFilters))]
        public void GetProperties_All_Success(SModel.ContactFilterModel filter)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<SearchController>(Permissions.ContactView);

            var contacts = new[] { EntityHelper.CreateContact("1", firstName: "person") };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Contact.GetPage(It.IsAny<ContactFilter>())).Returns(new Paged<Entity.PimsContactMgrVw>(contacts));

            // Act
            var result = controller.GetContacts(filter);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Models.PageModel<SModel.ContactSummaryModel>>(actionResult.Value);
            var expectedResult = mapper.Map<Models.PageModel<SModel.ContactSummaryModel>>(new Paged<Entity.PimsContactMgrVw>(contacts));
            expectedResult.Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.Contact.GetPage(It.IsAny<ContactFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(ContactQueryFilters))]
        public void GetProperties_Query_Success(Uri uri)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<SearchController>(Permissions.ContactView, uri);

            var contacts = new[] { EntityHelper.CreateContact("1", firstName: "person") };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Contact.GetPage(It.IsAny<ContactFilter>())).Returns(new Paged<Entity.PimsContactMgrVw>(contacts));

            // Act
            var result = controller.GetContacts();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Models.PageModel<SModel.ContactSummaryModel>>(actionResult.Value);
            var expectedResult = mapper.Map<Models.PageModel<SModel.ContactSummaryModel>>(new Paged<Entity.PimsContactMgrVw>(contacts));
            expectedResult.Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.Contact.GetPage(It.IsAny<ContactFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a failed request because the query doesn't contain filter values.
        /// </summary>
        [Fact]
        public void GetProperties_Query_NoFilter_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<SearchController>(Permissions.ContactView);
            var request = helper.GetService<Mock<HttpRequest>>();
            request.Setup(m => m.QueryString).Returns(new QueryString("?page=0"));

            var service = helper.GetService<Mock<IPimsRepository>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.GetContacts());
            service.Verify(m => m.Contact.GetPage(It.IsAny<Entity.Models.ContactFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a fitler object.
        /// </summary>
        [Fact]
        public void GetProperties_NoFilter_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<SearchController>(Permissions.ContactView);

            var service = helper.GetService<Mock<IPimsRepository>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.GetContacts(null));
            service.Verify(m => m.Contact.GetPage(It.IsAny<Entity.Models.ContactFilter>()), Times.Never());
        }
        #endregion
        #endregion
    }
}
