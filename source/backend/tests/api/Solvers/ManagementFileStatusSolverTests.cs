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
                        new object[] { ManagementFileStatusTypes.HOLD, true},
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
    }
}
