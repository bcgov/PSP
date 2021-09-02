using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Controllers;
using Pims.Core.Comparers;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
            var service = helper.GetService<Mock<IPimsService>>();
            var organization = new Entity.Organization
            {
                Name = "Ministry of Health",
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
            var service = helper.GetService<Mock<IPimsService>>();
            var propertyClassification = new Entity.PropertyClassificationType
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
            var service = helper.GetService<Mock<IPimsService>>();
            var role = new Entity.Role
            {
                Id = 1,
                Key = Guid.NewGuid(),
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
            var service = helper.GetService<Mock<IPimsService>>();

            var organization = EntityHelper.CreateOrganization(1, "organization");
            service.Setup(m => m.Lookup.GetOrganizations()).Returns(new[] { organization });

            var organizationType = EntityHelper.CreateOrganizationType("orgtype");
            service.Setup(m => m.Lookup.GetOrganizationTypes()).Returns(new[] { organizationType });

            var role = EntityHelper.CreateRole("admin");
            service.Setup(m => m.Lookup.GetRoles()).Returns(new[] { role });

            var province = EntityHelper.CreateProvince(1, "BC");
            service.Setup(m => m.Lookup.GetProvinces()).Returns(new[] { province });

            var country = EntityHelper.CreateCountry(1, "CAN");
            service.Setup(m => m.Lookup.GetCountries()).Returns(new[] { country });

            var region = EntityHelper.CreateRegion(1, "region");
            service.Setup(m => m.Lookup.GetRegions()).Returns(new[] { region });

            var district = EntityHelper.CreateDistrict(1, "district");
            service.Setup(m => m.Lookup.GetDistricts()).Returns(new[] { district });

            var propertyClassificationType = EntityHelper.CreatePropertyClassificationType("classification");
            service.Setup(m => m.Lookup.GetPropertyClassificationTypes()).Returns(new[] { propertyClassificationType });

            var propertyAreaUnitType = EntityHelper.CreatePropertyAreaUnitType("area");
            service.Setup(m => m.Lookup.GetPropertyAreaUnitTypes()).Returns(new[] { propertyAreaUnitType });

            var propertyTenureType = EntityHelper.CreatePropertyTenureType("tenure");
            service.Setup(m => m.Lookup.GetPropertyTenureTypes()).Returns(new[] { propertyTenureType });

            var propertyType = EntityHelper.CreatePropertyType("property");
            service.Setup(m => m.Lookup.GetPropertyTypes()).Returns(new[] { propertyType });

            // Act
            var result = controller.GetAll();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsAssignableFrom<IEnumerable<object>>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.RoleModel>(role), actualResult.Next(0), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.OrganizationModel>(organization), actualResult.Next(1), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(organizationType), actualResult.Next(2), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(country), actualResult.Next(3), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(province), actualResult.Next(4), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(region), actualResult.Next(5), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(district), actualResult.Next(6), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(propertyClassificationType), actualResult.Next(7), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(propertyAreaUnitType), actualResult.Next(8), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(propertyTenureType), actualResult.Next(9), new ShallowPropertyCompare());
            Assert.Equal(mapper.Map<Model.LookupModel>(propertyType), actualResult.Next(10), new ShallowPropertyCompare());
        }
        #endregion
    }
}
