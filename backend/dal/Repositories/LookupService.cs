using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// LookupService interface, provides a way to fetch lookup lists from the datasource.
    /// </summary>
    public class LookupService : BaseRepository, ILookupService
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a LookService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public LookupService(PimsContext dbContext, ClaimsPrincipal user, ILogger<LookupService> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get all organizations sorted by Name.
        /// </summary>
        public IEnumerable<PimsOrganization> GetOrganizations()
        {
            return this.Context.PimsOrganizations.AsNoTracking().OrderBy(a => a.OrganizationName).ToArray();
        }

        /// <summary>
        /// Get all organization types sorted by DisplayOrder and Id.
        /// </summary>
        public IEnumerable<PimsOrganizationType> GetOrganizationTypes()
        {
            return this.Context.PimsOrganizationTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.OrganizationTypeCode).ToArray();
        }

        /// <summary>
        /// Get all countries sorted by DisplayOrder and Code.
        /// </summary>
        public IEnumerable<PimsCountry> GetCountries()
        {
            return this.Context.PimsCountries.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.CountryCode).ToArray();
        }

        /// <summary>
        /// Get all provinces sorted by DisplayOrder and Code.
        /// </summary>
        public IEnumerable<PimsProvinceState> GetProvinces()
        {
            return this.Context.PimsProvinceStates.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.ProvinceStateCode).ToArray();
        }

        /// <summary>
        /// Get all regions sorted by DisplayOrder and Name.
        /// </summary>
        public IEnumerable<PimsRegion> GetRegions()
        {
            return this.Context.PimsRegions.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.RegionCode).ToArray();
        }

        /// <summary>
        /// Get all districts sorted by DisplayOrder and Name.
        /// </summary>
        public IEnumerable<PimsDistrict> GetDistricts()
        {
            return this.Context.PimsDistricts.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.DistrictCode).ToArray();
        }

        /// <summary>
        /// Get all property classification types sorted by DisplayOrder and Id.
        /// </summary>
        public IEnumerable<PimsPropertyClassificationType> GetPropertyClassificationTypes()
        {
            return this.Context.PimsPropertyClassificationTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.PropertyClassificationTypeCode).ToArray();
        }

        /// <summary>
        /// Get all property types sorted by DisplayOrder and Id.
        /// </summary>
        public IEnumerable<PimsPropertyType> GetPropertyTypes()
        {
            return this.Context.PimsPropertyTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.PropertyTypeCode).ToArray();
        }

        /// <summary>
        /// Get all property tenure types sorted by DisplayOrder and Id.
        /// </summary>
        public IEnumerable<PimsPropertyTenureType> GetPropertyTenureTypes()
        {
            return this.Context.PimsPropertyTenureTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).OrderBy(a => a.PropertyTenureTypeCode).ToArray();
        }

        /// <summary>
        /// Get all property area unit types sorted by DisplayOrder and Id.
        /// </summary>
        public IEnumerable<PimsAreaUnitType> GetPropertyAreaUnitTypes()
        {
            return this.Context.PimsAreaUnitTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).OrderBy(a => a.AreaUnitTypeCode).ToArray();
        }

        /// <summary>
        /// Get all property volume unit types sorted by DisplayOrder and Id.
        /// </summary>
        public IEnumerable<PimsVolumeUnitType> GetPropertyVolumeUnitTypes()
        {
            return this.Context.PimsVolumeUnitTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.VolumeUnitTypeCode).ToArray();
        }

        /// <summary>
        /// Get all roles sorted by Name.
        /// </summary>
        public IEnumerable<PimsRole> GetRoles()
        {
            return this.Context.PimsRoles.AsNoTracking().OrderBy(a => a.Name).ToArray();
        }

        public IEnumerable<PimsLeasePayRvblType> GetPaymentReceivableTypes()
        {
            return this.Context.PimsLeasePayRvblTypes.AsNoTracking().OrderBy(a => a.LeasePayRvblTypeCode).ToArray();
        }

        public IEnumerable<PimsLeaseProgramType> GetLeaseProgramTypes()
        {
            return this.Context.PimsLeaseProgramTypes.AsNoTracking().OrderBy(a => a.LeaseProgramTypeCode).ToArray();
        }

        public IEnumerable<PimsLeaseStatusType> GetLeaseStatusTypes()
        {
            return this.Context.PimsLeaseStatusTypes.AsNoTracking().OrderBy(a => a.LeaseStatusTypeCode).ToArray();
        }

        public IEnumerable<PimsLeaseLicenseType> GetLeaseTypes()
        {
            return this.Context.PimsLeaseLicenseTypes.AsNoTracking().OrderBy(a => a.LeaseLicenseTypeCode).ToArray();
        }

        public IEnumerable<PimsLeaseCategoryType> GetLeaseCategoryTypes()
        {
            return this.Context.PimsLeaseCategoryTypes.AsNoTracking().OrderBy(a => a.LeaseCategoryTypeCode).ToArray();
        }

        public IEnumerable<PimsLeasePurposeType> GetLeasePurposeTypes()
        {
            return this.Context.PimsLeasePurposeTypes.AsNoTracking().OrderBy(a => a.LeasePurposeTypeCode).ToArray();
        }

        public IEnumerable<PimsLeaseResponsibilityType> GetLeaseResponsibilityTypes()
        {
            return this.Context.PimsLeaseResponsibilityTypes.AsNoTracking().OrderBy(a => a.LeaseResponsibilityTypeCode).ToArray();
        }

        public IEnumerable<PimsLeaseInitiatorType> GetLeaseInitiatorTypes()
        {
            return this.Context.PimsLeaseInitiatorTypes.AsNoTracking().OrderBy(a => a.LeaseInitiatorTypeCode).ToArray();
        }

        public IEnumerable<PimsLeaseTermStatusType> GetLeaseTermStatusTypes()
        {
            return this.Context.PimsLeaseTermStatusTypes.AsNoTracking().OrderBy(a => a.LeaseTermStatusTypeCode).ToArray();
        }

        public IEnumerable<PimsLeasePmtFreqType> GetLeasePmtFreqTypes()
        {
            return this.Context.PimsLeasePmtFreqTypes.AsNoTracking().OrderBy(a => a.LeasePmtFreqTypeCode).ToArray();
        }

        public IEnumerable<PimsInsuranceType> GetInsuranceTypes()
        {
            return this.Context.PimsInsuranceTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsContactMethodType> GetContactMethodTypes()
        {
            return this.Context.PimsContactMethodTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.ContactMethodTypeCode).ToArray();
        }

        public IEnumerable<PimsPropertyImprovementType> GetPropertyImprovementTypes()
        {
            return this.Context.PimsPropertyImprovementTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsSecurityDepositType> GetSecurityDepositTypes()
        {
            return this.Context.PimsSecurityDepositTypes.AsNoTracking().OrderBy(d => d.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsLeasePaymentStatusType> GetLeasePaymentStatusTypes()
        {
            return this.Context.PimsLeasePaymentStatusTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsLeasePaymentMethodType> GetLeasePaymentMethodTypes()
        {
            return this.Context.PimsLeasePaymentMethodTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsResearchFileStatusType> GetResearchFileStatusTypes()
        {
            return this.Context.PimsResearchFileStatusTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsRequestSourceType> GeRequestSourceTypes()
        {
            return this.Context.PimsRequestSourceTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsResearchPurposeType> GetResearchPurposeTypes()
        {
            return this.Context.PimsResearchPurposeTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsPropResearchPurposeType> GetPropertyResearchPurposeTypes()
        {
            return this.Context.PimsPropResearchPurposeTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsPropertyAnomalyType> GetPropertyAnomalyTypes()
        {
            return this.Context.PimsPropertyAnomalyTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsPropertyRoadType> GetPropertyRoadTypes()
        {
            return this.Context.PimsPropertyRoadTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsPropertyAdjacentLandType> GetPropertyAdjacentLandTypes()
        {
            return this.Context.PimsPropertyAdjacentLandTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsVolumetricType> GetPropertyVolumetricTypes()
        {
            return this.Context.PimsVolumetricTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsPphStatusType> GetPPHStatusType()
        {
            return this.Context.PimsPphStatusTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsDocumentStatusType> GetDocumentStatusTypes()
        {
            return this.Context.PimsDocumentStatusTypes.AsNoTracking().OrderBy(d => d.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsDocumentTyp> GetDocumentTypes()
        {
            return this.Context.PimsDocumentTyps.AsNoTracking().ToArray();
        }

        public IEnumerable<PimsAcquisitionFileStatusType> GetAcquisitionFileStatusTypes()
        {
            return this.Context.PimsAcquisitionFileStatusTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsAcqPhysFileStatusType> GetAcquisitionPhysFileStatusTypes()
        {
            return this.Context.PimsAcqPhysFileStatusTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsAcquisitionType> GetAcquisitionTypes()
        {
            return this.Context.PimsAcquisitionTypes.AsNoTracking().OrderBy(r => r.DisplayOrder).ToArray();
        }

        public IEnumerable<PimsActivityTemplateType> GetActivityTemplateTypes()
        {
            return this.Context.PimsActivityTemplateTypes.AsNoTracking().ToArray();
        }

        public IEnumerable<PimsActivityInstanceStatusType> GetActivityStatusTypes()
        {
            return this.Context.PimsActivityInstanceStatusTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ToArray();
        }
        #endregion
    }
}
