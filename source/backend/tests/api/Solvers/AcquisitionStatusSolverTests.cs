using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;
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
                new object[] {AcquisitionStatusTypes.HOLD, true},
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
                new object[] {AcquisitionStatusTypes.ARCHIV, false},
                new object[] {AcquisitionStatusTypes.CANCEL, false},
                new object[] {AcquisitionStatusTypes.CLOSED, false},
                new object[] {AcquisitionStatusTypes.COMPLT, false},
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

        public static IEnumerable<object[]> CanEditPropertiesParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {AcquisitionStatusTypes.ACTIVE, true},
                new object[] {AcquisitionStatusTypes.DRAFT, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, false},
                new object[] {AcquisitionStatusTypes.CANCEL, false},
                new object[] {AcquisitionStatusTypes.CLOSED, false},
                new object[] {AcquisitionStatusTypes.COMPLT, false},
                new object[] {AcquisitionStatusTypes.HOLD, true},
            };

        [Theory]
        [MemberData(nameof(CanEditPropertiesParameters))]
        public void CanEditProperties_Parametrized(AcquisitionStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new AcquisitionStatusSolver();

            // Act
            var result = solver.CanEditProperties(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditOrDeleteAgreementParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {AcquisitionStatusTypes.ACTIVE, true},
                new object[] {AcquisitionStatusTypes.DRAFT, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, false},
                new object[] {AcquisitionStatusTypes.CANCEL, false},
                new object[] {AcquisitionStatusTypes.CLOSED, false},
                new object[] {AcquisitionStatusTypes.COMPLT, false},
                new object[] {AcquisitionStatusTypes.HOLD, true},
            };

        [Theory]
        [MemberData(nameof(CanEditOrDeleteAgreementParameters))]
        public void CanEditOrDeleteAgreements_Parametrized(AcquisitionStatusTypes? acquisitionStatus, bool expectedResult)
        {
            // Arrange
            var solver = new AcquisitionStatusSolver();

            // Act
            var result = solver.CanEditOrDeleteAgreement(acquisitionStatus);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditCompensationsParameters =>
            new List<object[]>
            {
                new object[] {null, null, null, false},
                new object[] {AcquisitionStatusTypes.ACTIVE, null, null, false},
                new object[] {AcquisitionStatusTypes.ACTIVE, null, false, false},
                new object[] {AcquisitionStatusTypes.ACTIVE, null, true, true},
                new object[] {AcquisitionStatusTypes.ACTIVE, true, null, true},
                new object[] {AcquisitionStatusTypes.ACTIVE, false, null, false},
                new object[] {AcquisitionStatusTypes.ACTIVE, true, true, true},
                new object[] {AcquisitionStatusTypes.ACTIVE, false, false, false},
                new object[] {AcquisitionStatusTypes.ACTIVE, true, false, true},
                new object[] {AcquisitionStatusTypes.ACTIVE, false, true, true},
                new object[] {AcquisitionStatusTypes.DRAFT, null, null, false},
                new object[] {AcquisitionStatusTypes.DRAFT, null, false, false},
                new object[] {AcquisitionStatusTypes.DRAFT, null, true, true},
                new object[] {AcquisitionStatusTypes.DRAFT, true, null, true},
                new object[] {AcquisitionStatusTypes.DRAFT, false, null, false},
                new object[] {AcquisitionStatusTypes.DRAFT, true, true, true},
                new object[] {AcquisitionStatusTypes.DRAFT, false, false, false},
                new object[] {AcquisitionStatusTypes.DRAFT, true, false, true},
                new object[] {AcquisitionStatusTypes.DRAFT, false, true, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, null, null, false},
                new object[] {AcquisitionStatusTypes.ARCHIV, null, false, false},
                new object[] {AcquisitionStatusTypes.ARCHIV, null, true, false},
                new object[] {AcquisitionStatusTypes.ARCHIV, true, null, false},
                new object[] {AcquisitionStatusTypes.ARCHIV, false, null, false},
                new object[] {AcquisitionStatusTypes.ARCHIV, true, true, false},
                new object[] {AcquisitionStatusTypes.ARCHIV, false, false, false},
                new object[] {AcquisitionStatusTypes.ARCHIV, true, false, false},
                new object[] {AcquisitionStatusTypes.ARCHIV, false, true, false},
                new object[] {AcquisitionStatusTypes.CANCEL, null, null, false},
                new object[] {AcquisitionStatusTypes.CANCEL, null, false, false},
                new object[] {AcquisitionStatusTypes.CANCEL, null, true, false},
                new object[] {AcquisitionStatusTypes.CANCEL, true, null, false},
                new object[] {AcquisitionStatusTypes.CANCEL, false, null, false},
                new object[] {AcquisitionStatusTypes.CANCEL, true, true, false},
                new object[] {AcquisitionStatusTypes.CANCEL, false, false, false},
                new object[] {AcquisitionStatusTypes.CANCEL, true, false, false},
                new object[] {AcquisitionStatusTypes.CANCEL, false, true, false},
                new object[] {AcquisitionStatusTypes.CLOSED, null, null, false},
                new object[] {AcquisitionStatusTypes.CLOSED, null, false, false},
                new object[] {AcquisitionStatusTypes.CLOSED, null, true, false},
                new object[] {AcquisitionStatusTypes.CLOSED, true, null, false},
                new object[] {AcquisitionStatusTypes.CLOSED, false, null, false},
                new object[] {AcquisitionStatusTypes.CLOSED, true, true, false},
                new object[] {AcquisitionStatusTypes.CLOSED, false, false, false},
                new object[] {AcquisitionStatusTypes.CLOSED, true, false, false},
                new object[] {AcquisitionStatusTypes.CLOSED, false, true, false},
                new object[] {AcquisitionStatusTypes.COMPLT, null, null, false},
                new object[] {AcquisitionStatusTypes.COMPLT, null, false, false},
                new object[] {AcquisitionStatusTypes.COMPLT, null, true, false},
                new object[] {AcquisitionStatusTypes.COMPLT, true, null, false},
                new object[] {AcquisitionStatusTypes.COMPLT, false, null, false},
                new object[] {AcquisitionStatusTypes.COMPLT, true, true, false},
                new object[] {AcquisitionStatusTypes.COMPLT, false, false, false},
                new object[] {AcquisitionStatusTypes.COMPLT, true, false, false},
                new object[] {AcquisitionStatusTypes.COMPLT, false, true, false},
                new object[] {AcquisitionStatusTypes.HOLD, null, null, false},
                new object[] {AcquisitionStatusTypes.HOLD, null, false, false},
                new object[] {AcquisitionStatusTypes.HOLD, null, true, true},
                new object[] {AcquisitionStatusTypes.HOLD, true, null, true},
                new object[] {AcquisitionStatusTypes.HOLD, false, null, false},
                new object[] {AcquisitionStatusTypes.HOLD, true, true, true},
                new object[] {AcquisitionStatusTypes.HOLD, false, false, false},
                new object[] {AcquisitionStatusTypes.HOLD, true, false, true},
                new object[] {AcquisitionStatusTypes.HOLD, false, true, true},
            };

        [Theory]
        [MemberData(nameof(CanEditCompensationsParameters))]
        public void CanEditCompensations_Parametrized(AcquisitionStatusTypes? status, bool? isDraftCompensation, bool? isAdmin, bool expectedResult)
        {
            // Arrange
            var solver = new AcquisitionStatusSolver();

            // Act
            var result = solver.CanEditOrDeleteCompensation(status, isDraftCompensation, isAdmin);

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
                new object[] {AcquisitionStatusTypes.ARCHIV, false},
                new object[] {AcquisitionStatusTypes.CANCEL, false},
                new object[] {AcquisitionStatusTypes.CLOSED, false},
                new object[] {AcquisitionStatusTypes.COMPLT, false},
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

        public static IEnumerable<object[]> CanEditExpropriationParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {AcquisitionStatusTypes.ACTIVE, true},
                new object[] {AcquisitionStatusTypes.DRAFT, true},
                new object[] {AcquisitionStatusTypes.ARCHIV, false},
                new object[] {AcquisitionStatusTypes.CANCEL, false},
                new object[] {AcquisitionStatusTypes.CLOSED, false},
                new object[] {AcquisitionStatusTypes.COMPLT, false},
                new object[] {AcquisitionStatusTypes.HOLD, true},
            };

        [Theory]
        [MemberData(nameof(CanEditExpropriationParameters))]
        public void CanEditExpropriation_Parametrized(AcquisitionStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new AcquisitionStatusSolver();

            // Act
            var result = solver.CanEditExpropriation(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }
        #endregion
    }
}
