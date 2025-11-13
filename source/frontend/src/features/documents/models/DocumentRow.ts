import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_Document } from '@/models/api/generated/ApiGen_Concepts_Document';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { EpochIsoDateTime, UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';

export class DocumentRow {
  id?: number;
  mayanDocumentId: number | null;
  documentType: ApiGen_Concepts_DocumentType | undefined;
  statusTypeCode: ApiGen_Base_CodeType<string> | undefined;
  queueStatusTypeCode: ApiGen_Base_CodeType<string> | null;
  fileName: string | undefined;
  isFileAvailable: boolean | undefined;
  appCreateTimestamp?: UtcIsoDateTime;
  appCreateUserid?: string;
  relationshipId: number | undefined;
  relationshipType: ApiGen_CodeTypes_DocumentRelationType | null = null;
  parentId: string | undefined;
  parentName: string | undefined;

  public static fromApi(
    relationship: ApiGen_Concepts_DocumentRelationship,
    parentName: string,
  ): DocumentRow {
    const row: DocumentRow = new DocumentRow();
    row.id = relationship.document?.id;
    row.documentType = relationship.document?.documentType ?? undefined;
    row.mayanDocumentId = relationship.document?.mayanDocumentId;
    row.queueStatusTypeCode = relationship.document?.documentQueueStatusTypeCode;
    row.statusTypeCode = relationship.document?.statusTypeCode ?? undefined;
    row.fileName = relationship.document?.fileName ?? undefined;
    row.appCreateTimestamp = relationship.document?.appCreateTimestamp;
    row.appCreateUserid = relationship.document?.appCreateUserid ?? undefined;

    row.relationshipId = relationship.id;
    row.relationshipType = relationship.relationshipType ?? null;
    row.parentId = relationship.parentId ?? undefined;
    row.parentName = parentName;
    return row;
  }

  public static fromApiDocument(document: ApiGen_Concepts_Document): DocumentRow {
    const row: DocumentRow = new DocumentRow();

    row.id = document?.id;
    row.documentType = document?.documentType ?? undefined;
    row.mayanDocumentId = document?.mayanDocumentId ?? undefined;
    row.statusTypeCode = document?.statusTypeCode ?? undefined;
    row.queueStatusTypeCode = document?.documentQueueStatusTypeCode;
    row.fileName = document?.fileName ?? undefined;
    row.appCreateTimestamp = document?.appCreateTimestamp;
    row.appCreateUserid = document?.appCreateUserid ?? undefined;

    row.relationshipId = undefined;
    row.relationshipType = null;
    row.parentId = undefined;
    row.parentName = '';
    return row;
  }

  public static toApi(document: DocumentRow): ApiGen_Concepts_DocumentRelationship {
    if (document.relationshipType === null) {
      throw new Error('Invalid document relationship type');
    }

    return {
      id: document.relationshipId || 0,
      relationshipType: document.relationshipType,
      parentId: document.parentId ?? null,
      parentNameOrNumber: document.parentName ?? '',
      document: {
        id: document.id || 0,
        mayanDocumentId: document.mayanDocumentId ?? 0,
        documentType: document.documentType ?? null,
        statusTypeCode: document.statusTypeCode ?? null,
        fileName: document.fileName ?? null,
        rowVersion: 1,

        appCreateTimestamp: EpochIsoDateTime,
        appLastUpdateTimestamp: EpochIsoDateTime,
        appLastUpdateUserid: null,
        appCreateUserid: null,
        appLastUpdateUserGuid: null,
        appCreateUserGuid: null,
        documentQueueStatusTypeCode: null,
      },
      rowVersion: 0,
      appCreateTimestamp: EpochIsoDateTime,
      appLastUpdateTimestamp: EpochIsoDateTime,
      appLastUpdateUserid: null,
      appCreateUserid: null,
      appLastUpdateUserGuid: null,
      appCreateUserGuid: null,
    };
  }
}
