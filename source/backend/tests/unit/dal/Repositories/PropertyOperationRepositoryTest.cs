using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "propertyoperation")]
    [ExcludeFromCodeCoverage]
    public class PropertyOperationRepositoryTest
    {
        private readonly TestHelper _helper;

        #region Constructors
        public PropertyOperationRepositoryTest()
        {
            _helper = new TestHelper();
        }
        #endregion

        private PropertyOperationRepository CreateRepositoryWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.CreateRepository<PropertyOperationRepository>(user);
        }

        #region Tests

        #region
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionAdd);
            var propertyOperation = EntityHelper.CreatePropertyOperation();

            var repository = helper.CreateRepository<PropertyOperationRepository>(user);

            var mockSequenceRepo = new Mock<ISequenceRepository>();
            mockSequenceRepo.Setup(x => x.GetNextSequenceValue(It.IsAny<string>())).Returns(100);

            // Act
            var result = repository.AddRange(new List<PimsPropertyOperation>() { propertyOperation });

            // Assert

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsPropertyOperation>>();
            result.Count().Should().Be(1);
            result.FirstOrDefault().PropertyOperationNo.Equals("100");
            result.FirstOrDefault().OperationDt.Should().BeWithin(TimeSpan.FromSeconds(1));
        }
        #endregion

        #endregion
    }
}
