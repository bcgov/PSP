using FluentAssertions;
using Moq;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Mayan.Metadata;
using Pims.Api.Models.Mayan.Sync;
using Pims.Api.Repositories.Mayan;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "document")]
    [ExcludeFromCodeCoverage]
    public class DocumentSyncServiceTest
    {
        #region Tests
        private readonly TestHelper _helper;

        public DocumentSyncServiceTest()
        {
            _helper = new TestHelper();
        }

        private DocumentSyncService CreateDocumentySyncServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            return _helper.Create<DocumentSyncService>(user);
        }

        private SyncModel CreateSyncModel (IList<MetadataModel> metadataModels = null, IList<DocumentTypeModel> documentTypeModels = null)
        {
            metadataModels = metadataModels ?? new List<MetadataModel>();
            documentTypeModels = documentTypeModels ?? new List<DocumentTypeModel>();
            return new SyncModel() { MetadataTypes = metadataModels, DocumentTypes = documentTypeModels };
        }

        private ExternalResult<QueryResult<MetadataType>> CreateMetadataResult (params MetadataType[] metadataTypes)
        {
            return new ExternalResult<QueryResult<MetadataType>>() { Payload = new QueryResult<MetadataType>() { Results = metadataTypes, Count = metadataTypes.Length }, HttpStatusCode = System.Net.HttpStatusCode.Created };
        }

        private ExternalResult<QueryResult<DocumentType>> CreateDocumentTypeResult(params DocumentType[] documentTypes)
        {
            return new ExternalResult<QueryResult<DocumentType>>() { Payload = new QueryResult<DocumentType>() { Results = documentTypes, Count = documentTypes.Length }, HttpStatusCode = System.Net.HttpStatusCode.Created };
        }

        private ExternalResult<QueryResult<DocumentTypeMetadataType>> CreateDocumentTypeMetadataTypeResult(params DocumentTypeMetadataType[] documentMetadataTypes)
        {
            return new ExternalResult<QueryResult<DocumentTypeMetadataType>>() { Payload = new QueryResult<DocumentTypeMetadataType>() { Results = documentMetadataTypes, Count = documentMetadataTypes.Length }, HttpStatusCode = System.Net.HttpStatusCode.Created };
        }

        #region SyncMetadata
        [Fact]
        public void Add_SyncMetadata_Single()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(new List<MetadataModel>() { new MetadataModel() { Label = "test", Name="TEST" } });

            var mayanRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>())).Returns(Task.FromResult(CreateMetadataResult()));
            MetadataType createdMetadataType = null;
            mayanRepository.Setup(x => x.TryCreateMetadataTypeAsync(It.IsAny<MetadataType>())).Callback<MetadataType>(x => createdMetadataType = x).Returns(Task.FromResult(new ExternalResult<MetadataType>() { HttpStatusCode = System.Net.HttpStatusCode.Created, Payload = new MetadataType() { Label = "test", Name = "TEST" } }));

            // Act
            var result = service.SyncMayanMetadataTypes(model);

            // Assert
            mayanRepository.Verify(x => x.TryCreateMetadataTypeAsync(It.IsAny<MetadataType>()), Times.Once);
            createdMetadataType.Should().BeEquivalentTo(new MetadataType() { Label = "test", Name = "TEST" });
            result.UpdatedMetadata.Should().BeEmpty();
            result.DeletedMetadata.Should().BeEmpty();
            result.CreatedMetadata.Should().OnlyContain(x => x.Payload.Name == "TEST" && x.Payload.Label == "test");
        }

        [Fact]
        public void Add_SyncMetadata_Multiple()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(new List<MetadataModel>() { new MetadataModel() { Label = "test", Name = "TEST" }, new MetadataModel() { Label = "test2", Name = "TEST2" } });

            var mayanRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>())).Returns(Task.FromResult(CreateMetadataResult()));
            MetadataType createdMetadataTypeOne = null;
            MetadataType createdMetadataTypeTwo = null;
            mayanRepository.Setup(x => x.TryCreateMetadataTypeAsync(It.Is<MetadataType>(x => x.Name == "TEST")))
                .Callback<MetadataType>(x => createdMetadataTypeOne = x).Returns(Task.FromResult(new ExternalResult<MetadataType>() { HttpStatusCode = System.Net.HttpStatusCode.Created, Payload = new MetadataType() { Label = "test", Name = "TEST" } }));
            mayanRepository.Setup(x => x.TryCreateMetadataTypeAsync(It.Is<MetadataType>(x => x.Name == "TEST2")))
                .Callback<MetadataType>(x => createdMetadataTypeTwo = x).Returns(Task.FromResult(new ExternalResult<MetadataType>() { HttpStatusCode = System.Net.HttpStatusCode.Created, Payload = new MetadataType() { Label = "test2", Name = "TEST2" } }));

            // Act
            var result = service.SyncMayanMetadataTypes(model);

            // Assert
            mayanRepository.Verify(x => x.TryCreateMetadataTypeAsync(It.IsAny<MetadataType>()), Times.Exactly(2));
            createdMetadataTypeOne.Should().BeEquivalentTo(new MetadataType() { Label = "test", Name = "TEST" });
            createdMetadataTypeTwo.Should().BeEquivalentTo(new MetadataType() { Label = "test2", Name = "TEST2" });
            result.UpdatedMetadata.Should().BeEmpty();
            result.DeletedMetadata.Should().BeEmpty();
            result.CreatedMetadata.Should().HaveCount(2);
            result.CreatedMetadata.Should().Contain(x => x.Payload.Name == "TEST" && x.Payload.Label == "test");
            result.CreatedMetadata.Should().Contain(x => x.Payload.Name == "TEST2" && x.Payload.Label == "test2");
        }

        [Fact]
        public void RemoveLingeringMetadata_SyncMetadata_Single()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel();
            model.RemoveLingeringMetadataTypes = true;

            var mayanRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult(new MetadataType[1] { new MetadataType() { Label = "test", Name = "TEST" } })));
            mayanRepository.Setup(x => x.TryDeleteMetadataTypeAsync(It.IsAny<long>())).Returns(Task.FromResult(new ExternalResult<string>() { Payload = "deleted"}));

            // Act
            var result = service.SyncMayanMetadataTypes(model);

            // Assert
            mayanRepository.Verify(x => x.TryDeleteMetadataTypeAsync(It.IsAny<long>()), Times.Once());
            result.CreatedMetadata.Should().BeEmpty();
            result.UpdatedMetadata.Should().BeEmpty();
            result.DeletedMetadata.Should().ContainSingle(x => x.Payload == "deleted");
        }

        [Fact]
        public void RemoveLingeringMetadata_SyncMetadata_Multiple()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel();
            model.RemoveLingeringMetadataTypes = true;

            var mayanRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult(new MetadataType[2] { new MetadataType() { Label = "test", Name = "TEST" }, new MetadataType() { Label = "test2", Name = "TEST2" } })));
            mayanRepository.Setup(x => x.TryDeleteMetadataTypeAsync(It.IsAny<long>())).Returns(Task.FromResult(new ExternalResult<string>() { Payload = "deleted" }));

            // Act
            var result = service.SyncMayanMetadataTypes(model);

            // Assert
            mayanRepository.Verify(x => x.TryDeleteMetadataTypeAsync(It.IsAny<long>()), Times.Exactly(2));
            result.CreatedMetadata.Should().BeEmpty();
            result.UpdatedMetadata.Should().BeEmpty();
            result.DeletedMetadata.Should().HaveCount(2);
            result.DeletedMetadata.Should().Contain(x => x.Payload == "deleted");
        }

        [Fact]
        public void RemoveLingeringMetadata_SyncMetadata_RemoveNoneIfFalse()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel();
            model.RemoveLingeringMetadataTypes = false;

            var mayanRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult(new MetadataType[2] { new MetadataType() { Label = "test", Name = "TEST" }, new MetadataType() { Label = "test2", Name = "TEST2" } })));
            mayanRepository.Setup(x => x.TryDeleteMetadataTypeAsync(It.IsAny<long>())).Returns(Task.FromResult(new ExternalResult<string>() { Payload = "deleted" }));

            // Act
            var result = service.SyncMayanMetadataTypes(model);

            // Assert
            mayanRepository.Verify(x => x.TryDeleteMetadataTypeAsync(It.IsAny<long>()), Times.Never());
            result.CreatedMetadata.Should().BeEmpty();
            result.UpdatedMetadata.Should().BeEmpty();
            result.DeletedMetadata.Should().BeEmpty();
        }

        [Fact]
        public void Update_SyncMetadata_SingleLabelChange()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(new List<MetadataModel>() { new MetadataModel() { Label = "testUpdated", Name = "TEST" } });

            var mayanRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult(new MetadataType[1] { new MetadataType() { Label = "test", Name = "TEST" } })));
            MetadataType updatedMetadataType = null;
            mayanRepository.Setup(x => x.TryUpdateMetadataTypeAsync(It.Is<MetadataType>(x => x.Name == "TEST")))
                .Callback<MetadataType>(x => updatedMetadataType = x).Returns(Task.FromResult(new ExternalResult<MetadataType>() { HttpStatusCode = System.Net.HttpStatusCode.OK, Payload = new MetadataType() { Label = "testUpdated", Name = "TEST" } }));

            // Act
            var result = service.SyncMayanMetadataTypes(model);

            // Assert
            mayanRepository.Verify(x => x.TryUpdateMetadataTypeAsync(It.IsAny<MetadataType>()), Times.Once());
            updatedMetadataType.Should().BeEquivalentTo(new MetadataModel() { Label = "testUpdated", Name = "TEST" });
            result.CreatedMetadata.Should().BeEmpty();
            result.UpdatedMetadata.Should().HaveCount(1);
            result.DeletedMetadata.Should().BeEmpty();
            result.UpdatedMetadata.Should().OnlyContain(x => x.Payload.Name == "TEST" && x.Payload.Label == "testUpdated");
        }

        [Fact]
        public void Update_SyncMetadata_MultipleLabelChange()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(new List<MetadataModel>() { new MetadataModel() { Label = "testUpdated", Name = "TEST" }, new MetadataModel() { Label = "testTwoUpdated", Name = "TESTTWO" } });

            var mayanRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult(new MetadataType[2] { new MetadataType() { Label = "test", Name = "TEST" }, new MetadataType() { Label = "testTwo", Name = "TESTTWO" } })));
            MetadataType updatedMetadataTypeOne = null;
            MetadataType updatedMetadataTypeTwo = null;
            mayanRepository.Setup(x => x.TryUpdateMetadataTypeAsync(It.Is<MetadataType>(x => x.Name == "TEST")))
                .Callback<MetadataType>(x => updatedMetadataTypeOne = x).Returns(Task.FromResult(new ExternalResult<MetadataType>() { HttpStatusCode = System.Net.HttpStatusCode.OK, Payload = new MetadataType() { Label = "testUpdated", Name = "TEST" } }));
            mayanRepository.Setup(x => x.TryUpdateMetadataTypeAsync(It.Is<MetadataType>(x => x.Name == "TESTTWO")))
                .Callback<MetadataType>(x => updatedMetadataTypeTwo = x).Returns(Task.FromResult(new ExternalResult<MetadataType>() { HttpStatusCode = System.Net.HttpStatusCode.OK, Payload = new MetadataType() { Label = "testTwoUpdated", Name = "TESTTWO" } }));

            // Act
            var result = service.SyncMayanMetadataTypes(model);

            // Assert
            mayanRepository.Verify(x => x.TryUpdateMetadataTypeAsync(It.IsAny<MetadataType>()), Times.Exactly(2));
            updatedMetadataTypeOne.Should().BeEquivalentTo(new MetadataModel() { Label = "testUpdated", Name = "TEST" });
            updatedMetadataTypeTwo.Should().BeEquivalentTo(new MetadataModel() { Label = "testTwoUpdated", Name = "TESTTWO" });
            result.CreatedMetadata.Should().BeEmpty();
            result.UpdatedMetadata.Should().HaveCount(2);
            result.DeletedMetadata.Should().BeEmpty();
            result.UpdatedMetadata.Should().ContainSingle(x => x.Payload.Name == "TEST" && x.Payload.Label == "testUpdated");
            result.UpdatedMetadata.Should().ContainSingle(x => x.Payload.Name == "TESTTWO" && x.Payload.Label == "testTwoUpdated");
        }

        [Fact]
        public void Update_SyncMetadata_SingleNameChange()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(new List<MetadataModel>() { new MetadataModel() { Label = "testUpdated", Name = "TESTUPDATED" } });

            var mayanRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult(new MetadataType[1] { new MetadataType() { Label = "test", Name = "TEST" } })));
            MetadataType createdMetadataType = null;
            mayanRepository.Setup(x => x.TryCreateMetadataTypeAsync(It.Is<MetadataType>(x => x.Name == "TESTUPDATED")))
                .Callback<MetadataType>(x => createdMetadataType = x).Returns(Task.FromResult(new ExternalResult<MetadataType>() { HttpStatusCode = System.Net.HttpStatusCode.Created, Payload = new MetadataType() { Label = "test", Name = "TEST" } }));

            // Act
            var result = service.SyncMayanMetadataTypes(model);

            // Assert
            mayanRepository.Verify(x => x.TryCreateMetadataTypeAsync(It.IsAny<MetadataType>()), Times.Once());
            createdMetadataType.Should().BeEquivalentTo(new MetadataModel() { Label = "testUpdated", Name = "TESTUPDATED" });
            result.CreatedMetadata.Should().HaveCount(1);
            result.UpdatedMetadata.Should().BeEmpty();
            result.DeletedMetadata.Should().BeEmpty();
        }

        [Fact]
        public void Update_SyncMetadata_NoLabelChange()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(new List<MetadataModel>() { new MetadataModel() { Label = "test", Name = "TEST" } });

            var mayanRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult(new MetadataType[1] { new MetadataType() { Label = "test", Name = "TEST" } })));
            mayanRepository.Setup(x => x.TryUpdateMetadataTypeAsync(It.Is<MetadataType>(x => x.Name == "TEST")))
                .Returns(Task.FromResult(new ExternalResult<MetadataType>() { HttpStatusCode = System.Net.HttpStatusCode.OK, Payload = new MetadataType() { Label = "testUpdated", Name = "TEST" } }));
            mayanRepository.Setup(x => x.TryCreateMetadataTypeAsync(It.Is<MetadataType>(x => x.Name == "TEST")))
                .Returns(Task.FromResult(new ExternalResult<MetadataType>() { HttpStatusCode = System.Net.HttpStatusCode.OK, Payload = new MetadataType() { Label = "testUpdated", Name = "TEST" } }));

            // Act
            var result = service.SyncMayanMetadataTypes(model);

            // Assert
            mayanRepository.Verify(x => x.TryCreateMetadataTypeAsync(It.IsAny<MetadataType>()), Times.Never());
            mayanRepository.Verify(x => x.TryUpdateMetadataTypeAsync(It.IsAny<MetadataType>()), Times.Never());
            result.CreatedMetadata.Should().BeEmpty();
            result.DeletedMetadata.Should().BeEmpty();
            result.UpdatedMetadata.Should().BeEmpty();
        }
        #endregion

        #region SyncPimsDocumentTypes

        [Fact]
        public void Add_SyncPimsDocumentTypes_Single()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() { new DocumentTypeModel() { Name = "TEST", Label = "test"} });

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>());
            pimsDocumentRepository.Setup(x => x.Add(It.IsAny<PimsDocumentTyp>())).Returns(new PimsDocumentTyp());

            // Act
            var result = service.SyncPimsDocumentTypes(model);

            // Assert
            pimsDocumentRepository.Verify(x => x.Add(It.IsAny<PimsDocumentTyp>()), Times.Once());
            result.Added.Should().HaveCount(1);
            result.Deleted.Should().BeEmpty();
            result.Updated.Should().BeEmpty();
        }

        [Fact]
        public void Add_SyncPimsDocumentTypes_Multiple()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() { new DocumentTypeModel() { Name = "TEST", Label = "test" }, new DocumentTypeModel() { Name = "TESTTWO", Label = "testTwo" } });

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>());
            pimsDocumentRepository.Setup(x => x.Add(It.IsAny<PimsDocumentTyp>())).Returns(new PimsDocumentTyp());

            // Act
            var result = service.SyncPimsDocumentTypes(model);

            // Assert
            pimsDocumentRepository.Verify(x => x.Add(It.IsAny<PimsDocumentTyp>()), Times.Exactly(2));
            result.Added.Should().HaveCount(2);
            result.Deleted.Should().BeEmpty();
            result.Updated.Should().BeEmpty();
        }

        [Fact]
        public void RemoveLingering_SyncPimsDocumentTypes_Single()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel();
            model.RemoveLingeringDocumentTypes = true;

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { new PimsDocumentTyp() { DocumentType = "TEST" } });

            // Act
            var result = service.SyncPimsDocumentTypes(model);

            // Assert
            //TODO: mock and assert that the document types are being disabled.
            result.Deleted.Should().BeEmpty();
            result.Added.Should().BeEmpty();
            result.Updated.Should().BeEmpty();
        }

        [Fact]
        public void RemoveLingering_SyncPimsDocumentTypes_Multiple()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel();
            model.RemoveLingeringDocumentTypes = true;

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { new PimsDocumentTyp() { DocumentType = "TEST" }, new PimsDocumentTyp() { DocumentType = "TESTTWO" } });

            // Act
            var result = service.SyncPimsDocumentTypes(model);

            // Assert
            //TODO: mock and assert that the document types are being disabled.
            result.Deleted.Should().BeEmpty();
            result.Added.Should().BeEmpty();
            result.Updated.Should().BeEmpty();
        }

        [Fact]
        public void RemoveLingering_SyncPimsDocumentTypes_DisableNoneIfFalse()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel();
            model.RemoveLingeringDocumentTypes = false;

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { new PimsDocumentTyp() { DocumentType = "TEST" }, new PimsDocumentTyp() { DocumentType = "TESTTWO" } });

            // Act
            var result = service.SyncPimsDocumentTypes(model);

            // Assert
            //TODO: mock and assert that the document types are being disabled.
            result.Deleted.Should().BeEmpty();
            result.Added.Should().BeEmpty();
            result.Updated.Should().BeEmpty();
        }

        [Fact]
        public void Update_SyncPimsDocumentTypes_Single()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() { new DocumentTypeModel() { Name = "TEST", Label = "test updated" } });

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { new PimsDocumentTyp() { DocumentType = "TEST" } });
            PimsDocumentTyp updatedDocumentTypeOne = null;
            pimsDocumentRepository.Setup(x => x.Update(It.IsAny<PimsDocumentTyp>())).Callback<PimsDocumentTyp>(x => updatedDocumentTypeOne = x).Returns(new PimsDocumentTyp());

            // Act
            var result = service.SyncPimsDocumentTypes(model);

            // Assert
            pimsDocumentRepository.Verify(x => x.Update(It.IsAny<PimsDocumentTyp>()), Times.Once());
            result.Added.Should().BeEmpty();
            result.Deleted.Should().BeEmpty();
            result.Updated.Should().HaveCount(1);
            updatedDocumentTypeOne.DocumentTypeDescription.Should().Be("test updated");
        }

        [Fact]
        public void Update_SyncPimsDocumentTypes_Multiple()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() { new DocumentTypeModel() { Name = "TEST", Label = "test updated" }, new DocumentTypeModel() { Name = "TESTTWO", Label = "test two updated" } });

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            PimsDocumentTyp updatedDocumentTypeOne = null;
            PimsDocumentTyp updatedDocumentTypeTwo = null;
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { new PimsDocumentTyp() { DocumentType = "TEST" }, new PimsDocumentTyp() { DocumentType = "TESTTWO" } });
            pimsDocumentRepository.Setup(x => x.Update(It.Is<PimsDocumentTyp>(x => x.DocumentType == "TEST"))).Callback<PimsDocumentTyp>(x => updatedDocumentTypeOne = x).Returns(new PimsDocumentTyp());
            pimsDocumentRepository.Setup(x => x.Update(It.Is<PimsDocumentTyp>(x => x.DocumentType == "TESTTWO"))).Callback<PimsDocumentTyp>(x => updatedDocumentTypeTwo = x).Returns(new PimsDocumentTyp());

            // Act
            var result = service.SyncPimsDocumentTypes(model);

            // Assert
            pimsDocumentRepository.Verify(x => x.Update(It.IsAny<PimsDocumentTyp>()), Times.Exactly(2));
            result.Added.Should().BeEmpty();
            result.Deleted.Should().BeEmpty();
            result.Updated.Should().HaveCount(2);
            updatedDocumentTypeOne.DocumentTypeDescription.Should().Be("test updated");
            updatedDocumentTypeTwo.DocumentTypeDescription.Should().Be("test two updated");
        }

        [Fact]
        public void Update_SyncPimsDocumentTypes_NoLabelChange()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() { new DocumentTypeModel() { Name = "TEST", Label = "test" } });

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { new PimsDocumentTyp() { DocumentType = "TEST", DocumentTypeDescription = "test" } });
            pimsDocumentRepository.Setup(x => x.Add(It.IsAny<PimsDocumentTyp>())).Returns(new PimsDocumentTyp());
            pimsDocumentRepository.Setup(x => x.Update(It.IsAny<PimsDocumentTyp>())).Returns(new PimsDocumentTyp());

            // Act
            var result = service.SyncPimsDocumentTypes(model);

            // Assert
            pimsDocumentRepository.Verify(x => x.Add(It.IsAny<PimsDocumentTyp>()), Times.Never());
            pimsDocumentRepository.Verify(x => x.Update(It.IsAny<PimsDocumentTyp>()), Times.Never());
            result.Added.Should().BeEmpty();
            result.Deleted.Should().BeEmpty();
            result.Updated.Should().BeEmpty();
        }

        [Fact]
        public void Update_SyncPimsDocumentTypes_SingleNameChange()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() { new DocumentTypeModel() { Name = "TESTUPDATE", Label = "test" } });

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { new PimsDocumentTyp() { DocumentType = "TEST", DocumentTypeDescription = "test" } });
            pimsDocumentRepository.Setup(x => x.Add(It.IsAny<PimsDocumentTyp>())).Returns(new PimsDocumentTyp());
            pimsDocumentRepository.Setup(x => x.Update(It.IsAny<PimsDocumentTyp>())).Returns(new PimsDocumentTyp());

            // Act
            var result = service.SyncPimsDocumentTypes(model);

            // Assert
            pimsDocumentRepository.Verify(x => x.Add(It.IsAny<PimsDocumentTyp>()), Times.Once());
            pimsDocumentRepository.Verify(x => x.Update(It.IsAny<PimsDocumentTyp>()), Times.Never());
            result.Added.Should().HaveCount(1);
            result.Deleted.Should().BeEmpty();
            result.Updated.Should().BeEmpty();
        }
        #endregion

        #region SyncPimsToMayan
        [Fact]
        public void Add_SyncPimsToMayan_SingleType()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() { new DocumentTypeModel() { Name = "TEST", Label = "test" } });

            var mayanRepository = _helper.GetService<Mock<IEdmsDocumentRepository>>();
            mayanRepository.Setup(x => x.TryGetDocumentTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeResult()));
            mayanRepository.Setup(x => x.TryGetDocumentTypeMetadataTypesAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeMetadataTypeResult()));
            mayanRepository.Setup(x => x.TryCreateDocumentTypeAsync(It.IsAny<DocumentType>()))
                .Returns(Task.FromResult(new ExternalResult<DocumentType>() { HttpStatusCode = System.Net.HttpStatusCode.Created, Payload = new DocumentType() { Id = 1 } }));

            var mayanMetadataRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanMetadataRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult()));

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { new PimsDocumentTyp() { DocumentType = "TEST", DocumentTypeDescription = "test" } });

            // Act
            var result = service.SyncPimsToMayan(model);

            // Assert
            mayanRepository.Verify(x => x.TryCreateDocumentTypeAsync(It.Is<DocumentType>(x => x.Label == "test")), Times.Once());
            pimsDocumentRepository.Verify(x => x.Update(It.Is<PimsDocumentTyp>(x => x.MayanId == 1)), Times.Once());
            result.CreatedDocumentType.Should().HaveCount(1);
            result.DeletedDocumentTypeMetadataType.Should().BeEmpty();
            result.LinkedDocumentMetadataTypes.Should().BeEmpty();
            result.DeletedDocumentType.Should().BeEmpty();
            result.UpdatedDocumentType.Should().BeEmpty();
        }

        [Fact]
        public void Add_SyncPimsToMayan_SingleTypeWithMetadata()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);
            DocumentType documentType = new DocumentType() { Label = "test" };
            MetadataType metadataType = new MetadataType() { Name = "TESTMETADATA", Label = "test metadata" };

            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() {
                new DocumentTypeModel() { Name = "TEST", Label = "test", MetadataTypes = new List<DocumentMetadataTypeModel>() {
                    new DocumentMetadataTypeModel() { Name = "TESTMETADATA", Label = "test metadata" } } } });

            var mayanRepository = _helper.GetService<Mock<IEdmsDocumentRepository>>();
            //on the first call mayan would not have the new document type. On the second call it should have been added.
            mayanRepository.SetupSequence(x => x.TryGetDocumentTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeResult()))
                .Returns(Task.FromResult(CreateDocumentTypeResult(documentTypes: new DocumentType[1] { new DocumentType() { Label = "test", Id = 1 } })));

            mayanRepository.Setup(x => x.TryGetDocumentTypeMetadataTypesAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeMetadataTypeResult()));
            mayanRepository.Setup(x => x.TryCreateDocumentTypeAsync(It.IsAny<DocumentType>()))
                .Returns(Task.FromResult(new ExternalResult<DocumentType>() { HttpStatusCode = System.Net.HttpStatusCode.Created, Payload = new DocumentType() { Id = 1 } }));

            var mayanMetadataRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanMetadataRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult(new MetadataType[1] { metadataType })));

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { new PimsDocumentTyp() { DocumentType = "TEST", DocumentTypeDescription = "test", MayanId = 1 } });

            // Act
            var result = service.SyncPimsToMayan(model);

            // Assert
            mayanRepository.Verify(x => x.TryCreateDocumentTypeAsync(It.Is<DocumentType>(x => x.Label == "test")), Times.Once());
            pimsDocumentRepository.Verify(x => x.Update(It.Is<PimsDocumentTyp>(x => x.MayanId == 1)), Times.Once());
            result.CreatedDocumentType.Should().HaveCount(1);
            result.LinkedDocumentMetadataTypes.Should().HaveCount(1);
            result.DeletedDocumentTypeMetadataType.Should().BeEmpty();
            result.DeletedDocumentType.Should().BeEmpty();
            result.UpdatedDocumentType.Should().BeEmpty();
        }

        [Fact]
        public void Add_SyncPimsToMayan_SingleTypeWithMetadata_MetadataDoesNotExist()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() {
                new DocumentTypeModel() { Name = "TEST", Label = "test", MetadataTypes = new List<DocumentMetadataTypeModel>() {
                    new DocumentMetadataTypeModel() { Name = "TESTMETADATA", Label = "test metadata" } } } });

            var mayanRepository = _helper.GetService<Mock<IEdmsDocumentRepository>>();
            //on the first call mayan would not have the new document type. On the second call it should have been added.
            mayanRepository.SetupSequence(x => x.TryGetDocumentTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeResult()))
                .Returns(Task.FromResult(CreateDocumentTypeResult(documentTypes: new DocumentType[1] { new DocumentType() { Label = "test", Id = 1 } })));

            mayanRepository.Setup(x => x.TryGetDocumentTypeMetadataTypesAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeMetadataTypeResult()));
            mayanRepository.Setup(x => x.TryCreateDocumentTypeAsync(It.IsAny<DocumentType>()))
                .Returns(Task.FromResult(new ExternalResult<DocumentType>() { HttpStatusCode = System.Net.HttpStatusCode.Created, Payload = new DocumentType() { Id = 1 } }));

            var mayanMetadataRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanMetadataRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult()));

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { new PimsDocumentTyp() { DocumentType = "TEST", DocumentTypeDescription = "test", MayanId = 1 } });

            // Act
            var result = service.SyncPimsToMayan(model);

            // Assert
            mayanRepository.Verify(x => x.TryCreateDocumentTypeAsync(It.Is<DocumentType>(x => x.Label == "test")), Times.Once());
            result.CreatedDocumentType.Should().HaveCount(1);
            result.LinkedDocumentMetadataTypes.Should().HaveCount(1);
            result.LinkedDocumentMetadataTypes.FirstOrDefault().Message.Should().Be("Metadata with name [TESTMETADATA] does not exist in Mayan");
            result.DeletedDocumentTypeMetadataType.Should().BeEmpty();
            result.DeletedDocumentType.Should().BeEmpty();
            result.UpdatedDocumentType.Should().BeEmpty();
        }

        [Fact]
        public void Add_SyncPimsToMayan_MultipleTypesWithMetadata()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);
            DocumentType documentTypeOne = new DocumentType() { Label = "test", Id = 1 };
            MetadataType metadataTypeOne = new MetadataType() { Name = "TESTMETADATA", Label = "test metadata" };
            DocumentType documentTypeTwo = new DocumentType() { Label = "test two", Id = 2 };
            MetadataType metadataTypeTwo = new MetadataType() { Name = "TESTTWOMETADATA", Label = "test two metadata" };

            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() {
                new DocumentTypeModel() { Name = "TEST", Label = "test", MetadataTypes = new List<DocumentMetadataTypeModel>() {
                    new DocumentMetadataTypeModel() { Name = "TESTMETADATA", Label = "test metadata" } } },
                new DocumentTypeModel() { Name = "TESTTWO", Label = "test two", MetadataTypes = new List<DocumentMetadataTypeModel>() {
                    new DocumentMetadataTypeModel() { Name = "TESTTWOMETADATA", Label = "test two metadata" } } } });

            var mayanRepository = _helper.GetService<Mock<IEdmsDocumentRepository>>();
            //on the first call mayan would not have the new document type. On the second call it should have been added.
            mayanRepository.SetupSequence(x => x.TryGetDocumentTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeResult()))
                .Returns(Task.FromResult(CreateDocumentTypeResult(documentTypes: new DocumentType[2] { documentTypeOne, documentTypeTwo })));

            mayanRepository.Setup(x => x.TryGetDocumentTypeMetadataTypesAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeMetadataTypeResult()));
            mayanRepository.SetupSequence(x => x.TryCreateDocumentTypeAsync(It.IsAny<DocumentType>()))
                .Returns(Task.FromResult(new ExternalResult<DocumentType>() { HttpStatusCode = System.Net.HttpStatusCode.Created, Payload = new DocumentType() { Id = 1 } }))
                .Returns(Task.FromResult(new ExternalResult<DocumentType>() { HttpStatusCode = System.Net.HttpStatusCode.Created, Payload = new DocumentType() { Id = 2 } }));

            var mayanMetadataRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanMetadataRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult(new MetadataType[2] { metadataTypeOne, metadataTypeTwo })));

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() {
                new PimsDocumentTyp() { DocumentType = "TEST", DocumentTypeDescription = "test", MayanId = 1 },
                new PimsDocumentTyp() { DocumentType = "TESTTWO", DocumentTypeDescription = "test two", MayanId = 2 } });

            // Act
            var result = service.SyncPimsToMayan(model);

            // Assert
            mayanRepository.Verify(x => x.TryCreateDocumentTypeAsync(It.Is<DocumentType>(x => x.Label == "test")), Times.Once());
            mayanRepository.Verify(x => x.TryCreateDocumentTypeAsync(It.Is<DocumentType>(x => x.Label == "test two")), Times.Once());
            pimsDocumentRepository.Verify(x => x.Update(It.Is<PimsDocumentTyp>(x => x.MayanId == 1)), Times.Once());
            pimsDocumentRepository.Verify(x => x.Update(It.Is<PimsDocumentTyp>(x => x.MayanId == 2)), Times.Once());
            result.CreatedDocumentType.Should().HaveCount(2);
            result.LinkedDocumentMetadataTypes.Should().HaveCount(2);
            result.DeletedDocumentTypeMetadataType.Should().BeEmpty();
            result.DeletedDocumentType.Should().BeEmpty();
            result.UpdatedDocumentType.Should().BeEmpty();
        }

        [Fact]
        public void Add_SyncPimsToMayan_SingleTypeWithMetadata_UpdateRequired()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);
            DocumentType documentType = new DocumentType() { Label = "test" };
            MetadataType metadataType = new MetadataType() { Name = "TESTMETADATA", Label = "test metadata" };

            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() {
                new DocumentTypeModel() { Name = "TEST", Label = "test", MetadataTypes = new List<DocumentMetadataTypeModel>() {
                    new DocumentMetadataTypeModel() { Name = "TESTMETADATA", Label = "test metadata", Required = true } } } });

            var mayanRepository = _helper.GetService<Mock<IEdmsDocumentRepository>>();
            //on the first call mayan would not have the new document type. On the second call it should have been added.
            mayanRepository.SetupSequence(x => x.TryGetDocumentTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeResult()))
                .Returns(Task.FromResult(CreateDocumentTypeResult(documentTypes: new DocumentType[1] { new DocumentType() { Label = "test", Id = 1 } })));

            mayanRepository.Setup(x => x.TryGetDocumentTypeMetadataTypesAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeMetadataTypeResult(new DocumentTypeMetadataType() { DocumentType = documentType, MetadataType = metadataType, Required = false })));
            mayanRepository.Setup(x => x.TryCreateDocumentTypeAsync(It.IsAny<DocumentType>()))
                .Returns(Task.FromResult(new ExternalResult<DocumentType>()));

            var mayanMetadataRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanMetadataRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult(new MetadataType[1] { metadataType })));

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { new PimsDocumentTyp() { DocumentType = "TEST", DocumentTypeDescription = "test", MayanId = 1 } });

            // Act
            var result = service.SyncPimsToMayan(model);

            // Assert
            mayanRepository.Verify(x => x.TryCreateDocumentTypeAsync(It.Is<DocumentType>(x => x.Label == "test")), Times.Once());
            result.CreatedDocumentType.Should().HaveCount(1);
            result.LinkedDocumentMetadataTypes.Should().HaveCount(1);
            result.DeletedDocumentTypeMetadataType.Should().BeEmpty();
            result.DeletedDocumentType.Should().BeEmpty();
            result.UpdatedDocumentType.Should().BeEmpty();
        }

        [Fact]
        public void Add_SyncPimsToMayan_DeleteLinkedMetadata()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);
            DocumentType documentType = new DocumentType() { Label = "test" };
            MetadataType metadataType = new MetadataType() { Name = "TESTMETADATA", Label = "test metadata" };

            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() {
                new DocumentTypeModel() { Name = "TEST", Label = "test", MetadataTypes = new List<DocumentMetadataTypeModel>() } });

            var mayanRepository = _helper.GetService<Mock<IEdmsDocumentRepository>>();
            //on the first call mayan would not have the new document type. On the second call it should have been added.
            mayanRepository.SetupSequence(x => x.TryGetDocumentTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeResult()))
                .Returns(Task.FromResult(CreateDocumentTypeResult(documentTypes: new DocumentType[1] { new DocumentType() { Label = "test", Id = 1 } })));

            mayanRepository.Setup(x => x.TryGetDocumentTypeMetadataTypesAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeMetadataTypeResult(new DocumentTypeMetadataType() { DocumentType = documentType, MetadataType = metadataType, Required = false })));
            mayanRepository.Setup(x => x.TryCreateDocumentTypeAsync(It.IsAny<DocumentType>()))
                .Returns(Task.FromResult(new ExternalResult<DocumentType>()));

            var mayanMetadataRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanMetadataRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult(new MetadataType[1] { metadataType })));

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { new PimsDocumentTyp() { DocumentType = "TEST", DocumentTypeDescription = "test", MayanId = 1 } });

            // Act
            var result = service.SyncPimsToMayan(model);

            // Assert
            mayanRepository.Verify(x => x.TryCreateDocumentTypeAsync(It.Is<DocumentType>(x => x.Label == "test")), Times.Once());
            result.CreatedDocumentType.Should().HaveCount(1);
            result.LinkedDocumentMetadataTypes.Should().BeEmpty();
            result.DeletedDocumentTypeMetadataType.Should().HaveCount(1);
            result.DeletedDocumentType.Should().BeEmpty();
            result.UpdatedDocumentType.Should().BeEmpty();
        }

        [Fact]
        public void Update_SyncPimsToMayan_SingleType()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() { new DocumentTypeModel() { Name = "TEST", Label = "test updated", MetadataTypes = new List<DocumentMetadataTypeModel>() } });

            var mayanRepository = _helper.GetService<Mock<IEdmsDocumentRepository>>();
            mayanRepository.Setup(x => x.TryGetDocumentTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeResult(new DocumentType[1] { new DocumentType() { Id = 1 } })));
            mayanRepository.Setup(x => x.TryGetDocumentTypeMetadataTypesAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeMetadataTypeResult()));
            mayanRepository.Setup(x => x.TryUpdateDocumentTypeAsync(It.IsAny<DocumentType>()))
                .Returns(Task.FromResult(new ExternalResult<DocumentType>()));

            var mayanMetadataRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanMetadataRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult()));

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { new PimsDocumentTyp() { DocumentType = "TEST", DocumentTypeDescription = "test updated", MayanId = 1 } });

            // Act
            var result = service.SyncPimsToMayan(model);

            // Assert
            mayanRepository.Verify(x => x.TryUpdateDocumentTypeAsync(It.Is<DocumentType>(x => x.Label == "test updated")), Times.Once());
            result.CreatedDocumentType.Should().BeEmpty();
            result.DeletedDocumentTypeMetadataType.Should().BeEmpty();
            result.LinkedDocumentMetadataTypes.Should().BeEmpty();
            result.DeletedDocumentType.Should().BeEmpty();
            result.UpdatedDocumentType.Should().HaveCount(1);
        }

        [Fact]
        public void Update_SyncPimsToMayan_SingleTypeWithMetadata()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            MetadataType metadataType = new MetadataType() { Name = "TESTMETADATA", Label = "test metadata" };
            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() { new DocumentTypeModel() { Name = "TEST", Label = "test updated",
                MetadataTypes = new List<DocumentMetadataTypeModel>() { new DocumentMetadataTypeModel() { Name = "TESTMETADATA", Label = "test metadata" } } } });

            var mayanRepository = _helper.GetService<Mock<IEdmsDocumentRepository>>();
            mayanRepository.SetupSequence(x => x.TryGetDocumentTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeResult(new DocumentType[1] { new DocumentType() { Id = 1 } })))
                .Returns(Task.FromResult(CreateDocumentTypeResult(documentTypes: new DocumentType[1] { new DocumentType() { Label = "test", Id = 1 } })));
            mayanRepository.Setup(x => x.TryGetDocumentTypeMetadataTypesAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeMetadataTypeResult()));
            mayanRepository.Setup(x => x.TryUpdateDocumentTypeAsync(It.IsAny<DocumentType>()))
                .Returns(Task.FromResult(new ExternalResult<DocumentType>()));

            var mayanMetadataRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanMetadataRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult(new MetadataType[1] { metadataType })));

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { new PimsDocumentTyp() { DocumentType = "TEST", DocumentTypeDescription = "test updated", MayanId = 1 } });

            // Act
            var result = service.SyncPimsToMayan(model);

            // Assert
            mayanRepository.Verify(x => x.TryUpdateDocumentTypeAsync(It.Is<DocumentType>(x => x.Label == "test updated")), Times.Once());
            result.CreatedDocumentType.Should().BeEmpty();
            result.DeletedDocumentTypeMetadataType.Should().BeEmpty();
            result.LinkedDocumentMetadataTypes.Should().HaveCount(1);
            result.DeletedDocumentType.Should().BeEmpty();
            result.UpdatedDocumentType.Should().HaveCount(1);
        }

        [Fact]
        public void RemoveLingering_SyncPimsToMayan_Success()
        {
            // Arrange
            var service = CreateDocumentySyncServiceWithPermissions(Permissions.DocumentAdmin);

            SyncModel model = CreateSyncModel(documentTypeModels: new List<DocumentTypeModel>() { new DocumentTypeModel() { Name = "TEST", Label = "test updated", MetadataTypes = new List<DocumentMetadataTypeModel>() } });
            model.RemoveLingeringDocumentTypes = true;

            var mayanRepository = _helper.GetService<Mock<IEdmsDocumentRepository>>();
            mayanRepository.Setup(x => x.TryGetDocumentTypeMetadataTypesAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeMetadataTypeResult()));
            mayanRepository.Setup(x => x.TryGetDocumentTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateDocumentTypeResult(new DocumentType[1] { new DocumentType() { Id = 1 } })));
            mayanRepository.Setup(x => x.TryDeleteDocumentTypeAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(new ExternalResult<string>() { Payload = "deleted document type" } ));

            var mayanMetadataRepository = _helper.GetService<Mock<IEdmsMetadataRepository>>();
            mayanMetadataRepository.Setup(x => x.TryGetMetadataTypesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.FromResult(CreateMetadataResult()));

            var pimsDocumentRepository = _helper.GetService<Mock<IDocumentTypeRepository>>();
            pimsDocumentRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>() { });

            // Act
            var result = service.SyncPimsToMayan(model);

            // Assert
            mayanRepository.Verify(x => x.TryDeleteDocumentTypeAsync(It.IsAny<long>()), Times.Once());
            result.CreatedDocumentType.Should().BeEmpty();
            result.DeletedDocumentTypeMetadataType.Should().BeEmpty();
            result.LinkedDocumentMetadataTypes.Should().BeEmpty();
            result.DeletedDocumentType.Should().HaveCount(1);
            result.UpdatedDocumentType.Should().BeEmpty();
        }
        #endregion
        #endregion
    }
}
