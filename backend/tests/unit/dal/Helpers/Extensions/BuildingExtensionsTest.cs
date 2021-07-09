using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Pims.Dal.Test.Helpers.Extensions
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "extensions")]
    [Trait("group", "building")]
    [ExcludeFromCodeCoverage]
    public class BuildingExtensionsTest
    {
        #region Variables
        public static IEnumerable<object[]> BuildingFilters =>
            new List<object[]>
            {
                new object[] { new BuildingFilter() { ProjectNumber = null }, 10 },
                new object[] { new BuildingFilter() { ProjectNumber = "" }, 10 },
                new object[] { new BuildingFilter() { ProjectNumber = " " }, 10 },
                new object[] { new BuildingFilter() { ProjectNumber = "1234" }, 1 },
                new object[] { new BuildingFilter() { FloorCount = null }, 10 },
                new object[] { new BuildingFilter() { FloorCount = 1 }, 1 },
                new object[] { new BuildingFilter() { Zoning = null }, 10 },
                new object[] { new BuildingFilter() { Zoning = "" }, 10 },
                new object[] { new BuildingFilter() { Zoning = " " }, 10 },
                new object[] { new BuildingFilter() { Zoning = "Zoning" }, 1 },
                new object[] { new BuildingFilter() { ZoningPotential = null }, 10 },
                new object[] { new BuildingFilter() { ZoningPotential = "" }, 10 },
                new object[] { new BuildingFilter() { ZoningPotential = " " }, 10 },
                new object[] { new BuildingFilter() { ZoningPotential = "ZoningPotential" }, 1 },
                new object[] { new BuildingFilter() { Address = null }, 10 },
                new object[] { new BuildingFilter() { Address = "" }, 10 },
                new object[] { new BuildingFilter() { Address = " " }, 10 },
                new object[] { new BuildingFilter() { Address = "1243 St" }, 1 },
                new object[] { new BuildingFilter() { MinMarketValue = null }, 10 },
                new object[] { new BuildingFilter() { MinMarketValue = 1 }, 0 },
                new object[] { new BuildingFilter() { MaxMarketValue = null }, 10 },
                new object[] { new BuildingFilter() { MaxMarketValue = 1 }, 0 },
                new object[] { new BuildingFilter() { MinAssessedValue = null }, 10 },
                new object[] { new BuildingFilter() { MinAssessedValue = 1 }, 0 },
                new object[] { new BuildingFilter() { MaxAssessedValue = null }, 10 },
                new object[] { new BuildingFilter() { MaxAssessedValue = 1 }, 0 },
                new object[] { new BuildingFilter() { Sort = new string[] { "Name" } }, 10 },
            };
        #endregion

        #region Tests
        #region ThrowIfNotUnique
        [Fact]
        public void ThrowIfNotUnique_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.SystemAdmin);
            var context = helper.InitializeDatabase(user);
            var parcel = context.CreateParcel(123);
            var building1 = context.CreateBuilding(1, null, "building 1");
            var building2 = context.CreateBuilding(2, null, "building 2");
            parcel.Buildings.Add(building1);
            parcel.Buildings.Add(building2);
            context.SaveChanges();

            var building3 = context.CreateBuilding(3, null, "building 3");

            // Act
            context.ThrowIfNotUnique(parcel, building3);

            // Assert
            Assert.True(true); // It didn't throw a DbUpdateException.
        }

        [Fact]
        public void ThrowIfNotUnique_NoParcel()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.SystemAdmin);
            var context = helper.InitializeDatabase(user);

            var building = context.CreateBuilding(3, null, "building 3");

            // Act
            context.ThrowIfNotUnique(null, building);

            // Assert
            Assert.True(true); // It didn't throw a DbUpdateException.
        }

        [Fact]
        public void ThrowIfNotUnique_BuildingWithoutName()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.SystemAdmin);
            var context = helper.InitializeDatabase(user);
            var parcel = context.CreateParcel(123);
            var building1 = context.CreateBuilding(1, null, null);
            var building2 = context.CreateBuilding(2, null, null);
            parcel.Buildings.Add(building1);
            parcel.Buildings.Add(building2);
            context.SaveChanges();

            var building3 = context.CreateBuilding(3, null, null);

            // Act
            context.ThrowIfNotUnique(null, building3);

            // Assert
            Assert.True(true); // It didn't throw a DbUpdateException.
        }

        [Fact]
        public void ThrowIfNotUnique_BuildingWithEmptyName()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.SystemAdmin);
            var context = helper.InitializeDatabase(user);
            var parcel = context.CreateParcel(123);
            var building1 = context.CreateBuilding(1, null, null);
            var building2 = context.CreateBuilding(2, null, null);
            parcel.Buildings.Add(building1);
            parcel.Buildings.Add(building2);
            context.SaveChanges();

            var building3 = context.CreateBuilding(3, null, "");

            // Act
            context.ThrowIfNotUnique(null, building3);

            // Assert
            Assert.True(true); // It didn't throw a DbUpdateException.
        }

        [Fact]
        public void ThrowIfNotUnique_DbUpdateException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.SystemAdmin);
            var context = helper.InitializeDatabase(user);
            var parcel = context.CreateParcel(123);
            var building1 = context.CreateBuilding(1, null, "building 1");
            var building2 = context.CreateBuilding(2, null, null);
            parcel.Buildings.Add(building1);
            parcel.Buildings.Add(building2);
            context.SaveChanges();

            var building3 = context.CreateBuilding(3, null, "building 1");

            // Act
            // Assert
            Assert.Throws<DbUpdateException>(() => context.ThrowIfNotUnique(parcel, building3));
        }
        #endregion

        #region GenerateQuery
        [Theory]
        [MemberData(nameof(BuildingFilters))]
        public void GenerateQuery(BuildingFilter filter, int expectedResult)
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.AdminProperties);
            var context = helper.CreatePimsContext("building-filter", user);

            // Only seed the first time the test runs.
            if (!context.Agencies.Any())
            {
                context.SeedDatabase();
                var parcel = context.CreateParcel(1234);
                parcel.Zoning = "Zoning";
                parcel.ZoningPotential = "ZoningPotential";
                var buildings = context.CreateBuildings(null, 1, 10);
                buildings.Next(0).ProjectNumbers = "4444,1234";
                buildings.Next(1).BuildingFloorCount = 1;
                buildings.Next(2).Parcels.Add(parcel);
                buildings.Next(3).Address.Address1 = "1243 St";
                context.SaveChanges();
            }

            // Act
            var query = context.GenerateQuery(user, filter);
            var result = query.ToArray();

            // Assert
            Assert.NotNull(query);
            result.Count().Should().Be(expectedResult);
        }
        #endregion

        #region GetZoning
        [Fact]
        public void Get_BuildingZoning()
        {
            // Arrange
            var zone = "Zoning";
            var parcel = new Parcel
            {
                Zoning = zone
            };
            var building = new Building();
            building.Parcels.Add(parcel);

            // Act
            var zoning = building.GetZoning();

            // Assert
            Assert.NotNull(zoning);
            Assert.IsAssignableFrom<IEnumerable<string>>(zoning);
            zoning.Should().HaveCount(1);
            zoning.First().Should().Be(zone);
        }

        [Fact]
        public void Get_BuildingZoning_NoParcel()
        {
            // Arrange
            var building = new Building();

            // Act
            var zoning = building.GetZoning();

            // Assert
            Assert.Empty(zoning);
        }
        #endregion

        #region GetZoningPotential
        [Fact]
        public void Get_BuildingZoningPotential()
        {
            // Arrange
            var zonePotential = "ZoningPotential";
            var parcel = new Parcel
            {
                ZoningPotential = zonePotential
            };
            var building = new Building();
            building.Parcels.Add(parcel);

            // Act
            var zoningPotential = building.GetZoningPotential();

            // Assert
            Assert.NotNull(zoningPotential);
            Assert.IsAssignableFrom<IEnumerable<string>>(zoningPotential);
            zoningPotential.Should().HaveCount(1);
            zoningPotential.First().Should().Be(zonePotential);
        }

        [Fact]
        public void Get_BuildingZoningPotential_NoParcel()
        {
            // Arrange
            var building = new Building();

            // Act
            var zoningPotential = building.GetZoningPotential();

            // Assert
            Assert.Empty(zoningPotential);
        }
        #endregion

        #region UpdateBuildingFinancials
        [Fact]
        public void UpdateBuildingFinancials_Existing()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.SystemAdmin);
            var context = helper.InitializeDatabase(user);
            var building = context.CreateBuilding(1);
            var date = DateTime.UtcNow;
            building.Evaluations.Add(new BuildingEvaluation(building, date, EvaluationKeys.Assessed, 350.34m));
            building.Fiscals.Add(new BuildingFiscal(building, 2021, FiscalKeys.Market, 344.34m) { EffectiveDate = date });
            context.SaveChanges();

            var evaluations = new List<BuildingEvaluation>() { new BuildingEvaluation(building, date, EvaluationKeys.Assessed, 100.34m) };
            var fiscals = new List<BuildingFiscal>() { new BuildingFiscal(building, 2021, FiscalKeys.Market, 200.34m) { EffectiveDate = date } };

            // Act
            context.UpdateBuildingFinancials(building, evaluations, fiscals);

            // Assert
            building.Evaluations.First().Value.Should().Be(100.34m);
            building.Fiscals.First().Value.Should().Be(200.34m);
        }

        [Fact]
        public void UpdateBuildingFinancials_Add()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.SystemAdmin);
            var context = helper.InitializeDatabase(user);
            var building = context.CreateBuilding(1);
            var date = DateTime.UtcNow;
            building.Evaluations.Add(new BuildingEvaluation(building, date, EvaluationKeys.Appraised, 350.34m));
            building.Fiscals.Add(new BuildingFiscal(building, 2021, FiscalKeys.NetBook, 344.34m) { EffectiveDate = date });
            context.SaveChanges();

            var evaluations = new List<BuildingEvaluation>() { new BuildingEvaluation(building, date, EvaluationKeys.Assessed, 100.34m) };
            var fiscals = new List<BuildingFiscal>() { new BuildingFiscal(building, 2021, FiscalKeys.Market, 200.34m) { EffectiveDate = date } };

            // Act
            context.UpdateBuildingFinancials(building, evaluations, fiscals);

            // Assert
            building.Evaluations.Should().HaveCount(2);
            building.Fiscals.Should().HaveCount(2);
        }

        [Fact]
        public void UpdateBuildingFinancials_NoChange()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.SystemAdmin);
            var context = helper.InitializeDatabase(user);
            var building = context.CreateBuilding(1);
            var date = DateTime.UtcNow;
            building.Evaluations.Add(new BuildingEvaluation(building, date, EvaluationKeys.Assessed, 350.34m));
            building.Fiscals.Add(new BuildingFiscal(building, 2021, FiscalKeys.Market, 344.34m) { EffectiveDate = date });
            context.SaveChanges();

            var evaluations = new List<BuildingEvaluation>() { new BuildingEvaluation(building, date, EvaluationKeys.Assessed, 350.34m) };
            var fiscals = new List<BuildingFiscal>() { new BuildingFiscal(building, 2021, FiscalKeys.Market, 344.34m) { EffectiveDate = date } };

            // Act
            context.UpdateBuildingFinancials(building, evaluations, fiscals);

            // Assert
            building.Evaluations.First().Value.Should().Be(350.34m);
            building.Fiscals.First().Value.Should().Be(344.34m);
            building.Evaluations.Should().HaveCount(1);
            building.Fiscals.Should().HaveCount(1);
        }
        #endregion
        #endregion
    }
}
