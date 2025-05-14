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
    public class LeaseDocumentRepositoryTest
    {
        private readonly TestHelper _helper;

        public LeaseDocumentRepositoryTest()
        {
            this._helper = new TestHelper();
        }

        private LeaseDocumentRepository CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.CreateRepository<LeaseDocumentRepository>(user);
        }

        [Fact]
        public void AddLeaseDocument_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentAdd, Permissions.LeaseEdit);

            // Act
            var result = repository.AddDocument(new PimsLeaseDocument());

            // Assert
            result.LeaseDocumentId.Should().Be(1);
        }

        [Fact]
        public void GetAllByLeaseFile_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentView, Permissions.LeaseView);

            var document = new PimsDocument() { FileName = "test doc", DocumentStatusTypeCodeNavigation = new PimsDocumentStatusType() { DocumentStatusTypeCode = "test", DbCreateUserid = "create user", DbLastUpdateUserid = "last user", Description = "description" }, DocumentType = new PimsDocumentTyp() { DocumentType = "type", DocumentTypeDescription = "description" } };
            var projectDocument = new PimsLeaseDocument() { Document = document };
            var pimsLeaseFile = new PimsLease() { LeaseStatusTypeCode = "status", LeaseLicenseTypeCode = "testcode", LeasePayRvblTypeCode = "test", LeaseProgramTypeCode = "test", PimsLeaseDocuments = new List<PimsLeaseDocument>() { projectDocument } };
            _helper.AddAndSaveChanges(pimsLeaseFile);

            // Act
            var result = repository.GetAllByParentId(projectDocument.LeaseId);

            // Assert
            result.FirstOrDefault().Internal_Id.Should().Be(1);
        }

        [Fact]
        public void DeleteLeaseDocument_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentDelete, Permissions.LeaseEdit);

            // Act
            var result = repository.DeleteDocument(new PimsLeaseDocument());

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void DeleteLeaseDocument_Null()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentDelete, Permissions.LeaseEdit);

            // Act
            Action act = () => repository.DeleteDocument(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
