using FluentAssertions;
using MapsterMapper;
using Pims.Core.Test;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Project.Models.Dispose;

namespace Pims.Api.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "mapping")]
    [ExcludeFromCodeCoverage]
    public class BuildingModelTest
    {
        #region Tests
        [Fact]
        public void Mapping_To_Model()
        {
            // Arrange
            var helper = new TestHelper();
            var mapper = helper.GetService<IMapper>();

            var building = EntityHelper.CreateBuilding(1);

            // Act
            var result = mapper.Map<Model.BuildingModel>(building);

            // Assert
            Assert.NotNull(result);
            result.Agency.Should().Be(building.Agency.Code);
            result.SubAgency.Should().BeNull();
        }

        [Fact]
        public void Mapping_To_Entity()
        {
            // Arrange
            var helper = new TestHelper();
            var mapper = helper.GetService<IMapper>();

            var model = new Model.BuildingModel()
            {
                AgencyId = 1,
                Address = new Models.Parcel.AddressModel()
                {
                    Id = 2
                }
            };

            // Act
            var result = mapper.Map<Entity.Building>(model);

            // Assert
            Assert.NotNull(result);
            result.AgencyId.Should().Be(model.AgencyId);
            result.AddressId.Should().Be(model.Address.Id);
        }
        #endregion
    }
}
