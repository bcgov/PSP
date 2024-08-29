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
    public class DispositionFileDocumentRepositoryTest
    {
        private readonly TestHelper _helper;

        public DispositionFileDocumentRepositoryTest()
        {
            this._helper = new TestHelper();
        }

        private DispositionFileDocumentRepository CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.CreateRepository<DispositionFileDocumentRepository>(user);
        }

        [Fact]
        public void AddDispositionDocument_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentAdd, Permissions.DispositionEdit);

            // Act
            var result = repository.AddDispositionDocument(new PimsDispositionFileDocument());

            // Assert
            result.DispositionFileDocumentId.Should().Be(1);
        }

        [Fact]
        public void GetAllByDispositionFile_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentView, Permissions.DispositionView);

            var document = new PimsDocument() { FileName = "test doc", DocumentStatusTypeCodeNavigation = new PimsDocumentStatusType() { DocumentStatusTypeCode = "test", DbCreateUserid = "create user", DbLastUpdateUserid = "last user", Description = "description" }, DocumentType = new PimsDocumentTyp() { DocumentType = "type", DocumentTypeDescription = "description" } };
            var dispositionFileDocument = new PimsDispositionFileDocument() { Document = document };
            var pimsDispositionFile = new PimsDispositionFile() { DispositionStatusTypeCode ="status", DispositionFileStatusTypeCode = "status", DispositionTypeCode = "dspType", PimsDispositionFileDocuments = new List<PimsDispositionFileDocument>() { dispositionFileDocument } };
            _helper.AddAndSaveChanges(pimsDispositionFile);

            // Act
            var result = repository.GetAllByDispositionFile(dispositionFileDocument.DispositionFileId);

            // Assert
            result.FirstOrDefault().Internal_Id.Should().Be(1);
        }

        [Fact]
        public void DeleteDispositionDocument_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentDelete, Permissions.DispositionEdit);

            // Act
            var result = repository.DeleteDispositionDocument(new PimsDispositionFileDocument());

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void DeleteDispositionDocument_Null()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentDelete, Permissions.DispositionEdit);

            // Act
            Action act = () => repository.DeleteDispositionDocument(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
