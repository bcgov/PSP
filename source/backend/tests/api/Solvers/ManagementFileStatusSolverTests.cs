using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;
using Xunit;

namespace Pims.Api.Test.Solvers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "management")]
    [ExcludeFromCodeCoverage]
    public class ManagementFileStatusSolverTests
    {
        public static IEnumerable<object[]> CanEditDetailsParameters =>
            new List<object[]>
            {
                        new object[] {null, false},
                        new object[] { ManagementFileStatusTypes.ACTIVE, true},
                        new object[] { ManagementFileStatusTypes.DRAFT, true},
                        new object[] { ManagementFileStatusTypes.THIRDRDPARTY, true},
                        new object[] { ManagementFileStatusTypes.ARCHIVED, false},
                        new object[] { ManagementFileStatusTypes.CANCELLED, false},
                        new object[] { ManagementFileStatusTypes.COMPLETE, false},
                        new object[] { ManagementFileStatusTypes.HOLD, false},
            };

        public static IEnumerable<object[]> IsAdminProtected =>
    new List<object[]>
    {
                        new object[] {null, false},
                        new object[] { ManagementFileStatusTypes.ACTIVE, false},
                        new object[] { ManagementFileStatusTypes.DRAFT, false},
                        new object[] { ManagementFileStatusTypes.THIRDRDPARTY, false},
                        new object[] { ManagementFileStatusTypes.CANCELLED, false},
                        new object[] { ManagementFileStatusTypes.HOLD, false},
                        new object[] { ManagementFileStatusTypes.ARCHIVED, true},
                        new object[] { ManagementFileStatusTypes.COMPLETE, true},
    };

        [Theory]
        [MemberData(nameof(CanEditDetailsParameters))]
        public void CanEditDetails_Parametrized(ManagementFileStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new ManagementFileStatusSolver();

            // Act
            var result = solver.CanEditDetails(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(IsAdminProtected))]
        public void IsAdminProtected_Parametrized(ManagementFileStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new ManagementFileStatusSolver();

            // Act
            var result = solver.IsAdminProtected(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
