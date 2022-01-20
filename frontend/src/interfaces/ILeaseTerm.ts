import { NumberFieldValue } from 'typings/NumberFieldValue';
import { stringToNull } from 'utils/formUtils';

import { ILeasePayment } from './ILeasePayment';
import ITypeCode, { defaultTypeCode } from './ITypeCode';
export interface ILeaseTerm {
  id?: number;
  leaseId?: number;
  leaseRowVersion?: number;
  statusTypeCode?: ITypeCode<string>;
  expiryDate?: string;
  renewalDate?: string;
  endDateHist?: string;
  leasePmtFreqTypeCode?: ITypeCode<string>;
  paymentAmount?: number;
  paymentDueDate?: string;
  paymentNote?: string;
  isGstEligible?: boolean;
  isTermExercised?: boolean;
  startDate: string;
  effectiveDateHist: string;
  payments: ILeasePayment[];
}

export interface IFormLeaseTerm
  extends ExtendOverride<
    ILeaseTerm,
    {
      expiryDate: string;
      renewalDate: string;
      endDateHist: string;
      paymentAmount: NumberFieldValue;
      paymentDueDate: string;
      paymentNote: string;
      isGstEligible: boolean;
      isTermExercised: boolean;
    }
  > {}

export const defaultFormLeaseTerm: IFormLeaseTerm = {
  leaseId: 0,
  startDate: '',
  expiryDate: '',
  renewalDate: '',
  endDateHist: '',
  paymentAmount: '',
  paymentDueDate: '',
  paymentNote: '',
  isGstEligible: false,
  isTermExercised: false,
  effectiveDateHist: '',
  statusTypeCode: defaultTypeCode,
  leasePmtFreqTypeCode: defaultTypeCode,
  payments: [],
};

export const formLeaseTermToApiLeaseTerm = (formLeaseTerm: IFormLeaseTerm): ILeaseTerm => {
  return {
    ...formLeaseTerm,
    renewalDate: undefined,
    expiryDate: stringToNull(formLeaseTerm.expiryDate),
    paymentAmount: stringToNull(formLeaseTerm.paymentAmount),
    leasePmtFreqTypeCode: formLeaseTerm.leasePmtFreqTypeCode?.id
      ? formLeaseTerm.leasePmtFreqTypeCode
      : undefined,
  };
};
