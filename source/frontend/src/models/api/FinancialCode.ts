import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_FinancialCode extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  type?: string;
  code?: string;
  description?: string;
  displayOrder?: number;
  effectiveDate?: string;
  expiryDate?: string;
}
