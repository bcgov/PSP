import { Api_Document } from 'models/api/Document';
import { Mayan_DocumentMetadata } from 'models/api/DocumentManagement';

export interface ComposedDocument {
  mayanMetadata?: Mayan_DocumentMetadata[];
  pimsDocument?: Api_Document;
}
