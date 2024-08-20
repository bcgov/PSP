using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    public class PropertyActivityDocumentRepositoryTest
    {
        [Fact]
        public void AddPropertyActivity_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var propertyActivityFileDocument = new PimsPropertyActivityDocument();

            var repository = helper.CreateRepository<PropertyActivityDocumentRepository>(user);

            // Act
            var result = repository.AddPropertyActivityDocument(propertyActivityFileDocument);

            // Assert
            result.PropertyActivityDocumentId.Should().Be(1);
        }

        [Fact]
        public void GetAllByPropertyActivity_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var document = new PimsDocument() { DocumentStatusTypeCodeNavigation = new PimsDocumentStatusType() { DocumentStatusTypeCode = "test", Description = "Active", DbCreateUserid = "test", DbLastUpdateUserid = "test" }, DocumentType = new PimsDocumentTyp() { DocumentType = "IMAGE", DocumentTypeDescription = "Image", DbCreateUserid = "test", DbLastUpdateUserid = "test" }, FileName = "test.txt" };
            var propertyActivityFileDocument = new PimsPropertyActivityDocument() { Document = document };
            PimsPropertyActivity pimsPropertyActivity = new PimsPropertyActivity() { PimsPropertyActivityDocuments = new List<PimsPropertyActivityDocument>() { propertyActivityFileDocument },
                PropMgmtActivityStatusTypeCode = "ACTIVE",
                PropMgmtActivitySubtypeCode = "ACTIVE",
                PropMgmtActivityTypeCode = "test"
            };
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(pimsPropertyActivity);

            var repository = helper.CreateRepository<PropertyActivityDocumentRepository>(user);

            // Act
            var result = repository.GetAllByPropertyActivity(propertyActivityFileDocument.PimsPropertyActivityId);

            // Assert
            result.FirstOrDefault().Internal_Id.Should().Be(1);
        }

        [Fact]
        public void DeletePropertyActivity_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var propertyActivityFileDocument = new PimsPropertyActivityDocument();

            var repository = helper.CreateRepository<PropertyActivityDocumentRepository>(user);

            // Act
            var result = repository.DeletePropertyActivityDocument(propertyActivityFileDocument);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void DeletePropertyActivity_Null()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var repository = helper.CreateRepository<PropertyActivityDocumentRepository>(user);

            // Act
            Action act = () => repository.DeletePropertyActivityDocument(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
