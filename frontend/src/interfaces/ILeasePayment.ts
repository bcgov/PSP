import { NumberFieldValue } from 'typings/NumberFieldValue';
import { stringToNull } from 'utils/formUtils';

import ITypeCode, { defaultTypeCode } from './ITypeCode';

export interface ILeasePayment {
  id?: number;
  leaseId?: number;
  leaseRowVersion?: number;
  leaseTermId: number;
  leasePaymentMethodType?: ITypeCode<string>;
  receivedDate: string;
  amountPreTax: number;
  amountPst?: number;
  amountGst?: number;
  amountTotal: number;
  note?: string;
  calculatedPaymentStatus?: string;
  leasePaymentStatusTypeCode?: ITypeCode<string>;
}

export interface IFormLeasePayment
  extends ExtendOverride<
    ILeasePayment,
    {
      amountPreTax: NumberFieldValue;
      amountGst: NumberFieldValue;
      amountPst: NumberFieldValue;
      amountTotal: NumberFieldValue;
    }
  > {}

export const formLeasePaymentToApiPayment = (
  formLeasePayment: IFormLeasePayment,
): ILeasePayment => {
  return {
    ...formLeasePayment,
    amountPreTax: stringToNull(formLeasePayment.amountPreTax),
    amountGst: stringToNull(formLeasePayment.amountGst),
    amountPst: stringToNull(formLeasePayment.amountPst),
    amountTotal: stringToNull(formLeasePayment.amountTotal),
    leasePaymentStatusTypeCode: formLeasePayment.leasePaymentStatusTypeCode?.id
      ? formLeasePayment.leasePaymentStatusTypeCode
      : undefined,
    leasePaymentMethodType: formLeasePayment.leasePaymentMethodType?.id
      ? formLeasePayment.leasePaymentMethodType
      : undefined,
  };
};

export const defaultFormLeasePayment: IFormLeasePayment = {
  id: 0,
  leaseTermId: 0,
  leasePaymentMethodType: defaultTypeCode,
  receivedDate: '',
  amountPreTax: '',
  amountPst: '',
  amountGst: '',
  amountTotal: '',
  note: '',
  leasePaymentStatusTypeCode: defaultTypeCode,
};
