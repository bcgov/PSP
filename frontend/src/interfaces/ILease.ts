import { NumberFieldValue } from 'typings/NumberFieldValue';

import { IOrganization, IPerson, IProperty } from '.';
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
  programName: 'program',
  startDate: '2020-01-01',
  paymentFrequencyTypeId: 'ANNUAL',
  paymentFrequencyType: 'Annually',
  renewalCount: 0,
  motiName: 'Moti, Name, Name',
  tenantNotes: [],
};

export const defaultFormLease: IFormLease = {
  organizations: [],
  persons: [],
  properties: [],
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
};
