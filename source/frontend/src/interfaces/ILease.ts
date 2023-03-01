import { FormTenant } from 'features/leases/detail/LeasePages/tenant/ViewTenantForm';
import { Api_AuditFields } from 'models/api/AuditFields';
import { Api_ConcurrentVersion } from 'models/api/ConcurrentVersion';
import { Api_LeaseTenant } from 'models/api/LeaseTenant';
import { Api_Person } from 'models/api/Person';
import { Api_Project } from 'models/api/Project';
import { Api_SecurityDeposit, Api_SecurityDepositReturn } from 'models/api/SecurityDeposit';
import { NumberFieldValue } from 'typings/NumberFieldValue';

import { IFormProperty, IInsurance, ILeaseImprovement, IOrganization, IProperty } from '.';
import { IFormLeaseTerm, ILeaseTerm } from './ILeaseTerm';
import { IRegion } from './IRegion';
import ITypeCode from './ITypeCode';

export interface ILease extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  lFileNo?: string;
  psFileNo?: string;
  tfaFileNumber?: string;
  expiryDate?: string;
  renewalDate?: string;
  startDate: string;
  responsibilityEffectiveDate?: string;
  paymentReceivableType?: ITypeCode<string>;
  categoryType?: ITypeCode<string>;
  purposeType?: ITypeCode<string>;
  responsibilityType?: ITypeCode<string>;
  initiatorType?: ITypeCode<string>;
  type?: ITypeCode<string>;
  statusType?: ITypeCode<string>;
  region?: IRegion;
  programType?: ITypeCode<string>;
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
  hasPhysicalLicense: boolean | null;
  hasDigitalLicense: boolean | null;
  tenantNotes: string[];
  insurances: IInsurance[];
  tenants: Api_LeaseTenant[];
  terms: ILeaseTerm[];
  properties: IProperty[];
  persons: Api_Person[];
  organizations: IOrganization[];
  improvements: ILeaseImprovement[];
  securityDeposits: Api_SecurityDeposit[];
  securityDepositReturns: Api_SecurityDepositReturn[];
  project?: Api_Project;
}

export interface IFormLease
  extends ExtendOverride<
    ILease,
    {
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
      tenants: FormTenant[];
      hasPhysicalLicense?: string;
      hasDigitalLicense?: string;
    }
  > {}

export interface IAddFormLease
  extends ExtendOverride<
    ILease,
    {
      amount: NumberFieldValue;
      renewalCount: NumberFieldValue;
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
      hasPhysicalLicense: string;
      hasDigitalLicense: string;
    }
  > {}

export const defaultLease: ILease = {
  tfaFileNumber: undefined,
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
  hasDigitalLicense: false,
  hasPhysicalLicense: false,
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
  tfaFileNumber: '',
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
  hasDigitalLicense: 'Unknown',
  hasPhysicalLicense: 'Unknown',
};
