using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Common;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Pims.Api.Constants;
using Pims.Api.Controllers;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Lookup;

namespace Pims.Api.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lookup")]
    [ExcludeFromCodeCoverage]
    public class LookupControllerTest
    {
        #region Variables
        private Mock<ILookupRepository> _repository;
        private LookupController _controller;
        private IMapper _mapper;
        private TestHelper _helper;
        #endregion

        #region Constructors
        public LookupControllerTest()
        {
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<LookupController>(Permissions.SystemAdmin);
            this._mapper = this._helper.GetService<IMapper>();
            this._repository = this._helper.GetService<Mock<ILookupRepository>>();
        }
        #endregion

        #region Tests

        [Fact]
        public void GetPropertyClassificationTypeCodes()
        {
            // Arrange
            var propertyClassification = new Entity.PimsPropertyClassificationType
            {
                Id = "Surplus Active",
            };
            this._repository.Setup(m => m.GetAllPropertyClassificationTypes()).Returns(new[] { propertyClassification });

            // Act
            var result = this._controller.GetPropertyClassificationTypes();

            // Assert
            this._repository.Verify(m => m.GetAllPropertyClassificationTypes(), Times.Once());
        }

        [Fact]
        public void GetRoleCodes()
        {
            // Arrange
            var role = new Entity.PimsRole
            {
                Id = 1,
                RoleUid = Guid.NewGuid(),
                Name = "Ministry of Health",
                Description = "The Ministry of Health",
            };
            this._repository.Setup(m => m.GetAllRoles()).Returns(new[] { role });

            // Act
            var result = this._controller.GetRoles();

            // Assert
            this._repository.Verify(m => m.GetAllRoles(), Times.Once());
        }

        [Fact]
        public void GetAll()
        {
            // Arrange
            var memoryCache = this._helper.GetService<Mock<IMemoryCache>>();
            var cacheEntry = new Mock<ICacheEntry>();

            memoryCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntry.Object);
            var areaUnitTypes = EntityHelper.CreatePropertyAreaUnitType("area");
            this._repository.Setup(m => m.GetAllPropertyAreaUnitTypes()).Returns(new[] { areaUnitTypes });

            var classificationTypes = EntityHelper.CreatePropertyClassificationType("classification");
            this._repository.Setup(m => m.GetAllPropertyClassificationTypes()).Returns(new[] { classificationTypes });

            var countries = EntityHelper.CreateCountry(1, "CAN");
            this._repository.Setup(m => m.GetAllCountries()).Returns(new[] { countries });

            var districts = EntityHelper.CreateDistrict(1, "district");
            this._repository.Setup(m => m.GetAllDistricts()).Returns(new[] { districts });

            var organizationTypes = EntityHelper.CreateOrganizationType("orgtype");
            this._repository.Setup(m => m.GetAllOrganizationTypes()).Returns(new[] { organizationTypes });

            var propertyTypes = EntityHelper.CreatePropertyType("property");
            this._repository.Setup(m => m.GetAllPropertyTypes()).Returns(new[] { propertyTypes });

            var provinces = EntityHelper.CreateProvince(1, "BC");
            this._repository.Setup(m => m.GetAllProvinces()).Returns(new[] { provinces });

            var regions = EntityHelper.CreateRegion(1, "region");
            this._repository.Setup(m => m.GetAllRegions()).Returns(new[] { regions });

            var roleCodes = EntityHelper.CreateRole("admin");
            this._repository.Setup(m => m.GetAllRoles()).Returns(new[] { roleCodes });

            var tenureTypes = EntityHelper.CreatePropertyTenureType("tenure");
            this._repository.Setup(m => m.GetAllPropertyTenureTypes()).Returns(new[] { tenureTypes });

            // Act
            var result = (JsonResult)this._controller.GetAll();

            // Assert
            var lookups = (IEnumerable<object>)result.Value;
            this._repository.Verify(m => m.GetAllPropertyAreaUnitTypes(), Times.Once());
            this._repository.Verify(m => m.GetAllPropertyClassificationTypes(), Times.Once());
            this._repository.Verify(m => m.GetAllCountries(), Times.Once());
            this._repository.Verify(m => m.GetAllDistricts(), Times.Once());
            this._repository.Verify(m => m.GetAllOrganizationTypes(), Times.Once());
            this._repository.Verify(m => m.GetAllPropertyTypes(), Times.Once());
            this._repository.Verify(m => m.GetAllProvinces(), Times.Once());
            this._repository.Verify(m => m.GetAllRegions(), Times.Once());
            this._repository.Verify(m => m.GetAllPropertyTenureTypes(), Times.Once());
        }

        [Fact]
        public void GetAll_Cached()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LookupController>(Permissions.PropertyView);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<ILookupRepository>>();
            var memoryCache = helper.GetService<Mock<IMemoryCache>>();
            var cachedResult = (object)new JsonResult(string.Empty);

            memoryCache.Setup(m => m.TryGetValue(It.IsAny<object>(), out cachedResult)).Returns(true);

            // Act
            var result = controller.GetAll();

            // Assert
            result.IsSameOrEqualTo(cachedResult);
        }
        #endregion
    }
}
