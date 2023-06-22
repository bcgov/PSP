import { DocumentRelationshipType } from '@/constants/documentRelationshipType';

import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Storage_DocumentDetail, Api_Storage_DocumentMetadata } from './DocumentStorage';
import { ExternalResult } from './ExternalResult';
import Api_TypeCode from './TypeCode';

export interface Api_Document extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  mayanDocumentId: number | null;
  documentType: Api_DocumentType | null;
  statusTypeCode: Api_TypeCode<string> | null;
  fileName: string | null;
}

export interface Api_DocumentType extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  documentType: string | null;
  documentTypeDescription: string | null;
  mayanId: number | null;
  isDisabled: boolean | null;
}

export interface Api_DocumentRelationship extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  parentId: number | null;
  isDisabled?: boolean;
  document: Api_Document | null;
  relationshipType: DocumentRelationshipType | null;
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
  documentRelationship: Api_DocumentRelationship | null;
  uploadResponse: Api_DocumentUploadResponse | null;
}

export interface Api_DocumentUploadResponse {
  document: Api_Document | null;
  documentExternalResult: ExternalResult<Api_Storage_DocumentDetail> | null;
  metadataExternalResult: ExternalResult<Api_Storage_DocumentMetadata> | null;
}

export interface Api_DocumentUpdateRequest {
  documentId: number;
  mayanDocumentId: number;
  documentStatusCode: string;
  documentMetadata: Api_DocumentMetadataUpdate[];
}

export interface Api_DocumentUpdateResponse {
  metadataExternalResult: ExternalResult<Api_Storage_DocumentMetadata> | null;
}
