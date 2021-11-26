import { NumberFieldValue } from 'typings/NumberFieldValue';

import {
  IInsurance,
  ILeaseImprovement,
  ILeaseSecurityDeposit,
  ILeaseSecurityDepositReturn,
  IOrganization,
  IPerson,
  IProperty,
} from '.';
import { ILeaseTerm } from './ILeaseTerm';
import ITypeCode from './ITypeCode';

export interface ILease {
  id?: number;
  lFileNo?: string;
  psFileNo?: string;
  tfaFileNo?: string;
  programName: string;
  expiryDate?: string;
  renewalDate?: string;
  startDate: string;
  properties: IProperty[];
  persons: IPerson[];
  organizations: IOrganization[];
  improvements: ILeaseImprovement[];
  paymentFrequencyType?: ITypeCode<string>;
  paymentReceivableType?: ITypeCode<string>;
  securityDeposits: ILeaseSecurityDeposit[];
  securityDepositReturns: ILeaseSecurityDepositReturn[];
  categoryType?: ITypeCode<string>;
  purposeType?: ITypeCode<string>;
  responsibilityType?: ITypeCode<string>;
  initiatorType?: ITypeCode<string>;
  type?: ITypeCode<string>;
  amount?: number;
  renewalCount: number;
  note?: string;
  motiName: string;
  description?: string;
  landArea?: number;
  areaUnit?: string;
  tenantNotes: string[];
  insurances: IInsurance[];
  isResidential: boolean;
  isCommercialBuilding: boolean;
  isOtherImprovement: boolean;
  returnNotes?: string; // security deposit notes (free form text)
  terms: ILeaseTerm[];
}

export interface IFormLease
  extends ExtendOverride<
    ILease,
    {
      amount: NumberFieldValue;
      renewalCount: NumberFieldValue;
      landArea: NumberFieldValue;
    }
  > {}

export const defaultLease: ILease = {
  organizations: [],
  persons: [],
  properties: [],
  improvements: [],
  programName: 'program',
  startDate: '2020-01-01',
  paymentFrequencyType: { id: 'ANNUAL', description: 'Annually', isDisabled: false },
  paymentReceivableType: { id: 'RCVBL', description: 'Receivable', isDisabled: false },
  categoryType: { id: 'COMM', description: 'Commercial', isDisabled: false },
  purposeType: { id: 'BCFERRIES', description: 'BC Ferries', isDisabled: false },
  responsibilityType: { id: 'HQ', description: 'Headquarters', isDisabled: false },
  initiatorType: { id: 'PROJECT', description: 'Project', isDisabled: false },
  type: { id: 'LSREG', description: 'Lease - Registered', isDisabled: false },
  renewalCount: 0,
  motiName: 'Moti, Name, Name',
  tenantNotes: [],
  insurances: [],
  securityDeposits: [],
  securityDepositReturns: [],
  isResidential: false,
  isCommercialBuilding: false,
  isOtherImprovement: false,
  returnNotes: '',
  terms: [],
};

export const defaultFormLease: IFormLease = {
  organizations: [],
  persons: [],
  properties: [],
  improvements: [],
  securityDeposits: [],
  securityDepositReturns: [],
  startDate: '',
  expiryDate: '',
  renewalDate: '',
  lFileNo: '',
  tfaFileNo: '',
  psFileNo: '',
  programName: '',
  motiName: '',
  amount: '',
  renewalCount: '',
  landArea: '',
  areaUnit: '',
  tenantNotes: [],
  insurances: [],
  isResidential: false,
  isCommercialBuilding: false,
  isOtherImprovement: false,
  returnNotes: '',
  terms: [],
};
