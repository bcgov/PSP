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
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class AcquisitionStatusSolverTests
    {
        #region Tests
        public static IEnumerable<object[]> CanEditDetailsParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {AcquisitionStatusTypes.ACTIVE, true},
                new object[] {AcquisitionStatusTypes.DRAFT, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, false},
                new object[] {AcquisitionStatusTypes.CANCEL, false},
                new object[] {AcquisitionStatusTypes.CLOSED, false},
                new object[] {AcquisitionStatusTypes.COMPLT, false},
                new object[] {AcquisitionStatusTypes.HOLD, false},
            };

        [Theory]
        [MemberData(nameof(CanEditDetailsParameters))]
        public void CanEditDetails_Parametrized(AcquisitionStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new AcquisitionStatusSolver();

            // Act
            var result = solver.CanEditDetails(status);

            // Assert            
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditChecklistsParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {AcquisitionStatusTypes.ACTIVE, true},
                new object[] {AcquisitionStatusTypes.DRAFT, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, true},
                new object[] {AcquisitionStatusTypes.CANCEL, true},
                new object[] {AcquisitionStatusTypes.CLOSED, true},
                new object[] {AcquisitionStatusTypes.COMPLT, true},
                new object[] {AcquisitionStatusTypes.HOLD, true},
            };

        [Theory]
        [MemberData(nameof(CanEditChecklistsParameters))]
        public void CanEditChecklists_Parametrized(AcquisitionStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new AcquisitionStatusSolver();

            // Act
            var result = solver.CanEditChecklists(status);

            // Assert            
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditOrDeleteAgreementParameters =>
            new List<object[]>
            {
                new object[] {null, null, false},
                new object[] {AcquisitionStatusTypes.ACTIVE, null, true},
                new object[] {AcquisitionStatusTypes.ACTIVE, AgreementStatusTypes.CANCELLED, true},
                new object[] {AcquisitionStatusTypes.ACTIVE, AgreementStatusTypes.DRAFT, true},
                new object[] {AcquisitionStatusTypes.ACTIVE, AgreementStatusTypes.FINAL, true},
                new object[] {AcquisitionStatusTypes.DRAFT,  null, true},
                new object[] {AcquisitionStatusTypes.DRAFT, AgreementStatusTypes.CANCELLED, true},
                new object[] {AcquisitionStatusTypes.DRAFT, AgreementStatusTypes.DRAFT, true},
                new object[] {AcquisitionStatusTypes.DRAFT, AgreementStatusTypes.FINAL, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, null, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, AgreementStatusTypes.CANCELLED, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, AgreementStatusTypes.DRAFT, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, AgreementStatusTypes.FINAL, false},
                new object[] {AcquisitionStatusTypes.CANCEL, null, true},
                new object[] {AcquisitionStatusTypes.CANCEL, AgreementStatusTypes.CANCELLED, true},
                new object[] {AcquisitionStatusTypes.CANCEL, AgreementStatusTypes.DRAFT, true},
                new object[] {AcquisitionStatusTypes.CANCEL, AgreementStatusTypes.FINAL, false},
                new object[] {AcquisitionStatusTypes.CLOSED, null, true},
                new object[] {AcquisitionStatusTypes.CLOSED, AgreementStatusTypes.CANCELLED, true},
                new object[] {AcquisitionStatusTypes.CLOSED, AgreementStatusTypes.DRAFT, true},
                new object[] {AcquisitionStatusTypes.CLOSED, AgreementStatusTypes.FINAL, false},
                new object[] {AcquisitionStatusTypes.COMPLT, null, true},
                new object[] {AcquisitionStatusTypes.COMPLT, AgreementStatusTypes.CANCELLED, true},
                new object[] {AcquisitionStatusTypes.COMPLT, AgreementStatusTypes.DRAFT, true},
                new object[] {AcquisitionStatusTypes.COMPLT, AgreementStatusTypes.FINAL, false},
                new object[] {AcquisitionStatusTypes.HOLD, null, true},
                new object[] {AcquisitionStatusTypes.HOLD, AgreementStatusTypes.CANCELLED, true},
                new object[] {AcquisitionStatusTypes.HOLD, AgreementStatusTypes.DRAFT, true},
                new object[] {AcquisitionStatusTypes.HOLD, AgreementStatusTypes.FINAL, false},
            };

        [Theory]
        [MemberData(nameof(CanEditOrDeleteAgreementParameters))]
        public void CanEditOrDeleteAgreements_Parametrized(AcquisitionStatusTypes? acquisitionStatus, AgreementStatusTypes? agreementStatus, bool expectedResult)
        {
            // Arrange
            var solver = new AcquisitionStatusSolver();

            // Act
            var result = solver.CanEditOrDeleteAgreement(acquisitionStatus, agreementStatus);

            // Assert            
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditCompensationsParameters =>
            new List<object[]>
            {
                new object[] {null, null, false},
                new object[] {AcquisitionStatusTypes.ACTIVE, null, true},
                new object[] {AcquisitionStatusTypes.ACTIVE, true, true},
                new object[] {AcquisitionStatusTypes.ACTIVE, false, true},
                new object[] {AcquisitionStatusTypes.DRAFT,  null, true},
                new object[] {AcquisitionStatusTypes.DRAFT, true, true},
                new object[] {AcquisitionStatusTypes.DRAFT, false, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, null, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, true, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, false, false},
                new object[] {AcquisitionStatusTypes.CANCEL, null, true},
                new object[] {AcquisitionStatusTypes.CANCEL, true, true},
                new object[] {AcquisitionStatusTypes.CANCEL, false, false},
                new object[] {AcquisitionStatusTypes.CLOSED, null, true},
                new object[] {AcquisitionStatusTypes.CLOSED, true, true},
                new object[] {AcquisitionStatusTypes.CLOSED, false, false},
                new object[] {AcquisitionStatusTypes.COMPLT, null, true},
                new object[] {AcquisitionStatusTypes.COMPLT, true, true},
                new object[] {AcquisitionStatusTypes.COMPLT, false, false},
                new object[] {AcquisitionStatusTypes.HOLD, null, true},
                new object[] {AcquisitionStatusTypes.HOLD, true, true},
                new object[] {AcquisitionStatusTypes.HOLD, false, false},
            };

        [Theory]
        [MemberData(nameof(CanEditCompensationsParameters))]
        public void CanEditCompensations_Parametrized(AcquisitionStatusTypes? status, bool? isDraftCompensation, bool expectedResult)
        {
            // Arrange
            var solver = new AcquisitionStatusSolver();

            // Act
            var result = solver.CanEditOrDeleteCompensation(status, isDraftCompensation);

            // Assert            
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditTakesParameters =>
          new List<object[]>
          {
                new object[] {null, false},
                new object[] {AcquisitionStatusTypes.ACTIVE, true},
                new object[] {AcquisitionStatusTypes.DRAFT, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, false},
                new object[] {AcquisitionStatusTypes.CANCEL, false},
                new object[] {AcquisitionStatusTypes.CLOSED, false},
                new object[] {AcquisitionStatusTypes.COMPLT, false},
                new object[] {AcquisitionStatusTypes.HOLD, false},
          };

        [Theory]
        [MemberData(nameof(CanEditTakesParameters))]
        public void CanEditTakes_Parametrized(AcquisitionStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new AcquisitionStatusSolver();

            // Act
            var result = solver.CanEditTakes(status);

            // Assert            
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditStakeholdersParameters =>
        new List<object[]>
        {
                new object[] {null, false},
                new object[] {AcquisitionStatusTypes.ACTIVE, true},
                new object[] {AcquisitionStatusTypes.DRAFT, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, true},
                new object[] {AcquisitionStatusTypes.CANCEL, true},
                new object[] {AcquisitionStatusTypes.CLOSED, true},
                new object[] {AcquisitionStatusTypes.COMPLT, true},
                new object[] {AcquisitionStatusTypes.HOLD, true},
        };

        [Theory]
        [MemberData(nameof(CanEditStakeholdersParameters))]
        public void CanEditStakeholders_Parametrized(AcquisitionStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new AcquisitionStatusSolver();

            // Act
            var result = solver.CanEditStakeholders(status);

            // Assert            
            Assert.Equal(expectedResult, result);
        }
        #endregion
    }
}
