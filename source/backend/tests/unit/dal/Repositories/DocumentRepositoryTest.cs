using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Exceptions;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    public class DocumentRepositoryTest
    {
        #region Constructors
        public DocumentRepositoryTest() { }
        #endregion

        #region Tests

        #region TryGet
        [Fact]
        public void TryGet_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var document = EntityHelper.CreateDocument();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(document);
            var repository = helper.CreateRepository<DocumentRepository>(user);

            var result = repository.TryGet(1);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDocument>();
            result.FileName.Should().Be("Test Document");
            result.DocumentId.Should().Be(document.DocumentId);

        }
        #endregion

        #region Add
        [Fact]
        public void Add_Success()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentAdd);

            var document = EntityHelper.CreateDocument();

            var repository = helper.CreateRepository<DocumentRepository>(user);

            var result = repository.Add(document);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDocument>();
            result.FileName.Should().Be("Test Document");
            result.DocumentId.Should().Be(document.DocumentId);
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentAdd);

            var repository = helper.CreateRepository<DocumentRepository>(user);

            Action addDocument = () => repository.Add(null);

            addDocument.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region Update
        [Fact]
        public void Update_success()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentEdit);

            var document = EntityHelper.CreateDocument();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(document);
            var repository = helper.CreateRepository<DocumentRepository>(user);

            document.FileName = "Updated document name"; // Update the existing document instance
            var result = repository.Update(document);

            result.Should().NotBeNull();
            result.FileName.Should().Be("Updated document name");
        }

        [Fact]
        public void Update_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentEdit);

            var repository = helper.CreateRepository<DocumentRepository>(user);

            Action updateDocument = () => repository.Update(null);

            updateDocument.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_ThrowIfNotAuthorized()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var document = EntityHelper.CreateDocument();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(document);
            var repository = helper.CreateRepository<DocumentRepository>(user);

            var documentUpdated = EntityHelper.CreateDocument(fileName: "Updated document name", id: 1);

            Action updateDocument = () => repository.Update(document);

            updateDocument.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_Success()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentDelete);

            var document = EntityHelper.CreateDocument();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(document);
            var repository = helper.CreateRepository<DocumentRepository>(user);

            context.ChangeTracker.Clear();

            var result = repository.Delete(document);

            result.Should().Be(true);
        }
        #endregion

        #region GetAll
        [Fact]
        public void GetAllByDocumentType_Success()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var documentTypeOne = EntityHelper.CreateDocumentType();
            helper.CreatePimsContext(user, true).AddAndSaveChanges(documentTypeOne);
            var documentTypeTwo = EntityHelper.CreateDocumentType(id: 2, type: "RAND");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(documentTypeTwo);

            PimsDocument[] documents = { EntityHelper.CreateDocument(), EntityHelper.CreateDocument(id: 2), EntityHelper.CreateDocument(id: 3, typeId: 2, documentType: "RAND") };

            foreach (PimsDocument doc in documents)
            {
                helper.CreatePimsContext(user, true).AddAndSaveChanges(doc);
            }

            var repository = helper.CreateRepository<DocumentRepository>(user);

            var result = repository.GetAllByDocumentType(documentTypeOne.DocumentType);

            result.Should().HaveCount(2);
        }
        #endregion
        #endregion
    }
}