import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_Document extends Api_ConcurrentVersion, Api_AuditFields {
  id: number;
  documentTypeId: number;
  documentType: string;
  fileName: string;
  statusTypeCode?: Api_TypeCode<string>;
}

export interface Api_Document_Type extends Api_ConcurrentVersion, Api_AuditFields {
  id: number;
  documentType: string;
}
