import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion_Null } from './ConcurrentVersion';

export interface Api_PayeeCheque extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number | null;
  acquisitionPayeeId: number | null;
  isPaymentInTrust: boolean | null;
  pretaxAmount: number | null;
  taxAmount: number | null;
  totalAmount: number | null;
  gstNumber: string | null;
}
