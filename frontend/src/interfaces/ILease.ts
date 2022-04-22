import { Api_Person } from 'models/api/Person';
import { Api_SecurityDeposit, Api_SecurityDepositReturn } from 'models/api/SecurityDeposit';
import { NumberFieldValue } from 'typings/NumberFieldValue';

import { IFormProperty, IInsurance, ILeaseImprovement, IOrganization, IProperty, ITenant } from '.';
import { IFormLeaseTerm, ILeaseTerm } from './ILeaseTerm';
import { IRegion } from './IRegion';
import ITypeCode from './ITypeCode';

export interface ILease {
  id?: number;
  lFileNo?: string;
  psFileNo?: string;
  tfaFileNo?: number;
  expiryDate?: string;
  renewalDate?: string;
  startDate: string;
  responsibilityEffectiveDate?: string;
  paymentReceivableType: ITypeCode<string>;
  categoryType: ITypeCode<string>;
  purposeType: ITypeCode<string>;
  responsibilityType: ITypeCode<string>;
  initiatorType: ITypeCode<string>;
  type?: ITypeCode<string>;
  statusType: ITypeCode<string>;
  region: IRegion;
  programType: ITypeCode<string>;
  otherType?: string;
  otherProgramType?: string;
  otherCategoryType?: string;
  otherPurposeType?: string;
  note?: string;
  programName?: string;
  motiName?: string;
  amount?: number;
  renewalCount: number;
  description?: string;
  isResidential: boolean;
  isCommercialBuilding: boolean;
  isOtherImprovement: boolean;
  returnNotes?: string; // security deposit notes (free form text)
  documentationReference?: string;
  tenantNotes: string[];
  insurances: IInsurance[];
  tenants: ITenant[];
  terms: ILeaseTerm[];
  properties: IProperty[];
  persons: Api_Person[];
  organizations: IOrganization[];
  improvements: ILeaseImprovement[];
  securityDeposits: Api_SecurityDeposit[];
  securityDepositReturns: Api_SecurityDepositReturn[];
  rowVersion?: number;
}

export interface IFormLease
  extends ExtendOverride<
    ILease,
    {
      tfaFileNo: NumberFieldValue;
      amount: NumberFieldValue;
      renewalCount: NumberFieldValue;
      paymentReceivableType?: ITypeCode<string>;
      categoryType?: ITypeCode<string>;
      purposeType?: ITypeCode<string>;
      responsibilityType?: ITypeCode<string>;
      initiatorType?: ITypeCode<string>;
      statusType?: ITypeCode<string>;
      region?: IRegion;
      programType?: ITypeCode<string>;
      terms: IFormLeaseTerm[];
    }
  > {}

export interface IAddFormLease
  extends ExtendOverride<
    ILease,
    {
      amount: NumberFieldValue;
      renewalCount: NumberFieldValue;
      tfaFileNo: NumberFieldValue;
      securityDeposits?: Api_SecurityDeposit[];
      securityDepositReturn?: Api_SecurityDepositReturn[];
      paymentReceivableType?: string;
      categoryType?: string;
      purposeType?: string;
      responsibilityType?: string;
      initiatorType?: string;
      type?: string;
      statusType?: string;
      region: NumberFieldValue;
      programType?: string;
      properties: IFormProperty[];
    }
  > {}

export const defaultLease: ILease = {
  organizations: [],
  persons: [],
  properties: [],
  improvements: [],
  statusType: { id: 'ACTIVE', description: 'Active', isDisabled: false },
  region: { regionCode: 1, regionName: 'South Coast Region' },
  programType: { id: 'OTHER', description: 'Other', isDisabled: false },
  startDate: '2020-01-01',
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
  tenants: [],
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
  tenantNotes: [],
  insurances: [],
  isResidential: false,
  isCommercialBuilding: false,
  isOtherImprovement: false,
  returnNotes: '',
  terms: [],
  tenants: [],
};

export const defaultAddFormLease: IAddFormLease = {
  lFileNo: '',
  psFileNo: '',
  tfaFileNo: '',
  expiryDate: '',
  renewalDate: '',
  startDate: '',
  responsibilityEffectiveDate: '',
  paymentReceivableType: '',
  categoryType: '',
  purposeType: '',
  responsibilityType: '',
  initiatorType: '',
  type: '',
  statusType: 'DRAFT',
  region: '',
  programType: '',
  otherType: '',
  otherProgramType: '',
  otherCategoryType: '',
  otherPurposeType: '',
  note: '',
  motiName: '',
  amount: '',
  renewalCount: '',
  description: '',
  isResidential: false,
  isCommercialBuilding: false,
  isOtherImprovement: false,
  returnNotes: '',
  documentationReference: '',
  tenantNotes: [],
  insurances: [],
  terms: [],
  tenants: [],
  properties: [{ pid: '', pin: '', areaUnitType: { id: '' } }],
  persons: [],
  organizations: [],
  improvements: [],
  securityDeposits: [],
  securityDepositReturns: [],
};
