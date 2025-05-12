using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "management")]
    [ExcludeFromCodeCoverage]
    public class ManagementFileDocumentRepositoryTest
    {
        private readonly TestHelper _helper;

        public ManagementFileDocumentRepositoryTest()
        {
            this._helper = new TestHelper();
        }

        private ManagementFileDocumentRepository CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.CreateRepository<ManagementFileDocumentRepository>(user);
        }

        [Fact]
        public void AddManagementFileDocument_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentAdd, Permissions.ManagementEdit);

            // Act
            var result = repository.AddDocument(new PimsManagementFileDocument());

            // Assert
            result.ManagementFileDocumentId.Should().Be(1);
        }

        [Fact]
        public void AddManagementFileDocument_Null()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentAdd, Permissions.ManagementEdit);

            // Act
            Action act = () => repository.AddDocument(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetAllByManagementFile_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentAdd, Permissions.ManagementEdit);

            var pimsManagementFile = EntityHelper.CreateManagementFile();
            var document = new PimsDocument()
            {
                DocumentStatusTypeCodeNavigation = new PimsDocumentStatusType() { DocumentStatusTypeCode = "test", Description = "Active", DbCreateUserid = "test", DbLastUpdateUserid = "test" },
                DocumentType = new PimsDocumentTyp() { DocumentType = "IMAGE", DocumentTypeDescription = "Image", DbCreateUserid = "test", DbLastUpdateUserid = "test" },
                FileName = "test.txt"
            };
            var managementFileDocument = new PimsManagementFileDocument() { Document = document, ManagementFileId = pimsManagementFile.ManagementFileId };
            pimsManagementFile.PimsManagementFileDocuments.Add(managementFileDocument);

            _helper.AddAndSaveChanges(pimsManagementFile);

            // Act
            var result = repository.GetAllByManagementFile(pimsManagementFile.ManagementFileId);

            // Assert
            result.FirstOrDefault().Internal_Id.Should().Be(managementFileDocument.Internal_Id);
        }

        [Fact]
        public void DeleteManagementFileDocument_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentAdd, Permissions.ManagementEdit);

            var pimsManagementFile = EntityHelper.CreateManagementFile();
            var document = new PimsDocument()
            {
                DocumentStatusTypeCodeNavigation = new PimsDocumentStatusType() { DocumentStatusTypeCode = "test", Description = "Active", DbCreateUserid = "test", DbLastUpdateUserid = "test" },
                DocumentType = new PimsDocumentTyp() { DocumentType = "IMAGE", DocumentTypeDescription = "Image", DbCreateUserid = "test", DbLastUpdateUserid = "test" },
                FileName = "test.txt"
            };
            var managementFileDocument = new PimsManagementFileDocument() { Document = document, ManagementFileId = pimsManagementFile.ManagementFileId };
            pimsManagementFile.PimsManagementFileDocuments.Add(managementFileDocument);

            var context = _helper.AddAndSaveChanges(pimsManagementFile);

            // Act
            context.ChangeTracker.Clear();
            var result = repository.DeleteDocument(managementFileDocument);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void DeleteManagementFile_Null()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentAdd, Permissions.ManagementEdit);

            // Act
            Action act = () => repository.DeleteDocument(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
