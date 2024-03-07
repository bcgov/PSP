using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DocumentFormat.OpenXml.Office2010.Excel;
using FluentAssertions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "disposition")]
    [ExcludeFromCodeCoverage]
    public class DispositionStatusSolverTests
    {
        #region Tests
        public static IEnumerable<object[]> CanEditDetailsParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {DispositionStatusTypes.ACTIVE, true},
                new object[] {DispositionStatusTypes.DRAFT, true},
                new object[] {DispositionStatusTypes.ARCHIVED, false},
                new object[] {DispositionStatusTypes.CANCELLED, false},
                new object[] {DispositionStatusTypes.CLOSED, false},
                new object[] {DispositionStatusTypes.COMPLETE, false},
                new object[] {DispositionStatusTypes.HOLD, false},
            };

        [Theory]
        [MemberData(nameof(CanEditDetailsParameters))]
        public void CanEditDetails_Parametrized(DispositionStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new DispositionStatusSolver();

            // Act
            var result = solver.CanEditDetails(status);

            // Assert            
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditPropertiesParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {DispositionStatusTypes.ACTIVE, true},
                new object[] {DispositionStatusTypes.DRAFT, true},
                new object[] {DispositionStatusTypes.ARCHIVED, false},
                new object[] {DispositionStatusTypes.CANCELLED, false},
                new object[] {DispositionStatusTypes.CLOSED, false},
                new object[] {DispositionStatusTypes.COMPLETE, false},
                new object[] {DispositionStatusTypes.HOLD, false},
            };

        [Theory]
        [MemberData(nameof(CanEditPropertiesParameters))]
        public void CanEditProperties_Parametrized(DispositionStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new DispositionStatusSolver();

            // Act
            var result = solver.CanEditProperties(status);

            // Assert            
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditOrDeleteValuesOffersSalesParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {DispositionStatusTypes.ACTIVE, true},
                new object[] {DispositionStatusTypes.DRAFT, true},
                new object[] {DispositionStatusTypes.ARCHIVED, false},
                new object[] {DispositionStatusTypes.CANCELLED, false},
                new object[] {DispositionStatusTypes.CLOSED, false},
                new object[] {DispositionStatusTypes.COMPLETE, false},
                new object[] {DispositionStatusTypes.HOLD, false},
            };

        [Theory]
        [MemberData(nameof(CanEditOrDeleteValuesOffersSalesParameters))]
        public void CanEditOrDeleteValuesOffersSales_Parametrized(DispositionStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new DispositionStatusSolver();

            // Act
            var result = solver.CanEditOrDeleteValuesOffersSales(status);

            // Assert            
            Assert.Equal(expectedResult, result);
        }
        #endregion
    }
}
