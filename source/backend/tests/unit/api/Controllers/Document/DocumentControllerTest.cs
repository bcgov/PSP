using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

using Pims.Api.Controllers;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Requests.Document.UpdateMetadata;
using Pims.Api.Models.Requests.Http;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class DocumentControllerTest
    {
        #region Variables
        private Mock<IDocumentService> _service;
        private Mock<IDocumentFileService> _documentFileService;
        private DocumentController _controller;
        private IMapper _mapper;
        #endregion

        public DocumentControllerTest()
        {
            var helper = new TestHelper();
            this._controller = helper.CreateController<DocumentController>(Permissions.DocumentView);
            this._mapper = helper.GetService<IMapper>();
            this._service = helper.GetService<Mock<IDocumentService>>();
            this._documentFileService = helper.GetService<Mock<IDocumentFileService>>();
        }

        #region Tests
        [Fact]
        public void GetDocumentTypes_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetPimsDocumentTypes()).Returns(new List<PimsDocumentTyp>());

            // Act
            var result = this._controller.GetDocumentTypes();

            // Assert
            this._service.Verify(m => m.GetPimsDocumentTypes(), Times.Once());
        }

        [Fact]
        public void UpdateDocumentMetadata_Success()
        {
            // Arrange
            var updateRequest = new DocumentUpdateRequest() { DocumentId = 1 };
            this._service.Setup(m => m.UpdateDocumentAsync(updateRequest)).ReturnsAsync(new DocumentUpdateResponse());

            // Act
            var result = this._controller.UpdateDocumentMetadata(1, updateRequest);

            // Assert
            this._service.Verify(m => m.UpdateDocumentAsync(updateRequest), Times.Once());
        }

        [Fact]
        public void UpdateDocumentMetadata_InvalidDocumentId()
        {
            // Arrange
            var updateRequest = new DocumentUpdateRequest() { DocumentId = 2 };
            this._service.Setup(m => m.UpdateDocumentAsync(updateRequest)).ReturnsAsync(new DocumentUpdateResponse());

            // Act
            Func<Task> act = async () => await this._controller.UpdateDocumentMetadata(1, updateRequest);

            // Assert
            act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public void DownloadWrappedFileDocument_Success()
        {
            // Arrange
            this._service.Setup(m => m.DownloadFileAsync(1, 2)).ReturnsAsync(new ExternalResponse<FileDownloadResponse>());

            // Act
            var result = this._controller.DownloadWrappedFile(1, 2);

            // Assert
            this._service.Verify(m => m.DownloadFileAsync(1, 2), Times.Once());
        }

        [Fact]
        public async Task DownloadFileDocument_Success()
        {
            // Arrange
            this._service.Setup(m => m.DownloadFileAsync(1, 2)).ReturnsAsync(new ExternalResponse<FileDownloadResponse>() { Payload = new FileDownloadResponse() { FilePayload = "cmVmYWN0b3IgcHJvcGVydHkgdGFicyB0byB1c2Ugcm91dGVyLCBpbmNsdWRpbmcgbWFuYWdlbWVudC4NCkVuc3VyZSB0aGF0IG1hbmFnZW1lbnQgdGFiIHJlZGlyZWN0cyBiYWNrIHRvIG1hbmFnZW1lbnQgdmlldyBhZnRlciBlZGl0aW5nLg0KDQpkaXNjdXNzIDY4MzkgaW4gZGV2IG1lZXRpbmcgdG1ydw0KDQp3aGF0IGlzIGdvaW5nIG9uIHdpdGggaXNfZGlzYWJsZWQ/IHdoeSBhcmUgd2UgYWRkaW5nIGl0IHRvIGpvaW4gdGFibGVzPw0KDQpjb21tZW50IG9uIHJldmlld2luZyB1aSB3aXRoIGFuYSBkdXJpbmcgY29kZSByZXZpZXcuDQoNCmNsZWFuIHVwIG9sZCBnaXRodWIgYWN0aW9ucyAtIA0KDQptYWtlIGdpdGh1YiBhY3Rpb25zIHRvIHByb2QgYW5kIHVhdCByZXN0cmljdGVkIHRvIGEgc21hbGxlciBncm91cA0KDQptYWtlIHByb2QgYW5kIHVhdCBhY3Rpb25zIGhhdmUgYSBkaXNjbGFpbWVyIHdoZW4gcnVubmluZywgYW5kIHRoZW4gaW5jcmVhc2UgdGhlIHZlcmJvc2l0eSBvZiBsb2dnaW5nIHRvIHRlYW1zIHN1Y2ggdGhhdCB0aGUgb3BlcmF0aW9ucyBjb25kdWN0ZWQgYnkgdGhlIGFjdGlvbiBhcmUgbGFpZCBvdXQgZm9yIG5vbi10ZWNobmljYWwgdXNlcnMuDQoNCg0KDQpyZW1vdmU6IGRiLXNjaG1hLnltbA0KbWF5YmU6IHphcC1zY2FuLnltbCwgdGFnLnltbCwgcmVsZWFzZS55bWwsIGltYWdlLXNjYW4tYW5hbHlzaXMsIGNpLWNkLXBpbXMtbWFzdGVyLnltbCwgYXBwLWxvZ2dpbmcueW1sDQoNCndlIHdvdWxkIGxpa2UgdG8gcmVtb3ZlIHRoZSB2ZXJzaW9uIGJ1bXAgZnJvbSBkZXYgd2hlbiB3ZSBoYXZlIGFuIGFsdGVybmF0ZSB3YXkgb2Ygc2VlaW5nIHRoZSBnaXQgY29tbWl0IG9uIHRoZSBpbWFnZS4NCg0KbmVlZCB0byB1cGRhdGUgZW52IGdlbmVyYXRpb24gdG8gYWZmZWN0IG5ldyBsb2NhdGlvbi4=", Mimetype = "text/plain" } });

            // Act
            var result = await this._controller.DownloadFile(1, 2);

            // Assert
            this._service.Verify(m => m.DownloadFileAsync(1, 2), Times.Once());
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task DownloadFileDocument_NullAsync()
        {
            // Arrange
            this._service.Setup(m => m.DownloadFileAsync(1, 2)).ReturnsAsync(new ExternalResponse<FileDownloadResponse>() { Payload = null });

            // Act
            var result = await this._controller.DownloadFile(1, 2);

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetDocumentList_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetStorageDocumentList("", null, null));

            // Act
            var result = this._controller.GetDocumentList();

            // Assert
            this._service.Verify(m => m.GetStorageDocumentList("", null, null), Times.Once());
        }

        [Fact]
        public void GetDocumentStorageTypes_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetStorageDocumentTypes("", null, null));

            // Act
            var result = this._controller.GetDocumentStorageTypes();

            // Assert
            this._service.Verify(m => m.GetStorageDocumentTypes("", null, null), Times.Once());
        }

        [Fact]
        public void GetDocumentStorageTypeMetadata_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetDocumentTypeMetadataType(1, "", null, null));

            // Act
            var result = this._controller.GetDocumentStorageTypeMetadata(1);

            // Assert
            this._service.Verify(m => m.GetDocumentTypeMetadataType(1, "", null, null), Times.Once());
        }

        [Fact]
        public void GetDocumentStorageTypeDetail_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetStorageDocumentDetail(1));

            // Act
            var result = this._controller.GetDocumentStorageTypeDetail(1);

            // Assert
            this._service.Verify(m => m.GetStorageDocumentDetail(1), Times.Once());
        }

        [Fact]
        public void DownloadWrappedFile_Success()
        {
            // Arrange
            this._service.Setup(m => m.DownloadFileLatestAsync(1)).ReturnsAsync(new ExternalResponse<FileDownloadResponse>() { Payload = new FileDownloadResponse() });

            // Act
            var result = this._controller.DownloadWrappedFile(1);

            // Assert
            this._service.Verify(m => m.DownloadFileLatestAsync(1), Times.Once());
        }

        [Fact]
        public void DownloadFile_Success()
        {
            // Arrange
            this._service.Setup(m => m.DownloadFileAsync(1, 1)).ReturnsAsync(new ExternalResponse<FileDownloadResponse>() { Payload = new FileDownloadResponse() { FilePayload = "cmVmYWN0b3IgcHJvcGVydHkgdGFicyB0byB1c2Ugcm91dGVyLCBpbmNsdWRpbmcgbWFuYWdlbWVudC4NCkVuc3VyZSB0aGF0IG1hbmFnZW1lbnQgdGFiIHJlZGlyZWN0cyBiYWNrIHRvIG1hbmFnZW1lbnQgdmlldyBhZnRlciBlZGl0aW5nLg0KDQpkaXNjdXNzIDY4MzkgaW4gZGV2IG1lZXRpbmcgdG1ydw0KDQp3aGF0IGlzIGdvaW5nIG9uIHdpdGggaXNfZGlzYWJsZWQ/IHdoeSBhcmUgd2UgYWRkaW5nIGl0IHRvIGpvaW4gdGFibGVzPw0KDQpjb21tZW50IG9uIHJldmlld2luZyB1aSB3aXRoIGFuYSBkdXJpbmcgY29kZSByZXZpZXcuDQoNCmNsZWFuIHVwIG9sZCBnaXRodWIgYWN0aW9ucyAtIA0KDQptYWtlIGdpdGh1YiBhY3Rpb25zIHRvIHByb2QgYW5kIHVhdCByZXN0cmljdGVkIHRvIGEgc21hbGxlciBncm91cA0KDQptYWtlIHByb2QgYW5kIHVhdCBhY3Rpb25zIGhhdmUgYSBkaXNjbGFpbWVyIHdoZW4gcnVubmluZywgYW5kIHRoZW4gaW5jcmVhc2UgdGhlIHZlcmJvc2l0eSBvZiBsb2dnaW5nIHRvIHRlYW1zIHN1Y2ggdGhhdCB0aGUgb3BlcmF0aW9ucyBjb25kdWN0ZWQgYnkgdGhlIGFjdGlvbiBhcmUgbGFpZCBvdXQgZm9yIG5vbi10ZWNobmljYWwgdXNlcnMuDQoNCg0KDQpyZW1vdmU6IGRiLXNjaG1hLnltbA0KbWF5YmU6IHphcC1zY2FuLnltbCwgdGFnLnltbCwgcmVsZWFzZS55bWwsIGltYWdlLXNjYW4tYW5hbHlzaXMsIGNpLWNkLXBpbXMtbWFzdGVyLnltbCwgYXBwLWxvZ2dpbmcueW1sDQoNCndlIHdvdWxkIGxpa2UgdG8gcmVtb3ZlIHRoZSB2ZXJzaW9uIGJ1bXAgZnJvbSBkZXYgd2hlbiB3ZSBoYXZlIGFuIGFsdGVybmF0ZSB3YXkgb2Ygc2VlaW5nIHRoZSBnaXQgY29tbWl0IG9uIHRoZSBpbWFnZS4NCg0KbmVlZCB0byB1cGRhdGUgZW52IGdlbmVyYXRpb24gdG8gYWZmZWN0IG5ldyBsb2NhdGlvbi4=", Mimetype = "text/plain" } });

            // Act
            var result = this._controller.DownloadFile(1, 1);

            // Assert
            this._service.Verify(m => m.DownloadFileAsync(1, 1), Times.Once());
        }

        [Fact]
        public async Task DownloadFile_NoResultAsync()
        {
            // Arrange
            this._service.Setup(m => m.DownloadFileAsync(1, 1)).ReturnsAsync(new ExternalResponse<FileDownloadResponse>() { Payload = null });

            // Act
            var result = await this._controller.DownloadFile(1, 1);

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void StreamFile_Success()
        {
            // Arrange
            this._service.Setup(m => m.DownloadFileAsync(1, 1)).ReturnsAsync(new ExternalResponse<FileDownloadResponse>() { Payload = new FileDownloadResponse() { FilePayload = "cmVmYWN0b3IgcHJvcGVydHkgdGFicyB0byB1c2Ugcm91dGVyLCBpbmNsdWRpbmcgbWFuYWdlbWVudC4NCkVuc3VyZSB0aGF0IG1hbmFnZW1lbnQgdGFiIHJlZGlyZWN0cyBiYWNrIHRvIG1hbmFnZW1lbnQgdmlldyBhZnRlciBlZGl0aW5nLg0KDQpkaXNjdXNzIDY4MzkgaW4gZGV2IG1lZXRpbmcgdG1ydw0KDQp3aGF0IGlzIGdvaW5nIG9uIHdpdGggaXNfZGlzYWJsZWQ/IHdoeSBhcmUgd2UgYWRkaW5nIGl0IHRvIGpvaW4gdGFibGVzPw0KDQpjb21tZW50IG9uIHJldmlld2luZyB1aSB3aXRoIGFuYSBkdXJpbmcgY29kZSByZXZpZXcuDQoNCmNsZWFuIHVwIG9sZCBnaXRodWIgYWN0aW9ucyAtIA0KDQptYWtlIGdpdGh1YiBhY3Rpb25zIHRvIHByb2QgYW5kIHVhdCByZXN0cmljdGVkIHRvIGEgc21hbGxlciBncm91cA0KDQptYWtlIHByb2QgYW5kIHVhdCBhY3Rpb25zIGhhdmUgYSBkaXNjbGFpbWVyIHdoZW4gcnVubmluZywgYW5kIHRoZW4gaW5jcmVhc2UgdGhlIHZlcmJvc2l0eSBvZiBsb2dnaW5nIHRvIHRlYW1zIHN1Y2ggdGhhdCB0aGUgb3BlcmF0aW9ucyBjb25kdWN0ZWQgYnkgdGhlIGFjdGlvbiBhcmUgbGFpZCBvdXQgZm9yIG5vbi10ZWNobmljYWwgdXNlcnMuDQoNCg0KDQpyZW1vdmU6IGRiLXNjaG1hLnltbA0KbWF5YmU6IHphcC1zY2FuLnltbCwgdGFnLnltbCwgcmVsZWFzZS55bWwsIGltYWdlLXNjYW4tYW5hbHlzaXMsIGNpLWNkLXBpbXMtbWFzdGVyLnltbCwgYXBwLWxvZ2dpbmcueW1sDQoNCndlIHdvdWxkIGxpa2UgdG8gcmVtb3ZlIHRoZSB2ZXJzaW9uIGJ1bXAgZnJvbSBkZXYgd2hlbiB3ZSBoYXZlIGFuIGFsdGVybmF0ZSB3YXkgb2Ygc2VlaW5nIHRoZSBnaXQgY29tbWl0IG9uIHRoZSBpbWFnZS4NCg0KbmVlZCB0byB1cGRhdGUgZW52IGdlbmVyYXRpb24gdG8gYWZmZWN0IG5ldyBsb2NhdGlvbi4=", Mimetype = "text/plain" } });

            // Act
            var result = this._controller.StreamFile(1, 1);

            // Assert
            this._service.Verify(m => m.StreamFileAsync(1, 1), Times.Once());
        }

        [Fact]
        public async Task StreamFile_NoResultAsync()
        {
            // Arrange
            this._service.Setup(m => m.DownloadFileAsync(1, 1)).ReturnsAsync(new ExternalResponse<FileDownloadResponse>() { Payload = null });

            // Act
            var result = await this._controller.StreamFile(1, 1);

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DownloadFileLatest_Success()
        {
            // Arrange
            this._service.Setup(m => m.DownloadFileLatestAsync(1)).ReturnsAsync(new ExternalResponse<FileDownloadResponse>() { Payload = new FileDownloadResponse() { FilePayload = "cmVmYWN0b3IgcHJvcGVydHkgdGFicyB0byB1c2Ugcm91dGVyLCBpbmNsdWRpbmcgbWFuYWdlbWVudC4NCkVuc3VyZSB0aGF0IG1hbmFnZW1lbnQgdGFiIHJlZGlyZWN0cyBiYWNrIHRvIG1hbmFnZW1lbnQgdmlldyBhZnRlciBlZGl0aW5nLg0KDQpkaXNjdXNzIDY4MzkgaW4gZGV2IG1lZXRpbmcgdG1ydw0KDQp3aGF0IGlzIGdvaW5nIG9uIHdpdGggaXNfZGlzYWJsZWQ/IHdoeSBhcmUgd2UgYWRkaW5nIGl0IHRvIGpvaW4gdGFibGVzPw0KDQpjb21tZW50IG9uIHJldmlld2luZyB1aSB3aXRoIGFuYSBkdXJpbmcgY29kZSByZXZpZXcuDQoNCmNsZWFuIHVwIG9sZCBnaXRodWIgYWN0aW9ucyAtIA0KDQptYWtlIGdpdGh1YiBhY3Rpb25zIHRvIHByb2QgYW5kIHVhdCByZXN0cmljdGVkIHRvIGEgc21hbGxlciBncm91cA0KDQptYWtlIHByb2QgYW5kIHVhdCBhY3Rpb25zIGhhdmUgYSBkaXNjbGFpbWVyIHdoZW4gcnVubmluZywgYW5kIHRoZW4gaW5jcmVhc2UgdGhlIHZlcmJvc2l0eSBvZiBsb2dnaW5nIHRvIHRlYW1zIHN1Y2ggdGhhdCB0aGUgb3BlcmF0aW9ucyBjb25kdWN0ZWQgYnkgdGhlIGFjdGlvbiBhcmUgbGFpZCBvdXQgZm9yIG5vbi10ZWNobmljYWwgdXNlcnMuDQoNCg0KDQpyZW1vdmU6IGRiLXNjaG1hLnltbA0KbWF5YmU6IHphcC1zY2FuLnltbCwgdGFnLnltbCwgcmVsZWFzZS55bWwsIGltYWdlLXNjYW4tYW5hbHlzaXMsIGNpLWNkLXBpbXMtbWFzdGVyLnltbCwgYXBwLWxvZ2dpbmcueW1sDQoNCndlIHdvdWxkIGxpa2UgdG8gcmVtb3ZlIHRoZSB2ZXJzaW9uIGJ1bXAgZnJvbSBkZXYgd2hlbiB3ZSBoYXZlIGFuIGFsdGVybmF0ZSB3YXkgb2Ygc2VlaW5nIHRoZSBnaXQgY29tbWl0IG9uIHRoZSBpbWFnZS4NCg0KbmVlZCB0byB1cGRhdGUgZW52IGdlbmVyYXRpb24gdG8gYWZmZWN0IG5ldyBsb2NhdGlvbi4=", Mimetype = "text/plain" } });

            // Act
            var result = this._controller.DownloadFileLatest(1);

            // Assert
            this._service.Verify(m => m.DownloadFileLatestAsync(1), Times.Once());
        }

        [Fact]
        public async Task DownloadFileLatest_NoResultAsync()
        {
            // Arrange
            this._service.Setup(m => m.DownloadFileLatestAsync(1)).ReturnsAsync(new ExternalResponse<FileDownloadResponse>() { Payload = null });

            // Act
            var result = await this._controller.DownloadFileLatest(1);

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void StreamFileLatest_Success()
        {
            // Arrange
            this._service.Setup(m => m.DownloadFileLatestAsync(1)).ReturnsAsync(new ExternalResponse<FileDownloadResponse>() { Payload = new FileDownloadResponse() { FilePayload = "cmVmYWN0b3IgcHJvcGVydHkgdGFicyB0byB1c2Ugcm91dGVyLCBpbmNsdWRpbmcgbWFuYWdlbWVudC4NCkVuc3VyZSB0aGF0IG1hbmFnZW1lbnQgdGFiIHJlZGlyZWN0cyBiYWNrIHRvIG1hbmFnZW1lbnQgdmlldyBhZnRlciBlZGl0aW5nLg0KDQpkaXNjdXNzIDY4MzkgaW4gZGV2IG1lZXRpbmcgdG1ydw0KDQp3aGF0IGlzIGdvaW5nIG9uIHdpdGggaXNfZGlzYWJsZWQ/IHdoeSBhcmUgd2UgYWRkaW5nIGl0IHRvIGpvaW4gdGFibGVzPw0KDQpjb21tZW50IG9uIHJldmlld2luZyB1aSB3aXRoIGFuYSBkdXJpbmcgY29kZSByZXZpZXcuDQoNCmNsZWFuIHVwIG9sZCBnaXRodWIgYWN0aW9ucyAtIA0KDQptYWtlIGdpdGh1YiBhY3Rpb25zIHRvIHByb2QgYW5kIHVhdCByZXN0cmljdGVkIHRvIGEgc21hbGxlciBncm91cA0KDQptYWtlIHByb2QgYW5kIHVhdCBhY3Rpb25zIGhhdmUgYSBkaXNjbGFpbWVyIHdoZW4gcnVubmluZywgYW5kIHRoZW4gaW5jcmVhc2UgdGhlIHZlcmJvc2l0eSBvZiBsb2dnaW5nIHRvIHRlYW1zIHN1Y2ggdGhhdCB0aGUgb3BlcmF0aW9ucyBjb25kdWN0ZWQgYnkgdGhlIGFjdGlvbiBhcmUgbGFpZCBvdXQgZm9yIG5vbi10ZWNobmljYWwgdXNlcnMuDQoNCg0KDQpyZW1vdmU6IGRiLXNjaG1hLnltbA0KbWF5YmU6IHphcC1zY2FuLnltbCwgdGFnLnltbCwgcmVsZWFzZS55bWwsIGltYWdlLXNjYW4tYW5hbHlzaXMsIGNpLWNkLXBpbXMtbWFzdGVyLnltbCwgYXBwLWxvZ2dpbmcueW1sDQoNCndlIHdvdWxkIGxpa2UgdG8gcmVtb3ZlIHRoZSB2ZXJzaW9uIGJ1bXAgZnJvbSBkZXYgd2hlbiB3ZSBoYXZlIGFuIGFsdGVybmF0ZSB3YXkgb2Ygc2VlaW5nIHRoZSBnaXQgY29tbWl0IG9uIHRoZSBpbWFnZS4NCg0KbmVlZCB0byB1cGRhdGUgZW52IGdlbmVyYXRpb24gdG8gYWZmZWN0IG5ldyBsb2NhdGlvbi4=", Mimetype = "text/plain" } });

            // Act
            var result = this._controller.StreamFileLatest(1);

            // Assert
            this._service.Verify(m => m.StreamFileLatestAsync(1), Times.Once());
        }

        [Fact]
        public async Task StreamFileLatest_NoResultAsync()
        {
            // Arrange
            this._service.Setup(m => m.DownloadFileLatestAsync(1)).ReturnsAsync(new ExternalResponse<FileDownloadResponse>() { Payload = null });

            // Act
            var result = await this._controller.StreamFileLatest(1);

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetDocumentMetadata_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetStorageDocumentMetadata(1, "", null, null));

            // Act
            var result = this._controller.GetDocumentMetadata(1);

            // Assert
            this._service.Verify(m => m.GetStorageDocumentMetadata(1, "", null, null), Times.Once());
        }

        #endregion
    }
}
