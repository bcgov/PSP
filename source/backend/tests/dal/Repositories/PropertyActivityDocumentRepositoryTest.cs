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
    public class ManagementActivityDocumentRepositoryTest
    {
        [Fact]
        public void AddManagementActivity_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var managementActivityFileDocument = new PimsPropertyActivityDocument();

            var repository = helper.CreateRepository<ManagementActivityDocumentRepository>(user);

            // Act
            var result = repository.AddDocument(managementActivityFileDocument);

            // Assert
            result.PropertyActivityDocumentId.Should().Be(1);
        }

        [Fact]
        public void GetAllByManagementActivity_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var document = new PimsDocument() { DocumentStatusTypeCodeNavigation = new PimsDocumentStatusType() { DocumentStatusTypeCode = "test", Description = "Active", DbCreateUserid = "test", DbLastUpdateUserid = "test" }, DocumentType = new PimsDocumentTyp() { DocumentType = "IMAGE", DocumentTypeDescription = "Image", DbCreateUserid = "test", DbLastUpdateUserid = "test" }, FileName = "test.txt" };
            var managementActivityFileDocument = new PimsPropertyActivityDocument() { Document = document };
            PimsManagementActivity pimsManagementActivity = new PimsManagementActivity()
            {
                PimsPropertyActivityDocuments = new List<PimsPropertyActivityDocument>() { managementActivityFileDocument },
                PropMgmtActivityStatusTypeCode = "ACTIVE",
                PimsPropActivityMgmtActivities = new List<PimsPropActivityMgmtActivity>()
                {
                    new ()
                    {
                        PropMgmtActivitySubtypeCode = "ACCESS",
                    }
                },
                PropMgmtActivityTypeCode = "test"
            };
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(pimsManagementActivity);

            var repository = helper.CreateRepository<ManagementActivityDocumentRepository>(user);

            // Act
            var result = repository.GetAllByParentId(managementActivityFileDocument.PimsManagementActivityId);

            // Assert
            result.FirstOrDefault().Internal_Id.Should().Be(1);
        }

        [Fact]
        public void DeleteManagementActivity_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var managementActivityFileDocument = new PimsPropertyActivityDocument();

            var repository = helper.CreateRepository<ManagementActivityDocumentRepository>(user);

            // Act
            var result = repository.DeleteDocument(managementActivityFileDocument);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void DeletePropertyActivity_Null()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var repository = helper.CreateRepository<ManagementActivityDocumentRepository>(user);

            // Act
            Action act = () => repository.DeleteDocument(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
