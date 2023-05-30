import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion_Null } from './ConcurrentVersion';

export interface Api_Payee extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number | null;
  compensationRequisitionId: number;
}
