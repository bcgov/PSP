import { DocumentRelationshipType } from 'constants/documentRelationshipType';

import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Storage_DocumentDetail } from './DocumentStorage';
import { ExternalResult } from './ExternalResult';
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
  mayanId?: number;
}

export interface Api_DocumentRelationship extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  parentId?: number;
  isDisabled?: boolean;
  document?: Api_Document;
  relationshipType?: DocumentRelationshipType;
}

export interface Api_DocumentMetadata {
  metadataTypeId: number;
  value: string;
}

export interface Api_UploadRequest {
  documentType: Api_DocumentType;
  documentStatusCode: string;
  file: File;
  documentMetadata?: any;
}

export interface Api_UploadResponse {
  documentRelationship?: Api_DocumentRelationship;
  externalResult?: ExternalResult<Api_Storage_DocumentDetail>;
}
