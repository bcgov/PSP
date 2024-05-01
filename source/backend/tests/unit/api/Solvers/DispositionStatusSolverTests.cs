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
using Pims.Api.Models.CodeTypes;
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
                new object[] {DispositionFileStatusTypes.ACTIVE, true},
                new object[] {DispositionFileStatusTypes.DRAFT, true},
                new object[] {DispositionFileStatusTypes.HOLD, true},
                new object[] {DispositionFileStatusTypes.ARCHIVED, false},
                new object[] {DispositionFileStatusTypes.CANCELLED, false},
                new object[] {DispositionFileStatusTypes.COMPLETE, false},
            };

        [Theory]
        [MemberData(nameof(CanEditDetailsParameters))]
        public void CanEditDetails_Parametrized(DispositionFileStatusTypes? status, bool expectedResult)
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
                new object[] {DispositionFileStatusTypes.ACTIVE, true},
                new object[] {DispositionFileStatusTypes.DRAFT, true},
                new object[] {DispositionFileStatusTypes.HOLD, true},
                new object[] {DispositionFileStatusTypes.ARCHIVED, false},
                new object[] {DispositionFileStatusTypes.CANCELLED, false},
                new object[] {DispositionFileStatusTypes.COMPLETE, false},
            };

        [Theory]
        [MemberData(nameof(CanEditPropertiesParameters))]
        public void CanEditProperties_Parametrized(DispositionFileStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new DispositionStatusSolver();

            // Act
            var result = solver.CanEditDetails(status);

            // Assert            
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditOrDeleteValuesOffersSalesParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {DispositionFileStatusTypes.ACTIVE, true},
                new object[] {DispositionFileStatusTypes.DRAFT, true},
                new object[] {DispositionFileStatusTypes.HOLD, true},
                new object[] {DispositionFileStatusTypes.ARCHIVED, false},
                new object[] {DispositionFileStatusTypes.CANCELLED, false},
                new object[] {DispositionFileStatusTypes.COMPLETE, false},
            };

        [Theory]
        [MemberData(nameof(CanEditOrDeleteValuesOffersSalesParameters))]
        public void CanEditOrDeleteValuesOffersSales_Parametrized(DispositionFileStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new DispositionStatusSolver();

            // Act
            var result = solver.CanEditDetails(status);

            // Assert            
            Assert.Equal(expectedResult, result);
        }
        #endregion
    }
}
