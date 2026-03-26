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
using Pims.Api.Models.Concepts.Document;

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

        [Fact]
        public void GetPageDeep_AppliesTargetAccessRules_AndPreservesNonTargetDocuments()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);
            var context = helper.CreatePimsContext(user, true);

            var documentType = new PimsDocumentTyp
            {
                DocumentTypeId = 1,
                DocumentType = "TEST",
                DocumentTypeDescription = "Test",
                AppCreateUserid = "test",
                AppCreateUserDirectory = "test",
                AppLastUpdateUserid = "test",
                AppLastUpdateUserDirectory = "test",
                DbCreateUserid = "test",
                DbLastUpdateUserid = "test",
            };

            var templateType = new PimsDocumentTyp
            {
                DocumentTypeId = 999,
                DocumentType = "CDOGTEMP",
                DocumentTypeDescription = "Template",
                AppCreateUserid = "test",
                AppCreateUserDirectory = "test",
                AppLastUpdateUserid = "test",
                AppLastUpdateUserDirectory = "test",
                DbCreateUserid = "test",
                DbLastUpdateUserid = "test",
            };

            var contractorPersonId = 7L;
            var projectWithMember = EntityHelper.CreateProject(1, "PRJ-1", "Project with member");
            projectWithMember.RegionCode = 1;
            projectWithMember.PimsProjectPeople.Add(new PimsProjectPerson { PersonId = contractorPersonId });

            var projectWithoutMember = EntityHelper.CreateProject(2, "PRJ-2", "Project without member");
            projectWithoutMember.RegionCode = 1;

            var acqAllowed = EntityHelper.CreateAcquisitionFile(11);
            acqAllowed.RegionCode = 1;
            acqAllowed.PimsAcquisitionFileTeams.Add(new PimsAcquisitionFileTeam { PersonId = contractorPersonId });

            var mgmtAllowed = EntityHelper.CreateManagementFile(12);
            mgmtAllowed.RegionCode = 1;
            mgmtAllowed.Project = projectWithMember;

            var researchAllowed = EntityHelper.CreateResearchFile(13);
            researchAllowed.PimsResearchFileProjects.Add(new PimsResearchFileProject { Project = projectWithMember });

            var dispositionDenied = EntityHelper.CreateDispositionFile(14);
            dispositionDenied.RegionCode = 1;
            dispositionDenied.PimsDispositionFileTeams.Add(new PimsDispositionFileTeam { PersonId = 99 });

            var leaseDenied = EntityHelper.CreateLease(15, addProperty: false);
            leaseDenied.RegionCode = 2;

            var researchDenied = EntityHelper.CreateResearchFile(16);
            researchDenied.PimsResearchFileProjects.Add(new PimsResearchFileProject { Project = projectWithoutMember });

            var acquisitionDoc = EntityHelper.CreateDocument("acq-allowed", 101);
            acquisitionDoc.DocumentTypeId = 1;
            acquisitionDoc.DocumentType = documentType;
            acquisitionDoc.PimsAcquisitionFileDocuments.Add(new PimsAcquisitionFileDocument { AcquisitionFile = acqAllowed });

            var managementDoc = EntityHelper.CreateDocument("mgmt-allowed", 102);
            managementDoc.DocumentTypeId = 1;
            managementDoc.DocumentType = documentType;
            managementDoc.PimsManagementFileDocuments.Add(new PimsManagementFileDocument { ManagementFile = mgmtAllowed });

            var researchDoc = EntityHelper.CreateDocument("research-allowed", 103);
            researchDoc.DocumentTypeId = 1;
            researchDoc.DocumentType = documentType;
            researchDoc.PimsResearchFileDocuments.Add(new PimsResearchFileDocument { ResearchFile = researchAllowed });

            var dispositionDoc = EntityHelper.CreateDocument("disp-denied", 104);
            dispositionDoc.DocumentTypeId = 1;
            dispositionDoc.DocumentType = documentType;
            dispositionDoc.PimsDispositionFileDocuments.Add(new PimsDispositionFileDocument { DispositionFile = dispositionDenied });

            var leaseDoc = EntityHelper.CreateDocument("lease-denied", 105);
            leaseDoc.DocumentTypeId = 1;
            leaseDoc.DocumentType = documentType;
            leaseDoc.PimsLeaseDocuments.Add(new PimsLeaseDocument { Lease = leaseDenied });

            var researchDeniedDoc = EntityHelper.CreateDocument("research-denied", 106);
            researchDeniedDoc.DocumentTypeId = 1;
            researchDeniedDoc.DocumentType = documentType;
            researchDeniedDoc.PimsResearchFileDocuments.Add(new PimsResearchFileDocument { ResearchFile = researchDenied });

            var nonTargetProjectDoc = EntityHelper.CreateDocument("project-non-target", 107);
            nonTargetProjectDoc.DocumentTypeId = 1;
            nonTargetProjectDoc.DocumentType = documentType;
            nonTargetProjectDoc.PimsProjectDocuments.Add(new PimsProjectDocument { Project = projectWithoutMember });

            context.PimsDocumentTyps.AddRange(documentType, templateType);
            context.PimsDocuments.AddRange(acquisitionDoc, managementDoc, researchDoc, dispositionDoc, leaseDoc, researchDeniedDoc, nonTargetProjectDoc);
            context.CommitTransaction();

            var repository = helper.CreateRepository<DocumentRepository>(user);

            // Act
            var result = repository.GetPageDeep(
                new DocumentSearchFilterModel { Page = 1, Quantity = 20 },
                new DocumentAccessContext
                {
                    UserRegions = new HashSet<short> { 1 },
                    ContractorPersonId = contractorPersonId,
                    CanViewAcquisitionFiles = true,
                    CanViewDispositionFiles = true,
                    CanViewLeases = true,
                    CanViewManagementFiles = true,
                    CanViewResearchFiles = true,
                });

            // Assert
            result.Items.Should().Contain(d => d.FileName == "acq-allowed");
            result.Items.Should().Contain(d => d.FileName == "mgmt-allowed");
            result.Items.Should().Contain(d => d.FileName == "research-allowed");
            result.Items.Should().Contain(d => d.FileName == "project-non-target");

            result.Items.Should().NotContain(d => d.FileName == "disp-denied");
            result.Items.Should().NotContain(d => d.FileName == "lease-denied");
            result.Items.Should().NotContain(d => d.FileName == "research-denied");
        }

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
            document.PimsMgmtActivityDocuments = new List<PimsMgmtActivityDocument>() { new PimsMgmtActivityDocument() };
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
            document.PimsManagementFileDocuments = new List<PimsManagementFileDocument>() { new PimsManagementFileDocument() };
            document.PimsMgmtActivityDocuments = new List<PimsMgmtActivityDocument>() { new PimsMgmtActivityDocument() };
            document.PimsDispositionFileDocuments = new List<PimsDispositionFileDocument>() { new PimsDispositionFileDocument() };
            document.PimsFormTypes = new List<PimsFormType>() { new PimsFormType() { FormTypeCode = "FORM", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "Form" } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(document);
            var repository = helper.CreateRepository<DocumentRepository>(user);

            // Act
            var result = repository.DocumentRelationshipCount(1);

            // Assert
            result.Should().Be(8);
        }
        #endregion

        #region GetPageDeep
        [Fact]
        public void GetPageDeep_ExcludesFileType_WhenViewPermissionMissing()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);
            var context = helper.CreatePimsContext(user, true);

            var documentType = new PimsDocumentTyp
            {
                DocumentTypeId = 1,
                DocumentType = "TEST",
                DocumentTypeDescription = "Test",
                AppCreateUserid = "test",
                AppCreateUserDirectory = "test",
                AppLastUpdateUserid = "test",
                AppLastUpdateUserDirectory = "test",
                DbCreateUserid = "test",
                DbLastUpdateUserid = "test",
            };

            var acquisitionFile = EntityHelper.CreateAcquisitionFile(11);
            acquisitionFile.RegionCode = 1;

            var lease = EntityHelper.CreateLease(12, addProperty: false);
            lease.RegionCode = 1;

            var acquisitionDocument = EntityHelper.CreateDocument("acq-hidden-without-permission", 301);
            acquisitionDocument.DocumentTypeId = 1;
            acquisitionDocument.DocumentType = documentType;
            acquisitionDocument.PimsAcquisitionFileDocuments.Add(new PimsAcquisitionFileDocument { AcquisitionFile = acquisitionFile });

            var leaseDocument = EntityHelper.CreateDocument("lease-visible-with-permission", 302);
            leaseDocument.DocumentTypeId = 1;
            leaseDocument.DocumentType = documentType;
            leaseDocument.PimsLeaseDocuments.Add(new PimsLeaseDocument { Lease = lease });

            var mixedAssociationDocument = EntityHelper.CreateDocument("mixed-visible-with-research", 303);
            mixedAssociationDocument.DocumentTypeId = 1;
            mixedAssociationDocument.DocumentType = documentType;
            mixedAssociationDocument.PimsAcquisitionFileDocuments.Add(new PimsAcquisitionFileDocument { AcquisitionFile = acquisitionFile });

            var researchProject = EntityHelper.CreateProject(20, "PRJ-R", "Research Project");
            researchProject.RegionCode = 1;
            var researchFile = EntityHelper.CreateResearchFile(21);
            researchFile.PimsResearchFileProjects.Add(new PimsResearchFileProject { Project = researchProject });
            mixedAssociationDocument.PimsResearchFileDocuments.Add(new PimsResearchFileDocument { ResearchFile = researchFile });

            context.PimsDocumentTyps.Add(documentType);
            context.PimsDocuments.AddRange(acquisitionDocument, leaseDocument, mixedAssociationDocument);
            context.CommitTransaction();

            var repository = helper.CreateRepository<DocumentRepository>(user);

            // Act
            var result = repository.GetPageDeep(
                new DocumentSearchFilterModel { Page = 1, Quantity = 10 },
                new DocumentAccessContext
                {
                    UserRegions = new HashSet<short> { 1 },
                    CanViewAcquisitionFiles = false,
                    CanViewDispositionFiles = true,
                    CanViewLeases = true,
                    CanViewManagementFiles = true,
                    CanViewResearchFiles = true,
                    CanViewProjects = true,
                    CanViewProperties = true,
                });

            // Assert
            result.Items.Should().Contain(d => d.FileName == "lease-visible-with-permission");
            result.Items.Should().Contain(d => d.FileName == "mixed-visible-with-research");
            result.Items.Should().NotContain(d => d.FileName == "acq-hidden-without-permission");
        }

        [Fact]
        public void GetPageDeep_ExcludesProjectAndProperty_WhenViewPermissionMissing()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);
            var context = helper.CreatePimsContext(user, true);

            var documentType = new PimsDocumentTyp
            {
                DocumentTypeId = 1,
                DocumentType = "TEST",
                DocumentTypeDescription = "Test",
                AppCreateUserid = "test",
                AppCreateUserDirectory = "test",
                AppLastUpdateUserid = "test",
                AppLastUpdateUserDirectory = "test",
                DbCreateUserid = "test",
                DbLastUpdateUserid = "test",
            };

            var project = EntityHelper.CreateProject(30, "PRJ-30", "Project 30");
            project.RegionCode = 1;

            var property = EntityHelper.CreateProperty(40);
            property.RegionCode = 1;

            var projectDocument = EntityHelper.CreateDocument("project-hidden-without-permission", 304);
            projectDocument.DocumentTypeId = 1;
            projectDocument.DocumentType = documentType;
            projectDocument.PimsProjectDocuments.Add(new PimsProjectDocument { Project = project });

            var propertyDocument = EntityHelper.CreateDocument("property-hidden-without-permission", 305);
            propertyDocument.DocumentTypeId = 1;
            propertyDocument.DocumentType = documentType;
            propertyDocument.PimsPropertyDocuments.Add(new PimsPropertyDocument { Property = property });

            context.PimsDocumentTyps.Add(documentType);
            context.PimsDocuments.AddRange(projectDocument, propertyDocument);
            context.CommitTransaction();

            var repository = helper.CreateRepository<DocumentRepository>(user);

            // Act
            var result = repository.GetPageDeep(
                new DocumentSearchFilterModel { Page = 1, Quantity = 10 },
                new DocumentAccessContext
                {
                    UserRegions = new HashSet<short> { 1 },
                    CanViewAcquisitionFiles = true,
                    CanViewDispositionFiles = true,
                    CanViewLeases = true,
                    CanViewManagementFiles = true,
                    CanViewResearchFiles = true,
                    CanViewProjects = false,
                    CanViewProperties = false,
                });

            // Assert
            result.Items.Should().NotContain(d => d.FileName == "project-hidden-without-permission");
            result.Items.Should().NotContain(d => d.FileName == "property-hidden-without-permission");
        }

        [Fact]
        public void GetPageDeep_Filters_By_MayanDocumentIds()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var context = helper.CreatePimsContext(user, true);
            var timestamp = DateTime.UtcNow;
            var templateType = new PimsDocumentTyp()
            {
                DocumentTypeId = 100,
                DocumentType = "CDOGTEMP",
                DocumentTypeDescription = "Template",
                ConcurrencyControlNumber = 1,
                AppCreateTimestamp = timestamp,
                AppCreateUserid = "test",
                AppCreateUserDirectory = "test",
                AppLastUpdateTimestamp = timestamp,
                AppLastUpdateUserid = "test",
                AppLastUpdateUserDirectory = "test",
                DbCreateTimestamp = timestamp,
                DbCreateUserid = "test",
                DbLastUpdateTimestamp = timestamp,
                DbLastUpdateUserid = "test"
            };
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(10);

            var matchingDocument = EntityHelper.CreateDocument(id: 1);
            matchingDocument.MayanId = 101;
            var nonMatchingDocument = EntityHelper.CreateDocument(id: 2);
            nonMatchingDocument.MayanId = 202;

            PimsAcquisitionFileDocument CreateLink(long linkId, PimsDocument document)
            {
                return new PimsAcquisitionFileDocument()
                {
                    AcquisitionFileDocumentId = linkId,
                    AcquisitionFileId = acquisitionFile.AcquisitionFileId,
                    DocumentId = document.DocumentId,
                    AcquisitionFile = acquisitionFile,
                    Document = document,
                    ConcurrencyControlNumber = 1,
                    AppCreateTimestamp = timestamp,
                    AppCreateUserid = "test",
                    AppCreateUserDirectory = "test",
                    AppLastUpdateTimestamp = timestamp,
                    AppLastUpdateUserid = "test",
                    AppLastUpdateUserDirectory = "test",
                    DbCreateTimestamp = timestamp,
                    DbCreateUserid = "test",
                    DbLastUpdateTimestamp = timestamp,
                    DbLastUpdateUserid = "test"
                };
            }

            var matchingLink = CreateLink(1000, matchingDocument);
            var nonMatchingLink = CreateLink(1001, nonMatchingDocument);
            matchingDocument.PimsAcquisitionFileDocuments = new List<PimsAcquisitionFileDocument>() { matchingLink };
            nonMatchingDocument.PimsAcquisitionFileDocuments = new List<PimsAcquisitionFileDocument>() { nonMatchingLink };
            acquisitionFile.PimsAcquisitionFileDocuments.Add(matchingLink);
            acquisitionFile.PimsAcquisitionFileDocuments.Add(nonMatchingLink);

            context.PimsDocumentTyps.Add(templateType);
            context.PimsAcquisitionFiles.Add(acquisitionFile);
            context.PimsDocuments.AddRange(matchingDocument, nonMatchingDocument);
            context.PimsAcquisitionFileDocuments.AddRange(matchingLink, nonMatchingLink);
            context.SaveChanges();

            var repository = helper.CreateRepository<DocumentRepository>(user);
            var filter = new DocumentSearchFilterModel()
            {
                Page = 1,
                Quantity = 10,
                MayanDocumentIds = new[] { matchingDocument.MayanId.Value }
            };

            // Act
            var result = repository.GetPageDeep(
                filter,
                new DocumentAccessContext
                {
                    UserRegions = new HashSet<short> { 1 },
                    CanViewAcquisitionFiles = true,
                    CanViewDispositionFiles = true,
                    CanViewLeases = true,
                    CanViewManagementFiles = true,
                    CanViewResearchFiles = true,
                    CanViewProjects = true,
                    CanViewProperties = true,
                });

            // Assert
            result.Items.Should().HaveCount(1);
            result.Items.First().DocumentId.Should().Be(matchingDocument.DocumentId);
        }
        #endregion

        #endregion
    }
}
