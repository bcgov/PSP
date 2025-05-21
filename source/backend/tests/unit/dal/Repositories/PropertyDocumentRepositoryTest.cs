using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    public class PropertyDocumentRepositoryTest
    {
        private readonly TestHelper _helper;

        public PropertyDocumentRepositoryTest()
        {
            this._helper = new TestHelper();
        }

        private PropertyDocumentRepository CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.CreateRepository<PropertyDocumentRepository>(user);
        }

        [Fact]
        public void AddPropertyDocument_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentAdd, Permissions.PropertyEdit);

            // Act
            var result = repository.AddDocument(new PimsPropertyDocument());

            // Assert
            result.PropertyDocumentId.Should().Be(1);
        }

        [Fact]
        public void GetAllByPropertyFile_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentView, Permissions.PropertyView);

            var document = new PimsDocument() { FileName = "test doc", DocumentStatusTypeCodeNavigation = new PimsDocumentStatusType() { DocumentStatusTypeCode = "test", DbCreateUserid = "create user", DbLastUpdateUserid = "last user", Description = "description" }, DocumentType = new PimsDocumentTyp() { DocumentType = "type", DocumentTypeDescription = "description" } };
            var projectDocument = new PimsPropertyDocument() { Document = document };
            var pimsPropertyFile = new PimsProperty() { PropertyStatusTypeCode = "status", PropertyDataSourceTypeCode = "test_code", PropertyTypeCode = "test_type_code", SurplusDeclarationTypeCode = "test_surp_code", PimsPropertyDocuments = new List<PimsPropertyDocument>() { projectDocument } };
            _helper.AddAndSaveChanges(pimsPropertyFile);

            // Act
            var result = repository.GetAllByParentId(projectDocument.PropertyId);

            // Assert
            result.FirstOrDefault().Internal_Id.Should().Be(1);
        }

        [Fact]
        public void DeletePropertyDocument_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentDelete, Permissions.PropertyEdit);

            // Act
            var result = repository.DeleteDocument(new PimsPropertyDocument());

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void DeletePropertyDocument_Null()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentDelete, Permissions.PropertyEdit);

            // Act
            Action act = () => repository.DeleteDocument(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
