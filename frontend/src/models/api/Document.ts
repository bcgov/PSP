import { DocumentRelationshipType } from 'constants/documentRelationshipType';

import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Storage_DocumentDetail, Api_Storage_DocumentMetadata } from './DocumentStorage';
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

export interface Api_DocumentMetadataUpdate {
  metadataTypeId: number;
  value: string;
}

export interface Api_DocumentUploadRequest {
  documentType: Api_DocumentType;
  documentStatusCode: string;
  file: File;
  documentMetadata?: Api_DocumentMetadataUpdate[];
}

export interface Api_DocumentUploadResponse {
  documentRelationship?: Api_DocumentRelationship;
  documentExternalResult?: ExternalResult<Api_Storage_DocumentDetail>;
  metadataExternalResult?: ExternalResult<Api_Storage_DocumentMetadata>;
}

export interface Api_DocumentUpdateRequest {
  documentId: number;
  mayanDocumentId: number;
  documentStatusCode: string;
  documentMetadata: Api_DocumentMetadataUpdate[];
}

export interface Api_DocumentUpdateResponse {
  metadataExternalResult?: ExternalResult<Api_Storage_DocumentMetadata>;
}
