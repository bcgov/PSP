using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MapsterMapper;
using Moq;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Api.Models.Concepts.Lease;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Core.Security;
using Xunit;

namespace Pims.Api.Test.Controllers.Lease
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseImprovementControllerTest
    {
        private Mock<ILeaseService> _repository;
        private PropertyImprovementController _controller;
        private IMapper _mapper;
        private TestHelper _helper;

        public LeaseImprovementControllerTest()
        {
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<PropertyImprovementController>(Permissions.LeaseView);
            this._mapper = this._helper.GetService<IMapper>();
            this._repository = this._helper.GetService<Mock<ILeaseService>>();
        }

        #region Tests
        #region UpdateImprovements
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void UpdateImprovements_All_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            lease.PimsPropertyImprovements = new List<Pims.Dal.Entities.PimsPropertyImprovement>() { new Dal.Entities.PimsPropertyImprovement() { Internal_Id = 1 } };

            this._repository.Setup(m => m.UpdateImprovementsByLeaseId(It.IsAny<long>(), It.IsAny<ICollection<Pims.Dal.Entities.PimsPropertyImprovement>>())).Returns(lease.PimsPropertyImprovements);

            // Act
            var result = this._controller.UpdateImprovements(lease.Internal_Id, this._mapper.Map<IEnumerable<PropertyImprovementModel>>(lease.PimsPropertyImprovements));

            // Assert
            this._repository.Verify(m => m.UpdateImprovementsByLeaseId(It.IsAny<long>(), It.IsAny<ICollection<Pims.Dal.Entities.PimsPropertyImprovement>>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
