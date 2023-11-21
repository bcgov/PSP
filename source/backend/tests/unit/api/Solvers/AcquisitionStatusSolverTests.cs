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
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class AcquisitionStatusSolverTests
    {
        #region Tests
        public static IEnumerable<object[]> CanEditDetailsParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {AcqusitionStatusTypes.ACTIVE, true},
                new object[] {AcqusitionStatusTypes.DRAFT, true},
                new object[] {AcqusitionStatusTypes.ARCHIV, false},
                new object[] {AcqusitionStatusTypes.CANCEL, false},
                new object[] {AcqusitionStatusTypes.CLOSED, false},
                new object[] {AcqusitionStatusTypes.COMPLT, false},
                new object[] {AcqusitionStatusTypes.HOLD, false},
            };

        [Theory]
        [MemberData(nameof(CanEditDetailsParameters))]
        public void CanEditDetails_Parametrized(AcqusitionStatusTypes? status, bool expectedResult)
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
                new object[] {AcqusitionStatusTypes.ACTIVE, true},
                new object[] {AcqusitionStatusTypes.DRAFT, true},
                new object[] {AcqusitionStatusTypes.ARCHIV, true},
                new object[] {AcqusitionStatusTypes.CANCEL, true},
                new object[] {AcqusitionStatusTypes.CLOSED, true},
                new object[] {AcqusitionStatusTypes.COMPLT, true},
                new object[] {AcqusitionStatusTypes.HOLD, true},
            };

        [Theory]
        [MemberData(nameof(CanEditChecklistsParameters))]
        public void CanEditChecklists_Parametrized(AcqusitionStatusTypes? status, bool expectedResult)
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
                new object[] {AcqusitionStatusTypes.ACTIVE, null, true},
                new object[] {AcqusitionStatusTypes.ACTIVE, AgreementStatusTypes.CANCELLED, true},
                new object[] {AcqusitionStatusTypes.ACTIVE, AgreementStatusTypes.DRAFT, true},
                new object[] {AcqusitionStatusTypes.ACTIVE, AgreementStatusTypes.FINAL, true},
                new object[] {AcqusitionStatusTypes.DRAFT,  null, true},
                new object[] {AcqusitionStatusTypes.DRAFT, AgreementStatusTypes.CANCELLED, true},
                new object[] {AcqusitionStatusTypes.DRAFT, AgreementStatusTypes.DRAFT, true},
                new object[] {AcqusitionStatusTypes.DRAFT, AgreementStatusTypes.FINAL, true},
                new object[] {AcqusitionStatusTypes.ARCHIV, null, true},
                new object[] {AcqusitionStatusTypes.ARCHIV, AgreementStatusTypes.CANCELLED, true},
                new object[] {AcqusitionStatusTypes.ARCHIV, AgreementStatusTypes.DRAFT, true},
                new object[] {AcqusitionStatusTypes.ARCHIV, AgreementStatusTypes.FINAL, false},
                new object[] {AcqusitionStatusTypes.CANCEL, null, true},
                new object[] {AcqusitionStatusTypes.CANCEL, AgreementStatusTypes.CANCELLED, true},
                new object[] {AcqusitionStatusTypes.CANCEL, AgreementStatusTypes.DRAFT, true},
                new object[] {AcqusitionStatusTypes.CANCEL, AgreementStatusTypes.FINAL, false},
                new object[] {AcqusitionStatusTypes.CLOSED, null, true},
                new object[] {AcqusitionStatusTypes.CLOSED, AgreementStatusTypes.CANCELLED, true},
                new object[] {AcqusitionStatusTypes.CLOSED, AgreementStatusTypes.DRAFT, true},
                new object[] {AcqusitionStatusTypes.CLOSED, AgreementStatusTypes.FINAL, false},
                new object[] {AcqusitionStatusTypes.COMPLT, null, true},
                new object[] {AcqusitionStatusTypes.COMPLT, AgreementStatusTypes.CANCELLED, true},
                new object[] {AcqusitionStatusTypes.COMPLT, AgreementStatusTypes.DRAFT, true},
                new object[] {AcqusitionStatusTypes.COMPLT, AgreementStatusTypes.FINAL, false},
                new object[] {AcqusitionStatusTypes.HOLD, null, true},
                new object[] {AcqusitionStatusTypes.HOLD, AgreementStatusTypes.CANCELLED, true},
                new object[] {AcqusitionStatusTypes.HOLD, AgreementStatusTypes.DRAFT, true},
                new object[] {AcqusitionStatusTypes.HOLD, AgreementStatusTypes.FINAL, false},
            };

        [Theory]
        [MemberData(nameof(CanEditOrDeleteAgreementParameters))]
        public void CanEditOrDeleteAgreements_Parametrized(AcqusitionStatusTypes? acquisitionStatus, AgreementStatusTypes? agreementStatus, bool expectedResult)
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
                new object[] {AcqusitionStatusTypes.ACTIVE, null, true},
                new object[] {AcqusitionStatusTypes.ACTIVE, true, true},
                new object[] {AcqusitionStatusTypes.ACTIVE, false, true},
                new object[] {AcqusitionStatusTypes.DRAFT,  null, true},
                new object[] {AcqusitionStatusTypes.DRAFT, true, true},
                new object[] {AcqusitionStatusTypes.DRAFT, false, true},
                new object[] {AcqusitionStatusTypes.ARCHIV, null, true},
                new object[] {AcqusitionStatusTypes.ARCHIV, true, true},
                new object[] {AcqusitionStatusTypes.ARCHIV, false, false},
                new object[] {AcqusitionStatusTypes.CANCEL, null, true},
                new object[] {AcqusitionStatusTypes.CANCEL, true, true},
                new object[] {AcqusitionStatusTypes.CANCEL, false, false},
                new object[] {AcqusitionStatusTypes.CLOSED, null, true},
                new object[] {AcqusitionStatusTypes.CLOSED, true, true},
                new object[] {AcqusitionStatusTypes.CLOSED, false, false},
                new object[] {AcqusitionStatusTypes.COMPLT, null, true},
                new object[] {AcqusitionStatusTypes.COMPLT, true, true},
                new object[] {AcqusitionStatusTypes.COMPLT, false, false},
                new object[] {AcqusitionStatusTypes.HOLD, null, true},
                new object[] {AcqusitionStatusTypes.HOLD, true, true},
                new object[] {AcqusitionStatusTypes.HOLD, false, false},
            };

        [Theory]
        [MemberData(nameof(CanEditCompensationsParameters))]
        public void CanEditCompensations_Parametrized(AcqusitionStatusTypes? status, bool? isDraftCompensation, bool expectedResult)
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
                new object[] {AcqusitionStatusTypes.ACTIVE, true},
                new object[] {AcqusitionStatusTypes.DRAFT, true},
                new object[] {AcqusitionStatusTypes.ARCHIV, false},
                new object[] {AcqusitionStatusTypes.CANCEL, false},
                new object[] {AcqusitionStatusTypes.CLOSED, false},
                new object[] {AcqusitionStatusTypes.COMPLT, false},
                new object[] {AcqusitionStatusTypes.HOLD, false},
          };

        [Theory]
        [MemberData(nameof(CanEditTakesParameters))]
        public void CanEditTakes_Parametrized(AcqusitionStatusTypes? status, bool expectedResult)
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
                new object[] {AcqusitionStatusTypes.ACTIVE, true},
                new object[] {AcqusitionStatusTypes.DRAFT, true},
                new object[] {AcqusitionStatusTypes.ARCHIV, true},
                new object[] {AcqusitionStatusTypes.CANCEL, true},
                new object[] {AcqusitionStatusTypes.CLOSED, true},
                new object[] {AcqusitionStatusTypes.COMPLT, true},
                new object[] {AcqusitionStatusTypes.HOLD, true},
        };

        [Theory]
        [MemberData(nameof(CanEditStakeholdersParameters))]
        public void CanEditStakeholders_Parametrized(AcqusitionStatusTypes? status, bool expectedResult)
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