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
