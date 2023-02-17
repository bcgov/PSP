import { DocumentRelationshipType } from 'constants/documentRelationshipType';

import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Storage_DocumentDetail, Api_Storage_DocumentMetadata } from './DocumentStorage';
import { ExternalResult } from './ExternalResult';
import Api_TypeCode from './TypeCode';

export interface Api_Document extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  mayanDocumentId: number | undefined;
  documentType: Api_DocumentType | undefined;
  statusTypeCode: Api_TypeCode<string> | undefined;
  fileName: string | undefined;
}

export interface Api_DocumentType extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  documentType: string | undefined;
  mayanId: number | undefined;
}

export interface Api_DocumentRelationship extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  parentId: number | undefined;
  isDisabled?: boolean;
  document: Api_Document | undefined;
  relationshipType: DocumentRelationshipType | undefined;
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

export interface Api_DocumentUploadRelationshipResponse {
  documentRelationship: Api_DocumentRelationship | undefined;
  uploadResponse: Api_DocumentUploadResponse | undefined;
}

export interface Api_DocumentUploadResponse {
  document: Api_Document | undefined;
  documentExternalResult: ExternalResult<Api_Storage_DocumentDetail> | undefined;
  metadataExternalResult: ExternalResult<Api_Storage_DocumentMetadata> | undefined;
}

export interface Api_DocumentUpdateRequest {
  documentId: number;
  mayanDocumentId: number;
  documentStatusCode: string;
  documentMetadata: Api_DocumentMetadataUpdate[];
}

export interface Api_DocumentUpdateResponse {
  metadataExternalResult: ExternalResult<Api_Storage_DocumentMetadata> | undefined;
}
