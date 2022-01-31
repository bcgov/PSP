import { NumberFieldValue } from 'typings/NumberFieldValue';
import { stringToNull } from 'utils/formUtils';

import ITypeCode from './ITypeCode';

export interface ILeasePayment {
  id?: number;
  leaseTermId: number;
  leasePaymentMethodType: ITypeCode<string>;
  receivedDate: string;
  amountPreTax: number;
  amountPst?: number;
  amountGst?: number;
  amountTotal: number;
  note?: string;
  leasePaymentStatusTypeCode: ITypeCode<string>;
}

export interface IFormLeasePayment
  extends ExtendOverride<
    ILeasePayment,
    {
      amountPreTax: NumberFieldValue;
      amountGst: NumberFieldValue;
      amountTotal: NumberFieldValue;
    }
  > {}

export const formLeasePaymentToApiPayment = (
  formLeasePayment: IFormLeasePayment,
  gstConstant?: number,
): ILeasePayment => {
  return {
    ...formLeasePayment,
    amountPreTax: stringToNull(formLeasePayment.amountPreTax),
    amountGst: stringToNull(formLeasePayment.amountGst),
    amountTotal: stringToNull(formLeasePayment.amountTotal),
  };
};
