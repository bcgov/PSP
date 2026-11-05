using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Mapster;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.User;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
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
            var failures = new List<string>();

            // Act
            // Skip rules that are templates rather than concrete maps:
            // - open-generic rules (e.g. Paged<> -> PageModel<>) are closed on demand by Mapster at runtime;
            // - rules targeting abstract destinations (e.g. IBaseEntity -> BaseConcurrentModel) only exist to be inherited via Inherits<>().
            var concreteRules = config.RuleMap.Keys.Where(t =>
                !t.Source.ContainsGenericParameters
                && !t.Destination.ContainsGenericParameters
                && !t.Destination.IsAbstract);

            foreach (var tuple in concreteRules)
            {
                try
                {
                    config.Compile(tuple.Source, tuple.Destination);
                }
                catch (Exception ex)
                {
                    failures.Add($"{tuple.Source.FullName} -> {tuple.Destination.FullName}: {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            // Assert
            failures.Should().BeEmpty("because all mapping rules must align flawlessly at runtime");
        }

        [Fact]
        public void Mapster_OpenGeneric_PageModel_Mapping_Should_Compile_When_Closed()
        {
            // Arrange
            var helper = new TestHelper();
            var config = helper.GetService<TypeAdapterConfig>();

            // Act
            Action act = () => config.Compile(typeof(Paged<PimsUser>), typeof(PageModel<UserModel>));

            // Assert
            act.Should().NotThrow();
        }
    }
}
