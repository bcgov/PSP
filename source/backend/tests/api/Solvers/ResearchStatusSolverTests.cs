using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "research")]
    [ExcludeFromCodeCoverage]
    public class ResearchStatusSolverTests
    {
        #region Tests
        public static IEnumerable<object[]> CanEditDetailsParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {ResearchFileStatusTypes.ACTIVE, true},
                new object[] {ResearchFileStatusTypes.INACTIVE, true},
                new object[] {ResearchFileStatusTypes.CLOSED, false},
                new object[] {ResearchFileStatusTypes.ARCHIVED, false},
            };

        [Theory]
        [MemberData(nameof(CanEditDetailsParameters))]
        public void CanEditDetails_Parametrized(ResearchFileStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new ResearchStatusSolver();

            // Act
            var result = solver.CanEditDetails(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CanEditPropertiesParameters =>
            new List<object[]>
            {
                new object[] {null, false},
                new object[] {ResearchFileStatusTypes.ACTIVE, true},
                new object[] {ResearchFileStatusTypes.INACTIVE, true},
                new object[] {ResearchFileStatusTypes.CLOSED, false},
                new object[] {ResearchFileStatusTypes.ARCHIVED, false},
            };

        [Theory]
        [MemberData(nameof(CanEditPropertiesParameters))]
        public void CanEditProperties_Parametrized(ResearchFileStatusTypes? status, bool expectedResult)
        {
            // Arrange
            var solver = new ResearchStatusSolver();

            // Act
            var result = solver.CanEditProperties(status);

            // Assert
            Assert.Equal(expectedResult, result);
        }
        #endregion
    }
}
