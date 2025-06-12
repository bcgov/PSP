using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "take")]
    [ExcludeFromCodeCoverage]
    public class TakeInteractionSolverTest
    {
        // xUnit.net creates a new instance of the test class for every test that is run,
        // so any code which is placed into the constructor of the test class will be run for every single test.
        private readonly TestHelper _helper;

        public TakeInteractionSolverTest()
        {
            this._helper = new TestHelper();
        }

        public static IEnumerable<object[]> takesTestParameters = new List<object[]>() {

            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Section 66" } }, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Crown Grant" } }, true },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Transfer Admin" } }, true },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true, IsAcquiredForInventory=false }}, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true, IsAcquiredForInventory=true }}, true },
        }.ToArray();

        [Theory]
        [MemberData(nameof(takesTestParameters))]
        public void Update_Success_Transfer_MultipleTakes_Core(List<PimsTake> takes, bool expectedIsOwned)
        {
            // Arrange
            var service = this._helper.Create<TakeInteractionSolver>();

            // Act
            var result = service.ResultsInOwnedProperty(takes);

            // Assert
            Assert.Equal(result, expectedIsOwned);
        }
    }
}
