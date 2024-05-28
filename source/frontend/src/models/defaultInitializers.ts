import moment from 'moment';

import { prettyFormatDate } from '@/utils';

import { ApiGen_Base_BaseAudit } from './api/generated/ApiGen_Base_BaseAudit';
import { ApiGen_Concepts_AcquisitionFile } from './api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_Lease } from './api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_Property } from './api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_ResearchFile } from './api/generated/ApiGen_Concepts_ResearchFile';
import { EpochIsoDateTime } from './api/UtcIsoDateTime';

// Note: Avoid using the initializers below as much as possible. Nullable members should be explicit when creating or filling an object.
export const getEmptyBaseAudit = (rowVersion?: number | null): ApiGen_Base_BaseAudit => ({
  appCreateTimestamp: EpochIsoDateTime,
  appLastUpdateTimestamp: EpochIsoDateTime,
  appLastUpdateUserid: null,
  appCreateUserid: null,
  appLastUpdateUserGuid: null,
  appCreateUserGuid: null,
  rowVersion: rowVersion ?? null,
});

/**
 * Avoid using. Nullable members should be explicit when creating or filling an object
 * @returns an empty api leade
 */
export const getEmptyLease = (): ApiGen_Concepts_Lease => ({
  id: 0,
  amount: null,
  motiName: null,
  programName: null,
  documentationReference: null,
  note: null,
  description: null,
  lFileNo: null,
  tfaFileNumber: null,
  psFileNo: null,
  otherCategoryType: null,
  otherProgramType: null,
  otherPurposeType: null,
  otherType: null,
  expiryDate: null,
  startDate: EpochIsoDateTime,
  renewalCount: 0,
  paymentReceivableType: null,
  type: null,
  initiatorType: null,
  responsibilityType: null,
  categoryType: null,
  purposeType: null,
  fileStatusTypeCode: null,
  region: null,
  programType: null,
  returnNotes: null,
  responsibilityEffectiveDate: null,
  fileProperties: null,
  consultations: null,
  tenants: null,
  terms: null,
  isResidential: false,
  isCommercialBuilding: false,
  isOtherImprovement: false,
  hasPhysicalFile: false,
  hasDigitalFile: false,
  hasPhysicalLicense: null,
  hasDigitalLicense: null,
  isExpired: false,
  project: null,
  fileName: null,
  fileNumber: null,
  ...getEmptyBaseAudit(),
});

/**
 * Avoid using. Nullable members should be explicit when creating or filling an object
 * @returns a defaut lease
 */
export const defaultApiLease = (): ApiGen_Concepts_Lease => ({
  ...getEmptyLease(),
  tfaFileNumber: null,
  fileProperties: [],
  fileStatusTypeCode: null,
  region: null,
  programType: null,
  startDate: prettyFormatDate(moment()),
  paymentReceivableType: null,
  categoryType: null,
  purposeType: null,
  responsibilityType: null,
  initiatorType: null,
  type: null,
  motiName: null,
  isResidential: false,
  isCommercialBuilding: false,
  isOtherImprovement: false,
  returnNotes: '',
  consultations: [],
  tenants: [],
  terms: [],
  otherCategoryType: null,
  otherProgramType: null,
  otherPurposeType: null,
  otherType: null,
});

/**
 * Avoid using. Nullable members should be explicit when creating or filling an object
 * @returns an empty api property
 */
export const getEmptyProperty = (): ApiGen_Concepts_Property => ({
  id: 0,
  propertyType: null,
  anomalies: null,
  tenures: null,
  roadTypes: null,
  status: null,
  dataSource: null,
  region: null,
  district: null,
  dataSourceEffectiveDateOnly: EpochIsoDateTime,
  latitude: null,
  longitude: null,
  name: null,
  description: null,
  isSensitive: false,
  isRetired: false,
  isProvincialPublicHwy: null,
  pphStatusUpdateUserid: null,
  pphStatusUpdateTimestamp: null,
  pphStatusUpdateUserGuid: null,
  isRwyBeltDomPatent: null,
  pphStatusTypeCode: null,
  address: null,
  pid: null,
  pin: null,
  planNumber: null,
  isOwned: false,
  isVisibleToOtherAgencies: false,
  areaUnit: null,
  landArea: null,
  isVolumetricParcel: null,
  volumetricMeasurement: null,
  volumetricUnit: null,
  volumetricType: null,
  landLegalDescription: null,
  municipalZoning: null,
  zoning: null,
  zoningPotential: null,
  location: null,
  boundary: null,
  generalLocation: null,
  propertyContacts: null,
  notes: null,
  surplusDeclarationType: null,
  surplusDeclarationComment: null,
  surplusDeclarationDate: EpochIsoDateTime,
  ...getEmptyBaseAudit(),
});

/**
 * Avoid using. Nullable members should be explicit when creating or filling an object
 * @returns an empty api research file
 */
export const getEmptyResearchFile = (): ApiGen_Concepts_ResearchFile => {
  return {
    ...getEmptyBaseAudit(),
    roadName: null,
    roadAlias: null,
    fileProperties: null,
    requestDate: null,
    requestDescription: null,
    requestSourceDescription: null,
    researchResult: null,
    researchCompletionDate: null,
    isExpropriation: null,
    expropriationNotes: null,
    requestSourceType: null,
    requestorPerson: null,
    requestorOrganization: null,
    researchFilePurposes: null,
    researchFileProjects: null,
    id: 0,
    fileName: null,
    fileNumber: null,
    fileStatusTypeCode: null,
  };
};

/**
 * Avoid using. Nullable members should be explicit when creating or filling an object
 * @returns an empty api acquisition file
 */
export const getEmptyAcquisitionFile = (): ApiGen_Concepts_AcquisitionFile => {
  return {
    ...getEmptyBaseAudit(),
    fileNo: 0,
    legacyFileNumber: null,
    assignedDate: null,
    deliveryDate: null,
    acquisitionPhysFileStatusTypeCode: null,
    acquisitionTypeCode: null,
    productId: null,
    totalAllowableCompensation: null,
    product: null,
    fundingTypeCode: null,
    fundingOther: null,
    projectId: null,
    project: null,
    regionCode: null,
    legacyStakeholders: null,
    fileProperties: null,
    acquisitionTeam: null,
    acquisitionFileOwners: null,
    acquisitionFileInterestHolders: null,
    compensationRequisitions: null,
    fileChecklistItems: null,
    id: 0,
    fileName: null,
    fileNumber: null,
    fileStatusTypeCode: null,
  };
};
