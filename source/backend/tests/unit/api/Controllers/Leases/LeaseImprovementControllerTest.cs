using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Acquisition.Controllers;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Test.Controllers.Lease
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseImprovementControllerTest
    {
        private Mock<ILeaseRepository> _repository;
        private PropertyImprovementController _controller;
        private IMapper _mapper;
        private TestHelper _helper;

        public LeaseImprovementControllerTest()
        {
            _helper = new TestHelper();
            _controller = _helper.CreateController<PropertyImprovementController>(Permissions.LeaseView);
            _mapper = _helper.GetService<IMapper>();
            _repository = _helper.GetService<Mock<ILeaseRepository>>();
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

            _repository.Setup(m => m.UpdateLeaseImprovements(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ICollection<Pims.Dal.Entities.PimsPropertyImprovement>>())).Returns(lease);

            // Act
            var result = _controller.UpdateImprovements(lease.Internal_Id, _mapper.Map<Model.LeaseModel>(lease));

            // Assert
            _repository.Verify(m => m.UpdateLeaseImprovements(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ICollection<Pims.Dal.Entities.PimsPropertyImprovement>>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
