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
    public class ProjectDocumentRepositoryTest
    {
        private readonly TestHelper _helper;

        public ProjectDocumentRepositoryTest()
        {
            this._helper = new TestHelper();
        }

        private ProjectDocumentRepository CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.CreateRepository<ProjectDocumentRepository>(user);
        }

        [Fact]
        public void AddProjectDocument_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentAdd, Permissions.ProjectEdit);

            // Act
            var result = repository.AddDocument(new PimsProjectDocument());

            // Assert
            result.ProjectDocumentId.Should().Be(1);
        }

        [Fact]
        public void GetAllByProjectFile_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentView, Permissions.ProjectView);

            var document = new PimsDocument() { FileName = "test doc", DocumentStatusTypeCodeNavigation = new PimsDocumentStatusType() { DocumentStatusTypeCode = "test", DbCreateUserid = "create user", DbLastUpdateUserid = "last user", Description = "description" }, DocumentType = new PimsDocumentTyp() { DocumentType = "type", DocumentTypeDescription = "description" } };
            var projectDocument = new PimsProjectDocument() { Document = document };
            var pimsProjectFile = new PimsProject() { ProjectStatusTypeCode = "status", Description = "description", PimsProjectDocuments = new List<PimsProjectDocument>() { projectDocument } };
            _helper.AddAndSaveChanges(pimsProjectFile);

            // Act
            var result = repository.GetAllByParentId(projectDocument.ProjectId);

            // Assert
            result.FirstOrDefault().Internal_Id.Should().Be(1);
        }

        [Fact]
        public void DeleteProjectDocument_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentDelete, Permissions.ProjectEdit);

            // Act
            var result = repository.DeleteDocument(new PimsProjectDocument());

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void DeleteProjectDocument_Null()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.DocumentDelete, Permissions.ProjectEdit);

            // Act
            Action act = () => repository.DeleteDocument(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
