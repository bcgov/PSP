import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_LeasePayment extends Api_ConcurrentVersion {
  id: number | null;
  leaseTermId: number;
  leasePaymentMethodType: Api_TypeCode<string>;
  receivedDate: string;
  amountPreTax: number;
  amountPst: number | null;
  amountGst: number | null;
  amountTotal: number;
  note: string | null;
  leasePaymentStatusTypeCode: Api_TypeCode<string> | null;
}
