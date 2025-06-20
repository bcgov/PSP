using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;
using Pims.Core.Exceptions;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "documents")]
    [ExcludeFromCodeCoverage]
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

            // Act
            var result = repository.TryGet(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDocument>();
        }
        #endregion

        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var document = EntityHelper.CreateDocument();

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<DocumentRepository>(user);

            // Act
            var result = repository.Add(document);
            context.CommitTransaction();

            // Assert
            context.PimsDocuments.Should().HaveCount(1);
        }
        #endregion

        #region Updated
        [Fact]
        public void Updated_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentEdit);

            var document = EntityHelper.CreateDocument();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(document);
            var repository = helper.CreateRepository<DocumentRepository>(user);

            // Act
            document.FileName = "updated";
            var result = repository.Update(document);
            context.CommitTransaction();

            // Assert
            context.PimsDocuments.Should().HaveCount(1);
            context.PimsDocuments.FirstOrDefault().FileName.Should().Be("updated");
        }

        [Fact]
        public void Updated_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var document = EntityHelper.CreateDocument();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(document);
            var repository = helper.CreateRepository<DocumentRepository>(user);

            // Act
            document.FileName = "updated";
            Action act = () => repository.Update(document);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var document = EntityHelper.CreateDocument();
            document.PimsAcquisitionFileDocuments = new List<PimsAcquisitionFileDocument>() { new PimsAcquisitionFileDocument() };
            document.PimsResearchFileDocuments = new List<PimsResearchFileDocument>() { new PimsResearchFileDocument() };
            document.PimsProjectDocuments = new List<PimsProjectDocument>() { new PimsProjectDocument() };
            document.PimsLeaseDocuments = new List<PimsLeaseDocument>() { new PimsLeaseDocument() };
            document.PimsPropertyActivityDocuments = new List<PimsPropertyActivityDocument>() { new PimsPropertyActivityDocument() };
            document.PimsDispositionFileDocuments = new List<PimsDispositionFileDocument>() { new PimsDispositionFileDocument() };
            document.PimsFormTypes = new List<PimsFormType>() { new PimsFormType() { FormTypeCode = "FORM", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "Form" } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(document);
            var repository = helper.CreateRepository<DocumentRepository>(user);

            // Act
            context.ChangeTracker.Clear();
            var result = repository.Delete(document);
            context.CommitTransaction();

            // Assert
            context.PimsDocuments.Should().BeEmpty();
        }

        [Fact]
        public void Delete_Null()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var repository = helper.CreateRepository<DocumentRepository>(user);

            // Act
            Action act = () => repository.Delete(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region DocumentRelationshipCount
        [Fact]
        public void DocumentRelationshipCount_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var document = EntityHelper.CreateDocument();
            document.PimsAcquisitionFileDocuments = new List<PimsAcquisitionFileDocument>() { new PimsAcquisitionFileDocument() };
            document.PimsResearchFileDocuments = new List<PimsResearchFileDocument>() { new PimsResearchFileDocument() };
            document.PimsProjectDocuments = new List<PimsProjectDocument>() { new PimsProjectDocument() };
            document.PimsLeaseDocuments = new List<PimsLeaseDocument>() { new PimsLeaseDocument() };
            document.PimsPropertyActivityDocuments = new List<PimsPropertyActivityDocument>() { new PimsPropertyActivityDocument() };
            document.PimsDispositionFileDocuments = new List<PimsDispositionFileDocument>() { new PimsDispositionFileDocument() };
            document.PimsFormTypes = new List<PimsFormType>() { new PimsFormType() { FormTypeCode = "FORM", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "Form" } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(document);
            var repository = helper.CreateRepository<DocumentRepository>(user);

            // Act
            var result = repository.DocumentRelationshipCount(1);

            // Assert
            result.Should().Be(7);
        }
        #endregion

        #endregion
    }
}
