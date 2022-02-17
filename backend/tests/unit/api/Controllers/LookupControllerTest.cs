using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Controllers;
using Pims.Core.Comparers;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal;
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
        #endregion

        #region Constructors
        public LookupControllerTest()
        {
        }
        #endregion

        #region Tests
        [Fact]
        public void GetOrganizationCodes()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LookupController>(Permissions.PropertyView);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var organization = new Entity.PimsOrganization
            {
                OrganizationName = "Ministry of Health",
            };
            service.Setup(m => m.Lookup.GetOrganizations()).Returns(new[] { organization });

            // Act
            var result = controller.GetOrganizations();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.OrganizationModel[]>(actionResult.Value);
            Assert.Equal(new[] { mapper.Map<Model.OrganizationModel>(organization) }, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Lookup.GetOrganizations(), Times.Once());
        }

        [Fact]
        public void GetPropertyClassificationTypeCodes()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LookupController>(Permissions.PropertyView);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var propertyClassification = new Entity.PimsPropertyClassificationType
            {
                Id = "Surplus Active",
            };
            service.Setup(m => m.Lookup.GetPropertyClassificationTypes()).Returns(new[] { propertyClassification });

            // Act
            var result = controller.GetPropertyClassificationTypes();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.LookupModel[]>(actionResult.Value);
            Assert.Equal(new[] { mapper.Map<Model.LookupModel>(propertyClassification) }, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Lookup.GetPropertyClassificationTypes(), Times.Once());
        }

        [Fact]
        public void GetRoleCodes()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LookupController>(Permissions.PropertyView);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var role = new Entity.PimsRole
            {
                Id = 1,
                RoleUid = Guid.NewGuid(),
                Name = "Ministry of Health",
                Description = "The Ministry of Health"
            };
            service.Setup(m => m.Lookup.GetRoles()).Returns(new[] { role });

            // Act
            var result = controller.GetRoles();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.RoleModel[]>(actionResult.Value);
            Assert.Equal(new[] { mapper.Map<Model.RoleModel>(role) }, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Lookup.GetRoles(), Times.Once());
        }

        [Fact]
        public void GetAll()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LookupController>(Permissions.PropertyView);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();

            var areaUnitTypes = EntityHelper.CreatePropertyAreaUnitType("area");
            service.Setup(m => m.Lookup.GetPropertyAreaUnitTypes()).Returns(new[] { areaUnitTypes });

            var classificationTypes = EntityHelper.CreatePropertyClassificationType("classification");
            service.Setup(m => m.Lookup.GetPropertyClassificationTypes()).Returns(new[] { classificationTypes });

            var countries = EntityHelper.CreateCountry(1, "CAN");
            service.Setup(m => m.Lookup.GetCountries()).Returns(new[] { countries });

            var districts = EntityHelper.CreateDistrict(1, "district");
            service.Setup(m => m.Lookup.GetDistricts()).Returns(new[] { districts });

            var organizationTypes = EntityHelper.CreateOrganizationType("orgtype");
            service.Setup(m => m.Lookup.GetOrganizationTypes()).Returns(new[] { organizationTypes });

            var organizations = EntityHelper.CreateOrganization(1, "organization");
            service.Setup(m => m.Lookup.GetOrganizations()).Returns(new[] { organizations });

            var propertyTypes = EntityHelper.CreatePropertyType("property");
            service.Setup(m => m.Lookup.GetPropertyTypes()).Returns(new[] { propertyTypes });

            var provinces = EntityHelper.CreateProvince(1, "BC");
            service.Setup(m => m.Lookup.GetProvinces()).Returns(new[] { provinces });

            var regions = EntityHelper.CreateRegion(1, "region");
            service.Setup(m => m.Lookup.GetRegions()).Returns(new[] { regions });

            var roleCodes = EntityHelper.CreateRole("admin");
            service.Setup(m => m.Lookup.GetRoles()).Returns(new[] { roleCodes });

            var tenureTypes = EntityHelper.CreatePropertyTenureType("tenure");
            service.Setup(m => m.Lookup.GetPropertyTenureTypes()).Returns(new[] { tenureTypes });

            // Act
            var result = controller.GetAll();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsAssignableFrom<IEnumerable<object>>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.LookupModel>(areaUnitTypes), actualResult.Next(0), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(classificationTypes), actualResult.Next(1), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(countries), actualResult.Next(2), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(districts), actualResult.Next(3), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(organizationTypes), actualResult.Next(4), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.OrganizationModel>(organizations), actualResult.Next(5), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(propertyTypes), actualResult.Next(6), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(provinces), actualResult.Next(7), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(regions), actualResult.Next(8), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.RoleModel>(roleCodes), actualResult.Next(9), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(tenureTypes), actualResult.Next(10), new ShallowPropertyCompare());
        }
        #endregion
    }
}
