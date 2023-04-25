import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Project } from './Project';
import { Api_PropertyLease } from './PropertyLease';
import { Api_SecurityDeposit } from './SecurityDeposit';
import Api_TypeCode from './TypeCode';

export interface Api_Lease extends Api_ConcurrentVersion {
  id?: number;
  lFileNo?: string;
  psFileNo?: string;
  tfaFileNumber?: string;
  expiryDate?: string;
  startDate?: string;
  responsibilityEffectiveDate?: string;
  paymentReceivableType?: Api_TypeCode<string>;
  categoryType?: Api_TypeCode<string>;
  purposeType?: Api_TypeCode<string>;
  responsibilityType?: Api_TypeCode<string>;
  initiatorType?: Api_TypeCode<string>;
  type?: Api_TypeCode<string>;
  statusType?: Api_TypeCode<string>;
  region?: Api_TypeCode<number>;
  programType?: Api_TypeCode<string>;
  otherType?: string;
  otherProgramType?: string;
  otherCategoryType?: string;
  otherPurposeType?: string;
  note?: string;
  motiName?: string;
  amount?: number;
  description?: string;
  isResidential?: boolean;
  isCommercialBuilding?: boolean;
  isOtherImprovement?: boolean;
  returnNotes?: string; // security deposit notes (free form text)
  documentationReference?: string;
  hasPhysicalLicense?: boolean;
  hasDigitalLicense?: boolean;
  project?: Api_Project;
  properties?: Api_PropertyLease[];
  securityDeposits?: Api_SecurityDeposit[];
  consultations: Api_LeaseConsultation[] | null;
}

export const defaultApiLease: Api_Lease = {
  tfaFileNumber: undefined,
  properties: [],
  statusType: { id: 'ACTIVE', description: 'Active', isDisabled: false },
  region: { id: 1, description: 'South Coast Region' },
  programType: { id: 'OTHER', description: 'Other', isDisabled: false },
  startDate: '2020-01-01',
  paymentReceivableType: { id: 'RCVBL', description: 'Receivable', isDisabled: false },
  categoryType: { id: 'COMM', description: 'Commercial', isDisabled: false },
  purposeType: { id: 'BCFERRIES', description: 'BC Ferries', isDisabled: false },
  responsibilityType: { id: 'HQ', description: 'Headquarters', isDisabled: false },
  initiatorType: { id: 'PROJECT', description: 'Project', isDisabled: false },
  type: { id: 'LSREG', description: 'Lease - Registered', isDisabled: false },
  motiName: 'Moti, Name, Name',
  securityDeposits: [],
  isResidential: false,
  isCommercialBuilding: false,
  isOtherImprovement: false,
  returnNotes: '',
  consultations: null,
};

export interface Api_LeaseConsultation extends Api_ConcurrentVersion {
  id: number | null;
  consultationType: Api_TypeCode<string> | null;
  consultationStatusType: Api_TypeCode<string> | null;
  parentLeaseId: number | null;
  otherDescription: string | null;
}
