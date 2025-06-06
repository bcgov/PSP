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
using Pims.Dal.Entities.Models;
using System.Security;
using Pims.Api.Models.CodeTypes;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "documentQueues")]
    [ExcludeFromCodeCoverage]
    public class DocumentQueueRepositoryTest
    {
        #region Constructors
        public DocumentQueueRepositoryTest() { }
        #endregion

        #region Tests

        #region TryGetById
        [Fact]
        public void TryGetById_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var documentQueue = EntityHelper.CreateDocumentQueue();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(documentQueue);
            var repository = helper.CreateRepository<DocumentQueueRepository>(user);

            // Act
            var result = repository.TryGetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDocumentQueue>();
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentEdit);

            var documentQueue = EntityHelper.CreateDocumentQueue();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(documentQueue);
            var repository = helper.CreateRepository<DocumentQueueRepository>(user);

            // Act
            documentQueue.DocumentQueueStatusTypeCode = "Updated";
            var result = repository.Update(documentQueue);
            context.CommitTransaction();

            // Assert
            context.PimsDocumentQueues.Should().HaveCount(1);
            context.PimsDocumentQueues.FirstOrDefault().Document.Should().NotBeNull();
            context.PimsDocumentQueues.FirstOrDefault().DocumentQueueStatusTypeCode.Should().Be("Updated");
        }

        [Fact]
        public void Update_Success_RemoveDocument()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentEdit);

            var documentQueue = EntityHelper.CreateDocumentQueue();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(documentQueue);
            var repository = helper.CreateRepository<DocumentQueueRepository>(user);

            // Act
            documentQueue.DocumentQueueStatusTypeCode = "Updated";
            documentQueue.Document = null;
            var result = repository.Update(documentQueue, removeDocument: true);
            context.CommitTransaction();

            // Assert
            context.PimsDocumentQueues.Should().HaveCount(1);
            context.PimsDocumentQueues.FirstOrDefault().Document.Should().BeNull();
            context.PimsDocumentQueues.FirstOrDefault().DocumentQueueStatusTypeCode.Should().Be("Updated");
        }

        [Fact]
        public void Update_Null()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var repository = helper.CreateRepository<DocumentQueueRepository>(user);

            // Act
            Action act = () => repository.Update(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var documentQueue = EntityHelper.CreateDocumentQueue();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(documentQueue);
            var repository = helper.CreateRepository<DocumentQueueRepository>(user);

            // Act
            context.ChangeTracker.Clear();
            var result = repository.Delete(documentQueue);
            context.CommitTransaction();

            // Assert
            context.PimsDocumentQueues.Should().BeEmpty();
        }

        [Fact]
        public void Delete_Null()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var repository = helper.CreateRepository<DocumentQueueRepository>(user);

            // Act
            Action act = () => repository.Delete(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region GetAllByFilter
        [Theory]
        [InlineData(2, null, 2, null, null, null, null, new int[] { 1, 2 })]
        [InlineData(1, null, 1, null, null, null, null, new int[] { 1 })]
        [InlineData(1, DocumentQueueStatusTypes.PENDING, 1, null, null, null, null, new int[] { 1 })]
        [InlineData(1, DocumentQueueStatusTypes.SUCCESS, 1, null, null, null, null, new int[] { 2 })]
        [InlineData(1, DocumentQueueStatusTypes.PROCESSING, 0, null, null, null, null, null)]
        [InlineData(0, null, 0, null, null, null, null, null)]
        [InlineData(1, null, 1, "2023-01-01", null, null, null, new int[] { 2 })]
        [InlineData(1, null, 1, null, "2023-12-31", null, null, new int[] { 1 })]
        [InlineData(1, null, 1, null, null, 3, null, new int[] { 1 })]
        [InlineData(1, null, 1, null, null, null, "PAIMS", new int[] { 1 })]
        public void GetAllByFilter_Success(int quantity, string status, int expectedCount, string startDate, string endDate, int? maxRetries, string dataSourceTypeCode, int[] expectedDocumentQueueIds)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var documentQueue1 = EntityHelper.CreateDocumentQueue(1, DocumentQueueStatusTypes.PENDING.ToString(), "PAIMS");
            documentQueue1.DocProcessStartDt = DateTime.Parse("2022-01-01");
            documentQueue1.DocProcessEndDt = DateTime.Parse("2023-12-31");

            var documentQueue2 = EntityHelper.CreateDocumentQueue(2, DocumentQueueStatusTypes.SUCCESS.ToString());
            documentQueue2.DocProcessStartDt = DateTime.Parse("2023-01-01");
            documentQueue2.DocProcessEndDt = DateTime.Parse("2024-12-31");
            documentQueue2.DocProcessRetries = 4;

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(documentQueue1, documentQueue2);
            var repository = helper.CreateRepository<DocumentQueueRepository>(user);

            var filter = new DocumentQueueFilter
            {
                Quantity = quantity,
                DocumentQueueStatusTypeCodes = status != null ? new string[] { status } : null,
                DocProcessStartDate = startDate != null ? DateTime.Parse(startDate) : (DateTime?)null,
                DocProcessEndDate = endDate != null ? DateTime.Parse(endDate) : (DateTime?)null,
                MaxDocProcessRetries = maxRetries,
                DataSourceTypeCode = dataSourceTypeCode
            };

            // Act
            var result = repository.GetAllByFilter(filter);

            // Assert
            result.Should().HaveCount(expectedCount);
            if (expectedCount > 0)
            {
                result.Should().OnlyContain(dq => expectedDocumentQueueIds.Any(edq => edq == dq.DocumentQueueId));
            }
        }

        [Fact]
        public void GetAllByFilter_NoResults()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<DocumentQueueRepository>(user);

            var filter = new DocumentQueueFilter { Quantity = 1 };

            // Act
            var result = repository.GetAllByFilter(filter);

            // Assert
            result.Should().BeEmpty();
        }
        #endregion

        #region DocumentQueueCount
        [Fact]
        public void DocumentQueueCount_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var documentQueue = EntityHelper.CreateDocumentQueue();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(documentQueue);
            var repository = helper.CreateRepository<DocumentQueueRepository>(user);

            var statusType = new PimsDocumentQueueStatusType { DocumentQueueStatusTypeCode = documentQueue.DocumentQueueStatusTypeCode };

            // Act
            var result = repository.DocumentQueueCount(statusType);

            // Assert
            result.Should().Be(1);
        }
        #endregion

        #endregion
    }
}
