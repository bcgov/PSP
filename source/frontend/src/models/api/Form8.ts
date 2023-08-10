import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion_Null } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_ExpropriationPayment extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number | null;
  acquisitionFileId: number;
  acquisitionOwnerId: number | null;
  interestHolderId: number | null;
  expropriatingAuthorityId: number | null;
  description: string | null;
  isDisabled: boolean | null;
  paymentItems: Api_ExpropiationPaymentItem[] | null;
}

export interface Api_ExpropiationPaymentItem extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number | null;
  expropriationPaymentId: number | null;
  paymentItemTypeCode: string | null;
  paymentItemType: Api_TypeCode<string> | null;
  isGstRequired: boolean | null;
  pretaxAmount: number | null;
  taxAmount: number | null;
  totalAmount: number | null;
  isDisabled: boolean | null;
}
