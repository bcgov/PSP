import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_FinancialCode<T> extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  type?: string;
  code?: T;
  description?: string;
  displayOrder?: number;
  effectiveDate?: string;
  expiryDate?: string;
}
