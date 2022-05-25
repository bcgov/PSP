// TODO: This whole file needs to be rethought out and refactored.
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
  BBOX?: string;
}

// Lookup Codes
export const ADMINISTRATIVE_AREA_TYPES = 'PimsAdministrativeArea';
export const AREA_UNIT_TYPES = 'PimsAreaUnitType';
export const VOLUME_UNIT_TYPES = 'PimsVolumeUnitType';
export const CONTACT_METHOD_TYPES = 'PimsContactMethodType';
export const COUNTRY_TYPES = 'PimsCountry';
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
export const ROLE_TYPES = 'PimsRole';
export const SECURITY_DEPOSIT_TYPES = 'PimsSecurityDepositType';
export const RESEARCH_FILE_STATUS_TYPES = 'PimsResearchFileStatusType';
export const REQUEST_SOURCE_TYPES = 'PimsRequestSourceType';
export const RESEARCH_PURPOSE_TYPES = 'PimsResearchPurposeType';
export const PROPERTY_RESEARCH_PURPOSE_TYPES = 'PimsPropResearchPurposeType';
export const PROPERTY_LAND_PARCEL_TYPES = 'PimsPropertyType';

// TODO: This should all be removed from this and moved to the useApi* hooks.
// Organizations
export const POST_ORGANIZATIONS = () => `/admin/organizations/filter`; // get paged list of organizations

// TODO: This should all be removed from this and moved to the useApi* hooks.
// Auth Service
export const ACTIVATE_USER = () => `/auth/activate`; // get filtered properties or all if not specified.

// TODO: This should all be removed from this and moved to the useApi* hooks.
// User Service
export const POST_USERS = () => `/admin/users/my/organization`; // get paged list of users
