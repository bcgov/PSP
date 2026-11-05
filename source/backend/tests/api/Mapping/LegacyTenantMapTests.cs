using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Options;
using Pims.Api.Mapping.Tenant;
using Pims.Dal;
using Xunit;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Tenant;

namespace Pims.Api.Tests.Mapping
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "mappings")]
    [ExcludeFromCodeCoverage]
    public class LegacyTenantMapTests
    {
        private readonly IMapper _mapper;

        public LegacyTenantMapTests()
        {
            // 1. Define the option structures exactly as your API would configuration-wise
            var serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var pimsOptions = new PimsOptions { HelpDeskEmail = "env-override@pims.com" };

            // 2. Wrap them manually into Microsoft.Extensions.Options wrappers
            var wrappedSerializer = Options.Create(serializerOptions);
            var wrappedPims = Options.Create(pimsOptions);

            // 3. Instantiate the mapper by manually passing dependencies into the constructor
            var tenantMap = new TenantMap(wrappedSerializer, wrappedPims);

            // 4. Register the map to an isolated, localized configuration instance.
            //    Mirror the API's global setting (see Startup.AddMapster) so only explicitly mapped members are mapped;
            //    otherwise Mapster auto-maps Settings (string <-> TenantSettingsModel) by name and throws.
            var config = new TypeAdapterConfig();
            config.Default.IgnoreNonMapped(true);
            config.Default.IgnoreNullValues(true);
            config.AllowImplicitDestinationInheritance = true;
            config.AllowImplicitSourceInheritance = true;
            tenantMap.Register(config);

            _mapper = new Mapper(config);
        }

        [Fact]
        public void Map_PimsTenant_To_TenantModel_ShouldDeserializeJsonAndApplyEnvironmentOverride()
        {
            // Arrange
            var sourceEntity = new Entity.PimsTenant
            {
                Code = "BC",
                Name = "British Columbia",
                Settings = "{\"helpDeskEmail\":\"original@pims.com\",\"otherSetting\":\"value\"}"
            };

            // Act
            var result = _mapper.Map<Model.TenantModel>(sourceEntity);

            // Assert
            result.Should().NotBeNull();
            result.Code.Should().Be("BC");
            result.Name.Should().Be("British Columbia");
            result.Settings.Should().NotBeNull();
            result.Settings.HelpDeskEmail.Should().Be("env-override@pims.com");
        }

        [Fact]
        public void Map_TenantModel_To_PimsTenant_ShouldSerializeSettingsToJsonString()
        {
            // Arrange
            var sourceModel = new Model.TenantModel
            {
                Code = "CA",
                Name = "Canada",
                Settings = new Model.TenantSettingsModel
                {
                    HelpDeskEmail = "test@pims.com"
                }
            };

            // Act
            var result = _mapper.Map<Entity.PimsTenant>(sourceModel);

            // Assert
            result.Should().NotBeNull();
            result.Code.Should().Be("CA");
            result.Name.Should().Be("Canada");
            result.Settings.Should().NotBeNull();
            result.Settings.Should().Contain("test@pims.com");
        }
    }
}
