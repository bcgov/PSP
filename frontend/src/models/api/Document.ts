import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_Document extends Api_ConcurrentVersion, Api_AuditFields {
  id: number;
  documentTypeId: number;
  documentType: string;
  fileName: string;
  statusCode: string;
  status: string;
}

export interface Api_Document_Type extends Api_ConcurrentVersion, Api_AuditFields {
  id: number;
  documentType: string;
}
