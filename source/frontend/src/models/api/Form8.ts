import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion_Null } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_Form8 extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number | null;
  acquisitionFileId: number;
  acquisitionOwnerId: number | null;
  interestHolderId: number | null;
  expropriatingAuthorityId: number | null;
  paymentItemTypeCode: Api_TypeCode<string> | null;
  description: string | null;
  isGstRequired: boolean | null;
  pretaxAmount: number | null;
  taxAmount: number | null;
  totalAmount: number | null;
  isDisabled: boolean | null;
}

export interface Api_PaymentItem extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number | null;
  form8Id: number;
  paymentItemTypeCode: Api_TypeCode<string> | null;
  isGstRequired: boolean | null;
  pretaxAmount: number | null;
  taxAmount: number | null;
  totalAmount: number | null;
}
