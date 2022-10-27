using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Controllers;
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
        #endregion

        #region Constructors
        public LookupControllerTest()
        {
        }
        #endregion

        #region Tests

        [Fact]
        public void GetPropertyClassificationTypeCodes()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LookupController>(Permissions.PropertyView);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<ILookupRepository>>();
            var propertyClassification = new Entity.PimsPropertyClassificationType
            {
                Id = "Surplus Active",
            };
            repository.Setup(m => m.GetPropertyClassificationTypes()).Returns(new[] { propertyClassification });

            // Act
            var result = controller.GetPropertyClassificationTypes();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.LookupModel[]>(actionResult.Value);
            (new[] { mapper.Map<Model.LookupModel>(propertyClassification) }).Should().BeEquivalentTo(actualResult);
            repository.Verify(m => m.GetPropertyClassificationTypes(), Times.Once());
        }

        [Fact]
        public void GetRoleCodes()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LookupController>(Permissions.PropertyView);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<ILookupRepository>>();
            var role = new Entity.PimsRole
            {
                Id = 1,
                RoleUid = Guid.NewGuid(),
                Name = "Ministry of Health",
                Description = "The Ministry of Health",
            };
            repository.Setup(m => m.GetRoles()).Returns(new[] { role });

            // Act
            var result = controller.GetRoles();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.RoleModel[]>(actionResult.Value);
            (new[] { mapper.Map<Model.RoleModel>(role) }).Should().BeEquivalentTo(actualResult);
            repository.Verify(m => m.GetRoles(), Times.Once());
        }

        [Fact]
        public void GetAll()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LookupController>(Permissions.PropertyView);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<ILookupRepository>>();

            var areaUnitTypes = EntityHelper.CreatePropertyAreaUnitType("area");
            repository.Setup(m => m.GetPropertyAreaUnitTypes()).Returns(new[] { areaUnitTypes });

            var classificationTypes = EntityHelper.CreatePropertyClassificationType("classification");
            repository.Setup(m => m.GetPropertyClassificationTypes()).Returns(new[] { classificationTypes });

            var countries = EntityHelper.CreateCountry(1, "CAN");
            repository.Setup(m => m.GetCountries()).Returns(new[] { countries });

            var districts = EntityHelper.CreateDistrict(1, "district");
            repository.Setup(m => m.GetDistricts()).Returns(new[] { districts });

            var organizationTypes = EntityHelper.CreateOrganizationType("orgtype");
            repository.Setup(m => m.GetOrganizationTypes()).Returns(new[] { organizationTypes });

            var propertyTypes = EntityHelper.CreatePropertyType("property");
            repository.Setup(m => m.GetPropertyTypes()).Returns(new[] { propertyTypes });

            var provinces = EntityHelper.CreateProvince(1, "BC");
            repository.Setup(m => m.GetProvinces()).Returns(new[] { provinces });

            var regions = EntityHelper.CreateRegion(1, "region");
            repository.Setup(m => m.GetRegions()).Returns(new[] { regions });

            var roleCodes = EntityHelper.CreateRole("admin");
            repository.Setup(m => m.GetRoles()).Returns(new[] { roleCodes });

            var tenureTypes = EntityHelper.CreatePropertyTenureType("tenure");
            repository.Setup(m => m.GetPropertyTenureTypes()).Returns(new[] { tenureTypes });

            // Act
            var result = controller.GetAll();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsAssignableFrom<IEnumerable<object>>(actionResult.Value);
            mapper.Map<Model.LookupModel>(areaUnitTypes).Should().BeEquivalentTo(actualResult.Next(0));
            mapper.Map<Model.LookupModel>(classificationTypes).Should().BeEquivalentTo(actualResult.Next(1));
            mapper.Map<Model.LookupModel>(countries).Should().BeEquivalentTo(actualResult.Next(2));
            mapper.Map<Model.LookupModel>(districts).Should().BeEquivalentTo(actualResult.Next(3));
            mapper.Map<Model.LookupModel>(organizationTypes).Should().BeEquivalentTo(actualResult.Next(4));
            mapper.Map<Model.LookupModel>(propertyTypes).Should().BeEquivalentTo(actualResult.Next(5));
            mapper.Map<Model.LookupModel>(provinces).Should().BeEquivalentTo(actualResult.Next(6));
            mapper.Map<Model.LookupModel<short>>(regions).Should().BeEquivalentTo(actualResult.Next(7));
            mapper.Map<Model.RoleModel>(roleCodes).Should().BeEquivalentTo(actualResult.Next(8));
            mapper.Map<Model.LookupModel>(tenureTypes).Should().BeEquivalentTo(actualResult.Next(9));
        }
        #endregion
    }
}
