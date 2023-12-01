using FluentAssertions;
using k8s.KubeConfigModels;
using Moq;
using NetTopologySuite.Utilities;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "disposition")]
    public class DispositionFileRepositoryTest
    {
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionAdd);
            var dispositionFile = EntityHelper.CreateDispositionFile();

            var repository = helper.CreateRepository<DispositionFileRepository>(user);

            var mockSequenceRepo = new Mock<ISequenceRepository>();
            mockSequenceRepo.Setup(x => x.GetNextSequenceValue(It.IsAny<string>())).Returns(100);

            // Act
            var result = repository.Add(dispositionFile);

            // Assert
            
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionFile>();
            result.DispositionFileId.Should().Be(1);
            result.FileNumber.Equals("D-100");
        }
    }
}
