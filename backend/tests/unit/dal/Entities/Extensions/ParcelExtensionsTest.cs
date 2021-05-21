using FluentAssertions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Helpers.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class ParcelExtensionsTest
    {
        #region Tests
        #region GetMostRecentEvaluation
        [Fact]
        public void GetMostRecentEvaluation_Value()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var building = new Parcel();
            building.Evaluations.Add(new ParcelEvaluation(building, date, EvaluationKeys.Assessed, 111.11m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.Subtract(new TimeSpan(1, 0, 0)), EvaluationKeys.Assessed, 222.22m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.Subtract(new TimeSpan(2, 0, 0)), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = building.GetMostRecentEvaluation(EvaluationKeys.Assessed);

            // Assert
            result.Should().Be(111.11m);
        }

        [Fact]
        public void GetMostRecentEvaluation_Null()
        {
            // Arrange
            var building = new Parcel();

            // Act
            var result = building.GetMostRecentEvaluation(EvaluationKeys.Assessed);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetMostRecentEvaluation_NoneFound()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var building = new Parcel();
            building.Evaluations.Add(new ParcelEvaluation(building, date, EvaluationKeys.Assessed, 111.11m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.Subtract(new TimeSpan(1, 0, 0)), EvaluationKeys.Assessed, 222.22m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.Subtract(new TimeSpan(2, 0, 0)), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = building.GetMostRecentEvaluation(EvaluationKeys.Appraised);

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region GetMostRecentEvaluationDate
        [Fact]
        public void GetMostRecentEvaluationDate_Value()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var building = new Parcel();
            building.Evaluations.Add(new ParcelEvaluation(building, date, EvaluationKeys.Assessed, 111.11m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.Subtract(new TimeSpan(1, 0, 0)), EvaluationKeys.Assessed, 222.22m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.Subtract(new TimeSpan(2, 0, 0)), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = building.GetMostRecentEvaluationDate(EvaluationKeys.Assessed);

            // Assert
            result.Should().Be(date);
        }

        [Fact]
        public void GetMostRecentEvaluationDate_Null()
        {
            // Arrange
            var building = new Parcel();

            // Act
            var result = building.GetMostRecentEvaluationDate(EvaluationKeys.Assessed);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetMostRecentEvaluationDate_NoneFound()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var building = new Parcel();
            building.Evaluations.Add(new ParcelEvaluation(building, date, EvaluationKeys.Assessed, 111.11m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.Subtract(new TimeSpan(1, 0, 0)), EvaluationKeys.Assessed, 222.22m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.Subtract(new TimeSpan(2, 0, 0)), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = building.GetMostRecentEvaluationDate(EvaluationKeys.Appraised);

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region GetCurrentEvaluation
        [Fact]
        public void GetCurrentEvaluation_Value()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var building = new Parcel();
            building.Evaluations.Add(new ParcelEvaluation(building, date, EvaluationKeys.Assessed, 111.11m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.AddYears(-1), EvaluationKeys.Assessed, 222.22m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.AddYears(-2), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = building.GetCurrentEvaluation(EvaluationKeys.Assessed);

            // Assert
            result.Should().Be(111.11m);
        }

        [Fact]
        public void GetCurrentEvaluation_Null()
        {
            // Arrange
            var building = new Parcel();

            // Act
            var result = building.GetCurrentEvaluation(EvaluationKeys.Assessed);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetCurrentEvaluation_NoneFound()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var building = new Parcel();
            building.Evaluations.Add(new ParcelEvaluation(building, date, EvaluationKeys.Assessed, 111.11m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.AddYears(-1), EvaluationKeys.Assessed, 222.22m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.AddYears(-2), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = building.GetCurrentEvaluation(EvaluationKeys.Appraised);

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region GetCurrentEvaluationDate
        [Fact]
        public void GetCurrentEvaluationDate_Value()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var building = new Parcel();
            building.Evaluations.Add(new ParcelEvaluation(building, date, EvaluationKeys.Assessed, 111.11m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.AddYears(-1), EvaluationKeys.Assessed, 222.22m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.AddYears(-2), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = building.GetCurrentEvaluationDate(EvaluationKeys.Assessed);

            // Assert
            result.Should().Be(date);
        }

        [Fact]
        public void GetCurrentEvaluationDate_Null()
        {
            // Arrange
            var building = new Parcel();

            // Act
            var result = building.GetCurrentEvaluationDate(EvaluationKeys.Assessed);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetCurrentEvaluationDate_NoneFound()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var building = new Parcel();
            building.Evaluations.Add(new ParcelEvaluation(building, date, EvaluationKeys.Assessed, 111.11m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.AddYears(-1), EvaluationKeys.Assessed, 222.22m));
            building.Evaluations.Add(new ParcelEvaluation(building, date.AddYears(-2), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = building.GetCurrentEvaluationDate(EvaluationKeys.Appraised);

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region GetMostRecentFiscal
        [Fact]
        public void GetMostRecentFiscal_Value()
        {
            // Arrange
            var year = DateTime.Now.GetFiscalYear();
            var building = new Parcel();
            building.Fiscals.Add(new ParcelFiscal(building, year, FiscalKeys.Market, 111.11m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 1, FiscalKeys.Market, 222.22m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 2, FiscalKeys.Market, 333.33m));

            // Act
            var result = building.GetMostRecentFiscal(FiscalKeys.Market);

            // Assert
            result.Should().Be(111.11m);
        }

        [Fact]
        public void GetMostRecentFiscal_Null()
        {
            // Arrange
            var building = new Parcel();

            // Act
            var result = building.GetMostRecentFiscal(FiscalKeys.Market);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetMostRecentFiscal_NoneFound()
        {
            // Arrange
            var year = DateTime.Now.GetFiscalYear();
            var building = new Parcel();
            building.Fiscals.Add(new ParcelFiscal(building, year, FiscalKeys.Market, 111.11m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 1, FiscalKeys.Market, 222.22m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 2, FiscalKeys.Market, 333.33m));

            // Act
            var result = building.GetMostRecentFiscal(FiscalKeys.NetBook);

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region GetMostRecentFiscalYear
        [Fact]
        public void GetMostRecentFiscalYear_Value()
        {
            // Arrange
            var year = DateTime.Now.GetFiscalYear();
            var building = new Parcel();
            building.Fiscals.Add(new ParcelFiscal(building, year, FiscalKeys.Market, 111.11m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 1, FiscalKeys.Market, 222.22m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 2, FiscalKeys.Market, 333.33m));

            // Act
            var result = building.GetMostRecentFiscalYear(FiscalKeys.Market);

            // Assert
            result.Should().Be(year);
        }

        [Fact]
        public void GetMostRecentFiscalYear_Null()
        {
            // Arrange
            var building = new Parcel();

            // Act
            var result = building.GetMostRecentFiscalYear(FiscalKeys.Market);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetMostRecentFiscalYear_NoneFound()
        {
            // Arrange
            var year = DateTime.Now.GetFiscalYear();
            var building = new Parcel();
            building.Fiscals.Add(new ParcelFiscal(building, year, FiscalKeys.NetBook, 111.11m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 1, FiscalKeys.NetBook, 222.22m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 2, FiscalKeys.NetBook, 333.33m));

            // Act
            var result = building.GetMostRecentFiscalYear(FiscalKeys.Market);

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region GetCurrentFiscal
        [Fact]
        public void GetCurrentFiscal_Value()
        {
            // Arrange
            var year = DateTime.Now.GetFiscalYear();
            var building = new Parcel();
            building.Fiscals.Add(new ParcelFiscal(building, year, FiscalKeys.Market, 111.11m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 1, FiscalKeys.Market, 222.22m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 2, FiscalKeys.Market, 333.33m));

            // Act
            var result = building.GetCurrentFiscal(FiscalKeys.Market);

            // Assert
            result.Should().Be(111.11m);
        }

        [Fact]
        public void GetCurrentFiscal_Null()
        {
            // Arrange
            var building = new Parcel();

            // Act
            var result = building.GetCurrentFiscal(FiscalKeys.Market);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetCurrentFiscal_NoneFound()
        {
            // Arrange
            var year = DateTime.Now.GetFiscalYear();
            var building = new Parcel();
            building.Fiscals.Add(new ParcelFiscal(building, year, FiscalKeys.Market, 111.11m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 1, FiscalKeys.Market, 222.22m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 2, FiscalKeys.Market, 333.33m));

            // Act
            var result = building.GetCurrentFiscal(FiscalKeys.NetBook);

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region GetCurrentFiscalYear
        [Fact]
        public void GetCurrentFiscalYear_Value()
        {
            // Arrange
            var year = DateTime.Now.GetFiscalYear();
            var building = new Parcel();
            building.Fiscals.Add(new ParcelFiscal(building, year, FiscalKeys.Market, 111.11m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 1, FiscalKeys.Market, 222.22m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 2, FiscalKeys.Market, 333.33m));

            // Act
            var result = building.GetCurrentFiscalYear(FiscalKeys.Market);

            // Assert
            result.Should().Be(year);
        }

        [Fact]
        public void GetCurrentFiscalYear_Null()
        {
            // Arrange
            var building = new Parcel();

            // Act
            var result = building.GetCurrentFiscalYear(FiscalKeys.Market);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetCurrentFiscalYear_NoneFound()
        {
            // Arrange
            var year = DateTime.Now.GetFiscalYear();
            var building = new Parcel();
            building.Fiscals.Add(new ParcelFiscal(building, year, FiscalKeys.NetBook, 111.11m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 1, FiscalKeys.NetBook, 222.22m));
            building.Fiscals.Add(new ParcelFiscal(building, year - 2, FiscalKeys.NetBook, 333.33m));

            // Act
            var result = building.GetCurrentFiscalYear(FiscalKeys.Market);

            // Assert
            result.Should().BeNull();
        }
        #endregion
        #endregion
    }
}
