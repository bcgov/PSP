import { NumberFieldValue } from 'typings/NumberFieldValue';
import { stringToNull } from 'utils/formUtils';

import { formLeasePaymentToApiPayment, IFormLeasePayment, ILeasePayment } from './ILeasePayment';
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
  gstAmount?: number;
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
      gstAmount: NumberFieldValue;
      paymentDueDate: string;
      paymentNote: string;
      isGstEligible: boolean;
      isTermExercised: boolean;
      payments: IFormLeasePayment[];
    }
  > {}

export const defaultFormLeaseTerm: IFormLeaseTerm = {
  leaseId: 0,
  startDate: '',
  expiryDate: '',
  renewalDate: '',
  endDateHist: '',
  paymentAmount: '',
  gstAmount: '',
  paymentDueDate: '',
  paymentNote: '',
  isGstEligible: false,
  isTermExercised: false,
  effectiveDateHist: '',
  statusTypeCode: defaultTypeCode,
  leasePmtFreqTypeCode: defaultTypeCode,
  payments: [],
};

export const formLeaseTermToApiLeaseTerm = (
  formLeaseTerm: IFormLeaseTerm,
  gstConstant?: number,
): ILeaseTerm => {
  return {
    ...formLeaseTerm,
    renewalDate: undefined,
    expiryDate: stringToNull(formLeaseTerm.expiryDate),
    paymentAmount: stringToNull(formLeaseTerm.paymentAmount),
    gstAmount: stringToNull(
      formLeaseTerm.isGstEligible && gstConstant !== undefined
        ? (formLeaseTerm.paymentAmount as number) * (gstConstant / 100)
        : undefined,
    ),
    leasePmtFreqTypeCode: formLeaseTerm.leasePmtFreqTypeCode?.id
      ? formLeaseTerm.leasePmtFreqTypeCode
      : undefined,
    statusTypeCode: formLeaseTerm.statusTypeCode?.id ? formLeaseTerm.statusTypeCode : undefined,
    payments: formLeaseTerm.payments.map(payment => formLeasePaymentToApiPayment(payment)),
  };
};
