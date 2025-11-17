import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Mayan_DocumentDetail } from '@/models/api/generated/ApiGen_Mayan_DocumentDetail';
import { ApiGen_Mayan_DocumentMetadata } from '@/models/api/generated/ApiGen_Mayan_DocumentMetadata';

export interface ComposedDocument {
  mayanMetadata?: ApiGen_Mayan_DocumentMetadata[];
  pimsDocumentRelationship?: ApiGen_Concepts_DocumentRelationship;
  documentDetail?: ApiGen_Mayan_DocumentDetail;
  mayanFileId?: number;
}
