using Xunit;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using System.Diagnostics.CodeAnalysis;
using Pims.Core.Test;

namespace Pims.Dal.Tests.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "lookup")]
    [ExcludeFromCodeCoverage]
    public class LookupRepositoryTest
    {
        [Fact]
        public void LookupRepository_Organization_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllOrganizations();

            // Assert
            Assert.IsType<PimsOrganization[]>(result);
        }

        public void LookupRepository_OrganizationTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllOrganizationTypes();

            // Assert
            Assert.IsType<PimsOrganizationType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllRoles_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllRoles();

            // Assert
            Assert.IsType<PimsRole[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllCountries_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllCountries();

            // Assert
            Assert.IsType<PimsCountry[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllRegions_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllRegions();

            // Assert
            Assert.IsType<PimsRegion[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllDistricts_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllDistricts();

            // Assert
            Assert.IsType<PimsDistrict[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllPropertyTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllPropertyTypes();

            // Assert
            Assert.IsType<PimsPropertyType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllPropertyTenureTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllPropertyTenureTypes();

            // Assert
            Assert.IsType<PimsPropertyTenureType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllPropertyAreaUnitTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllPropertyAreaUnitTypes();

            // Assert
            Assert.IsType<PimsAreaUnitType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllPropertyVolumeUnitTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllPropertyVolumeUnitTypes();

            // Assert
            Assert.IsType<PimsVolumeUnitType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllPropertyManagementPurposeTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllPropertyManagementPurposeTypes();

            // Assert
            Assert.IsType<PimsPropertyPurposeType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllPaymentReceivableTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllPaymentReceivableTypes();

            // Assert
            Assert.IsType<PimsLeasePayRvblType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllLeaseProgramTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllLeaseProgramTypes();

            // Assert
            Assert.IsType<PimsLeaseProgramType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllConsultationTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllConsultationTypes();

            // Assert
            Assert.IsType<PimsConsultationType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllConsultationStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllConsultationStatusTypes();

            // Assert
            Assert.IsType<PimsConsultationStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllLeaseStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllLeaseStatusTypes();

            // Assert
            Assert.IsType<PimsLeaseStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllLeaseTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllLeaseTypes();

            // Assert
            Assert.IsType<PimsLeaseLicenseType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllLeasePurposeTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllLeasePurposeTypes();

            // Assert
            Assert.IsType<PimsLeasePurposeType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllLeaseResponsibilityTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllLeaseResponsibilityTypes();

            // Assert
            Assert.IsType<PimsLeaseResponsibilityType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllLeaseInitiatorTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllLeaseInitiatorTypes();

            // Assert
            Assert.IsType<PimsLeaseInitiatorType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllLeasePeriodStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllLeasePeriodStatusTypes();

            // Assert
            Assert.IsType<PimsLeasePeriodStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllLeasePmtFreqTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllLeasePmtFreqTypes();

            // Assert
            Assert.IsType<PimsLeasePmtFreqType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllInsuranceTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllInsuranceTypes();

            // Assert
            Assert.IsType<PimsInsuranceType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllContactMethodTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllContactMethodTypes();

            // Assert
            Assert.IsType<PimsContactMethodType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllPropertyImprovementTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllPropertyImprovementTypes();

            // Assert
            Assert.IsType<PimsPropertyImprovementType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllSecurityDepositTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllSecurityDepositTypes();

            // Assert
            Assert.IsType<PimsSecurityDepositType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllLeasePaymentStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllLeasePaymentStatusTypes();

            // Assert
            Assert.IsType<PimsLeasePaymentStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllLeasePaymentMethodTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllLeasePaymentMethodTypes();

            // Assert
            Assert.IsType<PimsLeasePaymentMethodType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllResearchFileStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllResearchFileStatusTypes();

            // Assert
            Assert.IsType<PimsResearchFileStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllRequestSourceTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllRequestSourceTypes();

            // Assert
            Assert.IsType<PimsRequestSourceType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllResearchPurposeTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllResearchPurposeTypes();

            // Assert
            Assert.IsType<PimsResearchPurposeType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllPropertyResearchPurposeTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllPropertyResearchPurposeTypes();

            // Assert
            Assert.IsType<PimsPropResearchPurposeType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllPropertyAnomalyTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllPropertyAnomalyTypes();

            // Assert
            Assert.IsType<PimsPropertyAnomalyType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllPropertyRoadTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllPropertyRoadTypes();

            // Assert
            Assert.IsType<PimsPropertyRoadType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllPropertyVolumetricTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllPropertyVolumetricTypes();

            // Assert
            Assert.IsType<PimsVolumetricType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllPPHStatusType_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllPPHStatusType();

            // Assert
            Assert.IsType<PimsPphStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllDocumentStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllDocumentStatusTypes();

            // Assert
            Assert.IsType<PimsDocumentStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllDocumentTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllDocumentTypes();

            // Assert
            Assert.IsType<PimsDocumentTyp[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllAcquisitionFileStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllAcquisitionFileStatusTypes();

            // Assert
            Assert.IsType<PimsAcquisitionFileStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllAcquisitionPhysFileStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllAcquisitionPhysFileStatusTypes();

            // Assert
            Assert.IsType<PimsAcqPhysFileStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllAcquisitionTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllAcquisitionTypes();

            // Assert
            Assert.IsType<PimsAcquisitionType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllAcqFileTeamProfileTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllAcqFileTeamProfileTypes();

            // Assert
            Assert.IsType<PimsAcqFlTeamProfileType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllLeaseStakeholderTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllLeaseStakeholderTypes();

            // Assert
            Assert.IsType<PimsLeaseStakeholderType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllAcquisitionFundingTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllAcquisitionFundingTypes();

            // Assert
            Assert.IsType<PimsAcquisitionFundingType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllProjectStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllProjectStatusTypes();

            // Assert
            Assert.IsType<PimsProjectStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllTakeTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllTakeTypes();

            // Assert
            Assert.IsType<PimsTakeType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllTakeStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllTakeStatusTypes();

            // Assert
            Assert.IsType<PimsTakeStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllTakeSiteContamTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllTakeSiteContamTypes();

            // Assert
            Assert.IsType<PimsTakeSiteContamType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllAcquisitionChecklistSectionTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllAcquisitionChecklistSectionTypes();

            // Assert
            Assert.IsType<PimsAcqChklstSectionType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllAcquisitionChecklistItemStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllChecklistItemStatusTypes();

            // Assert
            Assert.IsType<PimsChklstItemStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllAgreementTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllAgreementTypes();

            // Assert
            Assert.IsType<PimsAgreementType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllFormTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllFormTypes();

            // Assert
            Assert.IsType<PimsFormType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllInterestHolderInterestTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllInterestHolderInterestTypes();

            // Assert
            Assert.IsType<PimsInterestHolderInterestType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllExpropriationPaymentItemTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllExpropriationPaymentItemTypes();

            // Assert
            Assert.IsType<PimsPaymentItemType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllMgmtActivityStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllMgmtActivityStatusTypes();

            // Assert
            Assert.IsType<PimsMgmtActivityStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllMgmtActivitySubtypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllMgmtActivitySubtypes();

            // Assert
            Assert.IsType<PimsMgmtActivitySubtype[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllMgmtActivityTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllMgmtActivityTypes();

            // Assert
            Assert.IsType<PimsMgmtActivityType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllAgreementStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllAgreementStatusTypes();

            // Assert
            Assert.IsType<PimsAgreementStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllDispositionFileStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllDispositionFileStatusTypes();

            // Assert
            Assert.IsType<PimsDispositionFileStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllDispositionFileFundingTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllDispositionFileFundingTypes();

            // Assert
            Assert.IsType<PimsDispositionFundingType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllDispositionInitiatingDocTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllDispositionInitiatingDocTypes();

            // Assert
            Assert.IsType<PimsDispositionInitiatingDocType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllDispositionTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllDispositionTypes();

            // Assert
            Assert.IsType<PimsDispositionType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllDispositionPhysFileStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllDispositionPhysFileStatusTypes();

            // Assert
            Assert.IsType<PimsDspPhysFileStatusType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllDispositionInitiatingBranchTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllDispositionInitiatingBranchTypes();

            // Assert
            Assert.IsType<PimsDspInitiatingBranchType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllDispositionFlTeamProfileTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllDispositionFlTeamProfileTypes();

            // Assert
            Assert.IsType<PimsDspFlTeamProfileType[]>(result);
        }

        [Fact]
        public void LookupRepository_GetAllDispositionStatusTypes_ReturnsCorrectType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var context = helper.CreatePimsContext(user, true);

            var lookupRepository = helper.CreateRepository<LookupRepository>(user);

            // Act
            var result = lookupRepository.GetAllDispositionStatusTypes();

            // Assert
            Assert.IsType<PimsDispositionStatusType[]>(result);
        }
    }
}
