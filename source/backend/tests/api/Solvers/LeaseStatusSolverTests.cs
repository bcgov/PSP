using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseStatusSolverTests
    {
        #region Tests
        public static IEnumerable<object[]> CanEditDetailsParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {LeaseStatusTypes.ACTIVE, true},
                new object[] {LeaseStatusTypes.INACTIVE, true},
                new object[] {LeaseStatusTypes.DUPLICATE, false},
                new object[] {LeaseStatusTypes.DISCARD, false},
                new object[] {LeaseStatusTypes.TERMINATED, false},
                new object[] {LeaseStatusTypes.EXPIRED, false},
                new object[] {LeaseStatusTypes.ARCHIVED, false},
            };

        [Theory]
        [MemberData(nameof(CanEditDetailsParameters))]
        public void CanEditDetails_Parametrized(LeaseStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new LeaseStatusSolver();

            // Act
            var result = solver.CanEditDetails(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditPropertiesParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {LeaseStatusTypes.ACTIVE, true},
                new object[] {LeaseStatusTypes.INACTIVE, true},
                new object[] {LeaseStatusTypes.DUPLICATE, false},
                new object[] {LeaseStatusTypes.DISCARD, false},
                new object[] {LeaseStatusTypes.TERMINATED, false},
                new object[] {LeaseStatusTypes.EXPIRED, false},
                new object[] {LeaseStatusTypes.ARCHIVED, false},
            };

        [Theory]
        [MemberData(nameof(CanEditPropertiesParameters))]
        public void CanEditProperties_Parametrized(LeaseStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new LeaseStatusSolver();

            // Act
            var result = solver.CanEditProperties(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditOrDeleteCompensationParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {LeaseStatusTypes.ACTIVE, true},
                new object[] {LeaseStatusTypes.INACTIVE, true},
                new object[] {LeaseStatusTypes.DUPLICATE, false},
                new object[] {LeaseStatusTypes.DISCARD, false},
                new object[] {LeaseStatusTypes.TERMINATED, false},
                new object[] {LeaseStatusTypes.EXPIRED, false},
                new object[] {LeaseStatusTypes.ARCHIVED, false},
            };

        [Theory]
        [MemberData(nameof(CanEditOrDeleteCompensationParameters))]
        public void CanEditOrDeleteCompensation_Parametrized(LeaseStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new LeaseStatusSolver();

            // Act
            var result = solver.CanEditProperties(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditOrDeleteConsultationParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {LeaseStatusTypes.ACTIVE, true},
                new object[] {LeaseStatusTypes.INACTIVE, true},
                new object[] {LeaseStatusTypes.DUPLICATE, false},
                new object[] {LeaseStatusTypes.DISCARD, false},
                new object[] {LeaseStatusTypes.TERMINATED, false},
                new object[] {LeaseStatusTypes.EXPIRED, false},
                new object[] {LeaseStatusTypes.ARCHIVED, false},
            };

        [Theory]
        [MemberData(nameof(CanEditOrDeleteConsultationParameters))]
        public void CanEditOrDeleteConsultation_Parametrized(LeaseStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new LeaseStatusSolver();

            // Act
            var result = solver.CanEditOrDeleteConsultation(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditChecklistsParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {LeaseStatusTypes.ACTIVE, true},
                new object[] {LeaseStatusTypes.INACTIVE, true},
                new object[] {LeaseStatusTypes.DUPLICATE, false},
                new object[] {LeaseStatusTypes.DISCARD, false},
                new object[] {LeaseStatusTypes.TERMINATED, false},
                new object[] {LeaseStatusTypes.EXPIRED, false},
                new object[] {LeaseStatusTypes.ARCHIVED, false},
            };

        [Theory]
        [MemberData(nameof(CanEditChecklistsParameters))]
        public void CanEditChecklists_Parametrized(LeaseStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new LeaseStatusSolver();

            // Act
            var result = solver.CanEditChecklists(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditStakeholdersParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {LeaseStatusTypes.ACTIVE, true},
                new object[] {LeaseStatusTypes.INACTIVE, true},
                new object[] {LeaseStatusTypes.DUPLICATE, false},
                new object[] {LeaseStatusTypes.DISCARD, false},
                new object[] {LeaseStatusTypes.TERMINATED, false},
                new object[] {LeaseStatusTypes.EXPIRED, false},
                new object[] {LeaseStatusTypes.ARCHIVED, false},
            };

        [Theory]
        [MemberData(nameof(CanEditStakeholdersParameters))]
        public void CanEditStakeholders_Parametrized(LeaseStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new LeaseStatusSolver();

            // Act
            var result = solver.CanEditStakeholders(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditImprovementsParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {LeaseStatusTypes.ACTIVE, true},
                new object[] {LeaseStatusTypes.INACTIVE, true},
                new object[] {LeaseStatusTypes.DUPLICATE, false},
                new object[] {LeaseStatusTypes.DISCARD, false},
                new object[] {LeaseStatusTypes.TERMINATED, false},
                new object[] {LeaseStatusTypes.EXPIRED, false},
                new object[] {LeaseStatusTypes.ARCHIVED, false},
            };

        [Theory]
        [MemberData(nameof(CanEditImprovementsParameters))]
        public void CanEditImprovements_Parametrized(LeaseStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new LeaseStatusSolver();

            // Act
            var result = solver.CanEditImprovements(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditInsuranceParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {LeaseStatusTypes.ACTIVE, true},
                new object[] {LeaseStatusTypes.INACTIVE, true},
                new object[] {LeaseStatusTypes.DUPLICATE, false},
                new object[] {LeaseStatusTypes.DISCARD, false},
                new object[] {LeaseStatusTypes.TERMINATED, false},
                new object[] {LeaseStatusTypes.EXPIRED, false},
                new object[] {LeaseStatusTypes.ARCHIVED, false},
            };

        [Theory]
        [MemberData(nameof(CanEditInsuranceParameters))]
        public void CanEditInsurance_Parametrized(LeaseStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new LeaseStatusSolver();

            // Act
            var result = solver.CanEditInsurance(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditDepositsParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {LeaseStatusTypes.ACTIVE, true},
                new object[] {LeaseStatusTypes.INACTIVE, true},
                new object[] {LeaseStatusTypes.DUPLICATE, false},
                new object[] {LeaseStatusTypes.DISCARD, false},
                new object[] {LeaseStatusTypes.TERMINATED, false},
                new object[] {LeaseStatusTypes.EXPIRED, false},
                new object[] {LeaseStatusTypes.ARCHIVED, false},
            };

        [Theory]
        [MemberData(nameof(CanEditDepositsParameters))]
        public void CanEditDeposits_Parametrized(LeaseStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new LeaseStatusSolver();

            // Act
            var result = solver.CanEditDeposits(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditPaymentsParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {LeaseStatusTypes.ACTIVE, true},
                new object[] {LeaseStatusTypes.INACTIVE, true},
                new object[] {LeaseStatusTypes.DUPLICATE, false},
                new object[] {LeaseStatusTypes.DISCARD, false},
                new object[] {LeaseStatusTypes.TERMINATED, false},
                new object[] {LeaseStatusTypes.EXPIRED, false},
                new object[] {LeaseStatusTypes.ARCHIVED, false},
            };

        [Theory]
        [MemberData(nameof(CanEditPaymentsParameters))]
        public void CanEditPayments_Parametrized(LeaseStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new LeaseStatusSolver();

            // Act
            var result = solver.CanEditPayments(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }
        #endregion
    }
}
