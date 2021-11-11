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
  securityDeposits: ILeaseSecurityDeposit[];
  securityDepositReturns: ILeaseSecurityDepositReturn[];
  paymentReceivableTypeId?: string;
  paymentFrequencyTypeId: string;
  paymentFrequencyType: string;
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
  paymentFrequencyTypeId: 'ANNUAL',
  paymentFrequencyType: 'Annually',
  renewalCount: 0,
  motiName: 'Moti, Name, Name',
  tenantNotes: [],
  insurances: [],
  securityDeposits: [],
  securityDepositReturns: [],
  isResidential: false,
  isCommercialBuilding: false,
  isOtherImprovement: false,
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
  paymentReceivableTypeId: '',
  programName: '',
  motiName: '',
  amount: '',
  renewalCount: '',
  paymentFrequencyTypeId: '',
  paymentFrequencyType: '',
  landArea: '',
  areaUnit: '',
  tenantNotes: [],
  insurances: [],
  isResidential: false,
  isCommercialBuilding: false,
  isOtherImprovement: false,
};
