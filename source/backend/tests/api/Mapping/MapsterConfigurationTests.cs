using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using Pims.Api;
using Pims.Api.Models.Concepts.Address;
using Pims.Core.Test;
using Xunit;

namespace Pims.Api.Tests.Mapping
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "mappings")]
    [ExcludeFromCodeCoverage]
    public class MapsterConfigurationTests
    {
        [Fact]
        public void Mapster_Configuration_Across_All_Assemblies_Should_Compile_Successfully()
        {
            // Arrange
            var helper = new TestHelper();
            var config = helper.GetService<TypeAdapterConfig>();

            // Act
            Action act = () => config.Compile();

            // Assert
            act.Should().NotThrow("because all mapping rules must align flawlessly at runtime");
        }
    }
}
