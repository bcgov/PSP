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
            var parcel = new Parcel();
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date, EvaluationKeys.Assessed, 111.11m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddHours(-1), EvaluationKeys.Assessed, 222.22m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddHours(-2), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = parcel.GetMostRecentEvaluation(EvaluationKeys.Assessed);

            // Assert
            result.Should().Be(111.11m);
        }

        [Fact]
        public void GetMostRecentEvaluation_Null()
        {
            // Arrange
            var parcel = new Parcel();

            // Act
            var result = parcel.GetMostRecentEvaluation(EvaluationKeys.Assessed);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetMostRecentEvaluation_NoneFound()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var parcel = new Parcel();
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date, EvaluationKeys.Assessed, 111.11m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddHours(-1), EvaluationKeys.Assessed, 222.22m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddHours(-2), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = parcel.GetMostRecentEvaluation(EvaluationKeys.Appraised);

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
            var parcel = new Parcel();
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date, EvaluationKeys.Assessed, 111.11m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddHours(-1), EvaluationKeys.Assessed, 222.22m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddHours(-2), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = parcel.GetMostRecentEvaluationDate(EvaluationKeys.Assessed);

            // Assert
            result.Should().Be(date);
        }

        [Fact]
        public void GetMostRecentEvaluationDate_Null()
        {
            // Arrange
            var parcel = new Parcel();

            // Act
            var result = parcel.GetMostRecentEvaluationDate(EvaluationKeys.Assessed);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetMostRecentEvaluationDate_NoneFound()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var parcel = new Parcel();
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date, EvaluationKeys.Assessed, 111.11m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddHours(-1), EvaluationKeys.Assessed, 222.22m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddHours(-2), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = parcel.GetMostRecentEvaluationDate(EvaluationKeys.Appraised);

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
            var parcel = new Parcel();
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date, EvaluationKeys.Assessed, 111.11m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddYears(-1), EvaluationKeys.Assessed, 222.22m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddYears(-2), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = parcel.GetCurrentEvaluation(EvaluationKeys.Assessed);

            // Assert
            result.Should().Be(111.11m);
        }

        [Fact]
        public void GetCurrentEvaluation_Null()
        {
            // Arrange
            var parcel = new Parcel();

            // Act
            var result = parcel.GetCurrentEvaluation(EvaluationKeys.Assessed);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetCurrentEvaluation_NoneFound()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var parcel = new Parcel();
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date, EvaluationKeys.Assessed, 111.11m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddYears(-1), EvaluationKeys.Assessed, 222.22m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddYears(-2), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = parcel.GetCurrentEvaluation(EvaluationKeys.Appraised);

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
            var parcel = new Parcel();
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date, EvaluationKeys.Assessed, 111.11m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddYears(-1), EvaluationKeys.Assessed, 222.22m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddYears(-2), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = parcel.GetCurrentEvaluationDate(EvaluationKeys.Assessed);

            // Assert
            result.Should().Be(date);
        }

        [Fact]
        public void GetCurrentEvaluationDate_Null()
        {
            // Arrange
            var parcel = new Parcel();

            // Act
            var result = parcel.GetCurrentEvaluationDate(EvaluationKeys.Assessed);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetCurrentEvaluationDate_NoneFound()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var parcel = new Parcel();
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date, EvaluationKeys.Assessed, 111.11m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddYears(-1), EvaluationKeys.Assessed, 222.22m));
            parcel.Evaluations.Add(new ParcelEvaluation(parcel, date.AddYears(-2), EvaluationKeys.Assessed, 333.33m));

            // Act
            var result = parcel.GetCurrentEvaluationDate(EvaluationKeys.Appraised);

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
            var parcel = new Parcel();
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year, FiscalKeys.Market, 111.11m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 1, FiscalKeys.Market, 222.22m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 2, FiscalKeys.Market, 333.33m));

            // Act
            var result = parcel.GetMostRecentFiscal(FiscalKeys.Market);

            // Assert
            result.Should().Be(111.11m);
        }

        [Fact]
        public void GetMostRecentFiscal_Null()
        {
            // Arrange
            var parcel = new Parcel();

            // Act
            var result = parcel.GetMostRecentFiscal(FiscalKeys.Market);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetMostRecentFiscal_NoneFound()
        {
            // Arrange
            var year = DateTime.Now.GetFiscalYear();
            var parcel = new Parcel();
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year, FiscalKeys.Market, 111.11m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 1, FiscalKeys.Market, 222.22m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 2, FiscalKeys.Market, 333.33m));

            // Act
            var result = parcel.GetMostRecentFiscal(FiscalKeys.NetBook);

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
            var parcel = new Parcel();
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year, FiscalKeys.Market, 111.11m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 1, FiscalKeys.Market, 222.22m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 2, FiscalKeys.Market, 333.33m));

            // Act
            var result = parcel.GetMostRecentFiscalYear(FiscalKeys.Market);

            // Assert
            result.Should().Be(year);
        }

        [Fact]
        public void GetMostRecentFiscalYear_Null()
        {
            // Arrange
            var parcel = new Parcel();

            // Act
            var result = parcel.GetMostRecentFiscalYear(FiscalKeys.Market);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetMostRecentFiscalYear_NoneFound()
        {
            // Arrange
            var year = DateTime.Now.GetFiscalYear();
            var parcel = new Parcel();
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year, FiscalKeys.NetBook, 111.11m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 1, FiscalKeys.NetBook, 222.22m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 2, FiscalKeys.NetBook, 333.33m));

            // Act
            var result = parcel.GetMostRecentFiscalYear(FiscalKeys.Market);

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
            var parcel = new Parcel();
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year, FiscalKeys.Market, 111.11m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 1, FiscalKeys.Market, 222.22m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 2, FiscalKeys.Market, 333.33m));

            // Act
            var result = parcel.GetCurrentFiscal(FiscalKeys.Market);

            // Assert
            result.Should().Be(111.11m);
        }

        [Fact]
        public void GetCurrentFiscal_Null()
        {
            // Arrange
            var parcel = new Parcel();

            // Act
            var result = parcel.GetCurrentFiscal(FiscalKeys.Market);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetCurrentFiscal_NoneFound()
        {
            // Arrange
            var year = DateTime.Now.GetFiscalYear();
            var parcel = new Parcel();
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year, FiscalKeys.Market, 111.11m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 1, FiscalKeys.Market, 222.22m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 2, FiscalKeys.Market, 333.33m));

            // Act
            var result = parcel.GetCurrentFiscal(FiscalKeys.NetBook);

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
            var parcel = new Parcel();
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year, FiscalKeys.Market, 111.11m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 1, FiscalKeys.Market, 222.22m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 2, FiscalKeys.Market, 333.33m));

            // Act
            var result = parcel.GetCurrentFiscalYear(FiscalKeys.Market);

            // Assert
            result.Should().Be(year);
        }

        [Fact]
        public void GetCurrentFiscalYear_Null()
        {
            // Arrange
            var parcel = new Parcel();

            // Act
            var result = parcel.GetCurrentFiscalYear(FiscalKeys.Market);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetCurrentFiscalYear_NoneFound()
        {
            // Arrange
            var year = DateTime.Now.GetFiscalYear();
            var parcel = new Parcel();
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year, FiscalKeys.NetBook, 111.11m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 1, FiscalKeys.NetBook, 222.22m));
            parcel.Fiscals.Add(new ParcelFiscal(parcel, year - 2, FiscalKeys.NetBook, 333.33m));

            // Act
            var result = parcel.GetCurrentFiscalYear(FiscalKeys.Market);

            // Assert
            result.Should().BeNull();
        }
        #endregion
        #endregion
    }
}
