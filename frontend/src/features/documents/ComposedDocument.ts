import { Api_Document } from 'models/api/Document';
import { Api_Storage_DocumentMetadata } from 'models/api/DocumentStorage';

export interface ComposedDocument {
  mayanMetadata?: Api_Storage_DocumentMetadata[];
  pimsDocument?: Api_Document;
}

export interface DocumentMetadataForm {
  documentTypeId: string;
  documentStatusCode: string;
}
