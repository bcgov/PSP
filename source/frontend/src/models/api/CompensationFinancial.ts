import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion_Null } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_CompensationFinancial extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number | null;
  compensationId: number;
  financialActivityCodeId: number;
  financialActivityCode: Api_TypeCode<number> | null;
  pretaxAmount: number | null;
  isGstRequired: boolean | null;
  taxAmount: number | null;
  totalAmount: number | null;
  isDisabled: boolean | null;
}
