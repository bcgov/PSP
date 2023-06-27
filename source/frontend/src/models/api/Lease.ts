import moment from 'moment';

import { prettyFormatDate } from '@/utils';

import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Insurance } from './Insurance';
import { Api_LeaseTenant } from './LeaseTenant';
import { Api_LeaseTerm } from './LeaseTerm';
import { Api_Project } from './Project';
import { Api_PropertyLease } from './PropertyLease';
import { Api_SecurityDeposit } from './SecurityDeposit';
import Api_TypeCode from './TypeCode';

export interface Api_Lease extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number | null;
  lFileNo?: string | null;
  psFileNo?: string | null;
  tfaFileNumber?: string | null;
  expiryDate?: string | null;
  startDate?: string;
  responsibilityEffectiveDate?: string | null;
  paymentReceivableType: Api_TypeCode<string> | null;
  categoryType: Api_TypeCode<string> | null;
  purposeType: Api_TypeCode<string> | null;
  responsibilityType: Api_TypeCode<string> | null;
  initiatorType: Api_TypeCode<string> | null;
  type: Api_TypeCode<string> | null;
  statusType: Api_TypeCode<string> | null;
  region: Api_TypeCode<number> | null;
  programType: Api_TypeCode<string> | null;
  otherType: string | null;
  otherProgramType: string | null;
  otherCategoryType: string | null;
  otherPurposeType: string | null;
  note?: string | null;
  motiName?: string | null;
  amount?: number | null;
  description?: string | null;
  isResidential?: boolean | null;
  isCommercialBuilding?: boolean | null;
  isOtherImprovement?: boolean | null;
  returnNotes?: string | null; // security deposit notes (free form text)
  documentationReference?: string | null;
  hasPhysicalLicense?: boolean | null;
  hasDigitalLicense?: boolean | null;
  project?: Api_Project | null;
  properties?: Api_PropertyLease[];
  securityDeposits?: Api_SecurityDeposit[];
  consultations: Api_LeaseConsultation[];
  tenants: Api_LeaseTenant[];
  terms: Api_LeaseTerm[];
  insurances: Api_Insurance[];
}

export const defaultApiLease: Api_Lease = {
  tfaFileNumber: null,
  properties: [],
  statusType: null,
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
  securityDeposits: [],
  isResidential: false,
  isCommercialBuilding: false,
  isOtherImprovement: false,
  returnNotes: '',
  consultations: [],
  tenants: [],
  terms: [],
  insurances: [],
  otherCategoryType: null,
  otherProgramType: null,
  otherPurposeType: null,
  otherType: null,
};

export interface Api_LeaseConsultation extends Api_ConcurrentVersion {
  id: number | null;
  consultationType: Api_TypeCode<string> | null;
  consultationStatusType: Api_TypeCode<string> | null;
  parentLeaseId: number | null;
  otherDescription: string | null;
}
