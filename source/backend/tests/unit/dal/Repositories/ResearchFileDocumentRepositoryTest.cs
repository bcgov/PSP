using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    public class ResearchFileDocumentRepositoryTest
    {
        [Fact]
        public void AddResearch_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var ResearchFileDocument = new PimsResearchFileDocument();

            var repository = helper.CreateRepository<ResearchFileDocumentRepository>(user);

            // Act
            var result = repository.AddResearch(ResearchFileDocument);

            // Assert
            result.ResearchFileDocumentId.Should().Be(1);
        }

        [Fact]
        public void DeleteResearch_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var ResearchFileDocument = new PimsResearchFileDocument();

            var repository = helper.CreateRepository<ResearchFileDocumentRepository>(user);

            // Act
            var result = repository.DeleteResearch(ResearchFileDocument);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void DeleteResearch_Null()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var repository = helper.CreateRepository<ResearchFileDocumentRepository>(user);

            // Act
            Action act = () => repository.DeleteResearch(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
