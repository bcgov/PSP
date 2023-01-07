import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_FinancialCode extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  type?: string;
}
