import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_Document extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  mayanDocumentId?: number;
  documentType?: Api_DocumentType;
  statusTypeCode?: Api_TypeCode<string>;
  fileName?: string;
}

export interface Api_DocumentType extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  documentType?: string;
}
