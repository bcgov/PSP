using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Acquisition.Controllers;
using Pims.Api.Areas.Contact.Controllers;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Services;
using Pims.Api.Services.Interfaces;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;
using SearchController = Pims.Api.Areas.Contact.Controllers.SearchController;
using SModel = Pims.Api.Areas.Contact.Models.Search;

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
            new object [] { new Uri("http://host/api/contact/search?searchBy=organizations") },
            new object [] { new Uri("http://host/api/contact/search?searchBy=all") },
            new object [] { new Uri("http://host/api/contact/search?summary=person") },
            new object [] { new Uri("http://host/api/contact/search?summary=organization") },
            new object [] { new Uri("http://host/api/contact/search?municipality=victoria") },
            new object [] { new Uri("http://host/api/contact/search?activeContactsOnly=false") },
        };

        private TestHelper _helper;
        #endregion

        public SearchControllerTest()
        {
            this._helper = new TestHelper();
        }

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
            var controller = this._helper.CreateController<SearchController>(Permissions.ContactView);

            var service = this._helper.GetService<Mock<IContactService>>();
            var mapper = this._helper.GetService<IMapper>();

            service.Setup(m => m.GetPage(It.IsAny<ContactFilter>())).Returns(new Paged<Entity.PimsContactMgrVw>());

            // Act
            var result = controller.GetContacts(filter);

            // Assert
            service.Verify(m => m.GetPage(It.IsAny<ContactFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(ContactQueryFilters))]
        public void GetProperties_Query_Success(Uri uri)
        {
            // Arrange
            var controller = this._helper.CreateController<SearchController>(Permissions.ContactView, uri);

            var service = this._helper.GetService<Mock<IContactService>>();
            var mapper = this._helper.GetService<IMapper>();

            service.Setup(m => m.GetPage(It.IsAny<ContactFilter>())).Returns(new Paged<Entity.PimsContactMgrVw>());

            // Act
            var result = controller.GetContacts();

            // Assert
            service.Verify(m => m.GetPage(It.IsAny<ContactFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a failed request because the query doesn't contain filter values.
        /// </summary>
        [Fact]
        public void GetProperties_Query_NoFilter_BadRequest()
        {
            // Arrange
            var controller = this._helper.CreateController<SearchController>(Permissions.ContactView);
            var request = this._helper.GetService<Mock<HttpRequest>>();
            request.Setup(m => m.QueryString).Returns(new QueryString("?page=0"));

            var service = this._helper.GetService<Mock<IContactService>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.GetContacts());
            service.Verify(m => m.GetPage(It.IsAny<Entity.Models.ContactFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a fitler object.
        /// </summary>
        [Fact]
        public void GetProperties_NoFilter_BadRequest()
        {
            // Arrange
            var controller = this._helper.CreateController<SearchController>(Permissions.ContactView);

            var service = this._helper.GetService<Mock<IContactService>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.GetContacts(null));
            service.Verify(m => m.GetPage(It.IsAny<Entity.Models.ContactFilter>()), Times.Never());
        }
        #endregion
        #endregion
    }
}
