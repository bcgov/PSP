// TODO: PSP-4395 This whole file needs to be rethought out and refactored.
// Network URL's
import { AccessRequestStatus } from './accessStatus';

// Generic Params
export interface IPaginateParams {
  page: number;
  quantity?: number;
  sort?: string | string[];
}

export interface IGetUsersParams extends IPaginateParams {
  businessIdentifierValue?: string;
  firstName?: string;
  surname?: string;
  email?: string;
  organization?: string;
  role?: string;
  isDisabled?: boolean;
  position?: string;
}

export interface IGetOrganizationsParams extends IPaginateParams {
  name?: string;
  description?: string;
  isDisabled?: boolean;
}

export interface IPaginateAccessRequests extends IPaginateParams {
  status?: AccessRequestStatus | null;
}

// Parcels
export interface IPaginateProperties extends IPaginateParams {
  pid?: string;
  pin?: string;
  location?: string;
  address?: string;
}

export interface IGeoSearchParams {
  STREET_ADDRESS_1?: string;
  PID?: string;
  PIN?: string;
  SURVEY_PLAN_NUMBER?: string;
  BBOX?: string;
  latitude?: number | string;
  longitude?: number | string;
  forceExactMatch?: boolean;
}

// Lookup Codes
export const ADMINISTRATIVE_AREA_TYPES = 'PimsAdministrativeArea';
export const AREA_UNIT_TYPES = 'PimsAreaUnitType';
export const VOLUME_UNIT_TYPES = 'PimsVolumeUnitType';
export const CONTACT_METHOD_TYPES = 'PimsContactMethodType';
export const COUNTRY_TYPES = 'PimsCountry';
export const CONSULTATION_TYPES = 'PimsConsultationType';
export const CONSULTATION_STATUS_TYPES = 'PimsConsultationStatusType';
export const INSURANCE_TYPES = 'PimsInsuranceType';
export const LEASE_CATEGORY_TYPES = 'PimsLeaseCategoryType';
export const LEASE_INITIATOR_TYPES = 'PimsLeaseInitiatorType';
export const LEASE_PAYMENT_FREQUENCY_TYPES = 'PimsLeasePmtFreqType';
export const LEASE_PAYMENT_RECEIVABLE_TYPES = 'PimsLeasePayRvblType';
export const LEASE_PROGRAM_TYPES = 'PimsLeaseProgramType';
export const LEASE_PURPOSE_TYPES = 'PimsLeasePurposeType';
export const LEASE_RESPONSIBILITY_TYPES = 'PimsLeaseResponsibilityType';
export const LEASE_STATUS_TYPES = 'PimsLeaseStatusType';
export const LEASE_TERM_STATUS_TYPES = 'PimsLeaseTermStatusType';
export const LEASE_TYPES = 'PimsLeaseLicenseType';
export const ORGANIZATION_TYPES = 'PimsOrganization';
export const LEASE_PAYMENT_METHOD_TYPES = 'PimsLeasePaymentMethodType';
export const LEASE_PAYMENT_STATUS_TYPES = 'PimsLeasePaymentStatusType';
export const PROPERTY_CLASSIFICATION_TYPES = 'PimsPropertyClassification';
export const PROPERTY_IMPROVEMENT_TYPES = 'PimsPropertyImprovementType';
export const PROPERTY_ANOMALY_TYPES = 'PimsPropertyAnomalyType';
export const PROPERTY_TENURE_TYPES = 'PimsPropertyTenureType';
export const PROPERTY_ROAD_TYPES = 'PimsPropertyRoadType';
export const PROPERTY_ADJACENT_LAND_TYPES = 'PimsPropertyAdjacentLandType';
export const PROPERTY_VOLUMETRIC_TYPES = 'PimsVolumetricType';
export const PROVINCE_TYPES = 'PimsProvinceState';
export const REGION_TYPES = 'PimsRegion';
export const DISTRICT_TYPES = 'PimsDistrict';
export const ROLE_TYPES = 'PimsRole';
export const SECURITY_DEPOSIT_TYPES = 'PimsSecurityDepositType';
export const RESEARCH_FILE_STATUS_TYPES = 'PimsResearchFileStatusType';
export const REQUEST_SOURCE_TYPES = 'PimsRequestSourceType';
export const RESEARCH_PURPOSE_TYPES = 'PimsResearchPurposeType';
export const PROPERTY_RESEARCH_PURPOSE_TYPES = 'PimsPropResearchPurposeType';
export const PROPERTY_LAND_PARCEL_TYPES = 'PimsPropertyType';
export const PPH_STATUS_TYPES = 'PimsPphStatusType';
export const DOCUMENT_STATUS_TYPES = 'PimsDocumentStatusType';
export const ACQUISITION_FILE_STATUS_TYPES = 'PimsAcquisitionFileStatusType';
export const ACQUISITION_PHYSICAL_FILE_STATUS_TYPES = 'PimsAcqPhysFileStatusType';
export const ACQUISITION_TYPES = 'PimsAcquisitionType';
export const ACTIVITY_TEMPLATE_TYPE = 'PimsActivityTemplateType';
export const ACTIVITY_STATUS_TYPE = 'PimsActivityInstanceStatusType';
export const ACQUISITION_FILE_PERSON_PROFILE_TYPES = 'PimsAcqFlPersonProfileType';
export const TENANT_TYPES = 'PimsTenantType';
export const ACQUISITION_FUNDING_TYPES = 'PimsAcquisitionFundingType';
export const PROJECT_STATUS_TYPES = 'PimsProjectStatusType';
export const FORM_TYPES = 'PimsFormType';
export const TAKE_TYPES = 'PimsTakeType';
export const TAKE_STATUS_TYPES = 'PimsTakeStatusType';
export const TAKE_SITE_CONTAM_TYPES = 'PimsTakeSiteContamType';
export const TAKE_LAND_ACT_TYPES = 'PimsLandActType';
export const ACQUISITION_CHECKLIST_SECTION_TYPES = 'PimsAcqChklstSectionType';
export const ACQUISITION_CHECKLIST_ITEM_TYPES = 'PimsAcqChklstItemType';
export const ACQUISITION_CHECKLIST_ITEM_STATUS_TYPES = 'PimsAcqChklstItemStatusType';
export const AGREEMENT_TYPES = 'PimsAgreementType';
export const INTEREST_HOLDER_TYPES = 'PimsInterestHolderInterestType';
export const PAYMENT_ITEM_TYPES = 'PimsPaymentItemType';

// TODO: PSP-4395 This should all be removed from this and moved to the useApi* hooks.
// Auth Service
export const ACTIVATE_USER = () => `/auth/activate`; // get filtered properties or all if not specified.

export const MAX_SQL_INT_SIZE = 2147483647;
export const MAX_SQL_MONEY_SIZE = 922337203685477.58;
