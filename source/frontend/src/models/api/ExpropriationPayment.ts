import { Api_AcquisitionFileOwner } from './AcquisitionFile';
import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion_Null } from './ConcurrentVersion';
import { Api_InterestHolder } from './InterestHolder';
import { Api_Organization } from './Organization';
import Api_TypeCode from './TypeCode';

export interface Api_ExpropriationPayment extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number | null;
  acquisitionFileId: number;
  acquisitionOwnerId: number | null;
  acquisitionOwner: Api_AcquisitionFileOwner | null;
  interestHolderId: number | null;
  interestHolder: Api_InterestHolder | null;
  expropriatingAuthorityId: number | null;
  expropriatingAuthority: Api_Organization | null;
  description: string | null;
  isDisabled: boolean | null;
  paymentItems: Api_ExpropriationPaymentItem[] | null;
}

export interface Api_ExpropriationPaymentItem extends Api_ConcurrentVersion_Null, Api_AuditFields {
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
