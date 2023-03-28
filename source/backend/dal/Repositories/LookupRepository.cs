using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// LookRepository interface, provides a way to fetch lookup lists from the datasource.
    /// </summary>
    public class LookupRepository : BaseRepository, ILookupRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a LookRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public LookupRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<LookupRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get all organizations sorted by Name.
        /// </summary>
        public IEnumerable<PimsOrganization> GetAllOrganizations()
        {
            return this.Context.PimsOrganizations.AsNoTracking().OrderBy(a => a.OrganizationName).ToArray();
        }

        /// <summary>
        /// Get all organization types sorted by DisplayOrder and Id.
        /// </summary>
        public IEnumerable<PimsOrganizationType> GetAllOrganizationTypes()
        {
            return this.Context.PimsOrganizationTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.OrganizationTypeCode).ToArray();
        }

        /// <summary>
        /// Get all countries sorted by DisplayOrder and Code.
        /// </summary>
        public IEnumerable<PimsCountry> GetAllCountries()
        {
            return this.Context.PimsCountries.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.CountryCode).ToArray();
        }

        /// <summary>
        /// Get all provinces sorted by DisplayOrder and Code.
        /// </summary>
        public IEnumerable<PimsProvinceState> GetAllProvinces()
        {
            return this.Context.PimsProvinceStates.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.ProvinceStateCode).ToArray();
        }

        /// <summary>
        /// Get all regions sorted by DisplayOrder and Name.
        /// </summary>
        public IEnumerable<PimsRegion> GetAllRegions()
        {
            return this.Context.PimsRegions.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.RegionCode).ToArray();
        }

        /// <summary>
        /// Get all districts sorted by DisplayOrder and Name.
        /// </summary>
        public IEnumerable<PimsDistrict> GetAllDistricts()
        {
            return this.Context.PimsDistricts.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.DistrictCode).ToArray();
        }

        /// <summary>
        /// Get all property classification types sorted by DisplayOrder and Id.
        /// </summary>
        public IEnumerable<PimsPropertyClassificationType> GetAllPropertyClassificationTypes()
        {
            return this.Context.PimsPropertyClassificationTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.PropertyClassificationTypeCode).ToArray();
        }

        /// <summary>
        /// Get all property types sorted by DisplayOrder and Id.
        /// </summary>
        public IEnumerable<PimsPropertyType> GetAllPropertyTypes()
        {
            return this.Context.PimsPropertyTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.PropertyTypeCode).ToArray();
        }

        /// <summary>
        /// Get all property tenure types sorted by DisplayOrder and Id.
        /// </summary>
        public IEnumerable<PimsPropertyTenureType> GetAllPropertyTenureTypes()
        {
            return this.Context.PimsPropertyTenureTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).OrderBy(a => a.PropertyTenureTypeCode).ToArray();
        }

        /// <summary>
        /// Get all property area unit types sorted by DisplayOrder and Id.
        /// </summary>
        public IEnumerable<PimsAreaUnitType> GetAllPropertyAreaUnitTypes()
        {
            return this.Context.PimsAreaUnitTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).OrderBy(a => a.AreaUnitTypeCode).ToArray();
        }

        /// <summary>
        /// Get all property volume unit types sorted by DisplayOrder and Id.
        /// </summary>
        public IEnumerable<PimsVolumeUnitType> GetAllPropertyVolumeUnitTypes()
        {
            return this.Context.PimsVolumeUnitTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.VolumeUnitTypeCode).ToArray();
        }

        /// <summary>
        /// Get all roles sorted by Name.
        /// </summary>
        public IEnumerable<PimsRole> GetAllRoles()
        {
            return this.Context.PimsRoles.AsNoTracking().OrderBy(a => a.Name).ToArray();
        }

        public IEnumerable<PimsLeasePayRvblType> GetAllPaymentReceivableTypes()
        {
            return this.Context.PimsLeasePayRvblTypes.AsNoTracking().OrderBy(a => a.LeasePayRvblTypeCode).ToArray();
        }

        public IEnumerable<PimsLeaseProgramType> GetAllLeaseProgramTypes()
        {
            return this.Context.PimsLeaseProgramTypes.AsNoTracking().OrderBy(a => a.LeaseProgramTypeCode).ToArray();
        }

        public IEnumerable<PimsConsultationType> GetAllConsultationTypes()
        {
            return this.Context.PimsConsultationTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsConsultationStatusType> GetAllConsultationStatusTypes()
        {
            return this.Context.PimsConsultationStatusTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsLeaseStatusType> GetAllLeaseStatusTypes()
        {
            return this.Context.PimsLeaseStatusTypes.AsNoTracking().OrderBy(a => a.LeaseStatusTypeCode).ToArray();
        }

        public IEnumerable<PimsLeaseLicenseType> GetAllLeaseTypes()
        {
            return this.Context.PimsLeaseLicenseTypes.AsNoTracking().OrderBy(a => a.LeaseLicenseTypeCode).ToArray();
        }

        public IEnumerable<PimsLeaseCategoryType> GetAllLeaseCategoryTypes()
        {
            return this.Context.PimsLeaseCategoryTypes.AsNoTracking().OrderBy(a => a.LeaseCategoryTypeCode).ToArray();
        }

        public IEnumerable<PimsLeasePurposeType> GetAllLeasePurposeTypes()
        {
            return this.Context.PimsLeasePurposeTypes.AsNoTracking().OrderBy(a => a.LeasePurposeTypeCode).ToArray();
        }

        public IEnumerable<PimsLeaseResponsibilityType> GetAllLeaseResponsibilityTypes()
        {
            return this.Context.PimsLeaseResponsibilityTypes.AsNoTracking().OrderBy(a => a.LeaseResponsibilityTypeCode).ToArray();
        }

        public IEnumerable<PimsLeaseInitiatorType> GetAllLeaseInitiatorTypes()
        {
            return this.Context.PimsLeaseInitiatorTypes.AsNoTracking().OrderBy(a => a.LeaseInitiatorTypeCode).ToArray();
        }

        public IEnumerable<PimsLeaseTermStatusType> GetAllLeaseTermStatusTypes()
        {
            return this.Context.PimsLeaseTermStatusTypes.AsNoTracking().OrderBy(a => a.LeaseTermStatusTypeCode).ToArray();
        }

        public IEnumerable<PimsLeasePmtFreqType> GetAllLeasePmtFreqTypes()
        {
            return this.Context.PimsLeasePmtFreqTypes.AsNoTracking().OrderBy(a => a.LeasePmtFreqTypeCode).ToArray();
        }

        public IEnumerable<PimsInsuranceType> GetAllInsuranceTypes()
        {
            return this.Context.PimsInsuranceTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsContactMethodType> GetAllContactMethodTypes()
        {
            return this.Context.PimsContactMethodTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.ContactMethodTypeCode).ToArray();
        }

        public IEnumerable<PimsPropertyImprovementType> GetAllPropertyImprovementTypes()
        {
            return this.Context.PimsPropertyImprovementTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsSecurityDepositType> GetAllSecurityDepositTypes()
        {
            return this.Context.PimsSecurityDepositTypes.AsNoTracking().OrderBy(d => d.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsLeasePaymentStatusType> GetAllLeasePaymentStatusTypes()
        {
            return this.Context.PimsLeasePaymentStatusTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsLeasePaymentMethodType> GetAllLeasePaymentMethodTypes()
        {
            return this.Context.PimsLeasePaymentMethodTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsResearchFileStatusType> GetAllResearchFileStatusTypes()
        {
            return this.Context.PimsResearchFileStatusTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsRequestSourceType> GetAllRequestSourceTypes()
        {
            return this.Context.PimsRequestSourceTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsResearchPurposeType> GetAllResearchPurposeTypes()
        {
            return this.Context.PimsResearchPurposeTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsPropResearchPurposeType> GetAllPropertyResearchPurposeTypes()
        {
            return this.Context.PimsPropResearchPurposeTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsPropertyAnomalyType> GetAllPropertyAnomalyTypes()
        {
            return this.Context.PimsPropertyAnomalyTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsPropertyRoadType> GetAllPropertyRoadTypes()
        {
            return this.Context.PimsPropertyRoadTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsPropertyAdjacentLandType> GetAllPropertyAdjacentLandTypes()
        {
            return this.Context.PimsPropertyAdjacentLandTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsVolumetricType> GetAllPropertyVolumetricTypes()
        {
            return this.Context.PimsVolumetricTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsPphStatusType> GetAllPPHStatusType()
        {
            return this.Context.PimsPphStatusTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsDocumentStatusType> GetAllDocumentStatusTypes()
        {
            return this.Context.PimsDocumentStatusTypes.AsNoTracking().OrderBy(d => d.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsDocumentTyp> GetAllDocumentTypes()
        {
            return this.Context.PimsDocumentTyps.AsNoTracking().ToArray();
        }

        public IEnumerable<PimsAcquisitionFileStatusType> GetAllAcquisitionFileStatusTypes()
        {
            return this.Context.PimsAcquisitionFileStatusTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsAcqPhysFileStatusType> GetAllAcquisitionPhysFileStatusTypes()
        {
            return this.Context.PimsAcqPhysFileStatusTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsAcquisitionType> GetAllAcquisitionTypes()
        {
            return this.Context.PimsAcquisitionTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsActivityTemplateType> GetAllActivityTemplateTypes()
        {
            return this.Context.PimsActivityTemplateTypes.AsNoTracking().ToArray();
        }

        public IEnumerable<PimsActivityInstanceStatusType> GetAllActivityStatusTypes()
        {
            return this.Context.PimsActivityInstanceStatusTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsAcqFlPersonProfileType> GetAllAcqFilePersonProfileTypes()
        {
            return this.Context.PimsAcqFlPersonProfileTypes.AsNoTracking().ToArray();
        }

        public IEnumerable<PimsTenantType> GetAllTenantTypes()
        {
            return this.Context.PimsTenantTypes.AsNoTracking().ToArray();
        }

        public IEnumerable<PimsAcquisitionFundingType> GetAllAcquisitionFundingTypes()
        {
            return this.Context.PimsAcquisitionFundingTypes.AsNoTracking().ToArray();
        }

        public IEnumerable<PimsProjectStatusType> GetAllProjectStatusTypes()
        {
            return this.Context.PimsProjectStatusTypes.AsNoTracking().ToArray();
        }

        public IEnumerable<PimsTakeType> GetAllTakeTypes()
        {
            return this.Context.PimsTakeTypes.AsNoTracking().ToArray();
        }

        public IEnumerable<PimsTakeStatusType> GetAllTakeStatusTypes()
        {
            return this.Context.PimsTakeStatusTypes.AsNoTracking().ToArray();
        }

        public IEnumerable<PimsTakeSiteContamType> GetAllTakeSiteContamTypes()
        {
            return this.Context.PimsTakeSiteContamTypes.AsNoTracking().ToArray();
        }

        // TODO: Needs DB changes to use correct type here
        public IEnumerable<PimsAcqChklstSectionType> GetAllAcquisitionChecklistSectionTypes()
        {
            return Context.PimsAcqChklstSectionTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsAcqChklstItemStatusType> GetAllAcquisitionChecklistItemStatusTypes()
        {
            return Context.PimsAcqChklstItemStatusTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsFormType> GetAllFormTypes()
        {
            return this.Context.PimsFormTypes.AsNoTracking().ToArray();
        }
        #endregion
    }
}
