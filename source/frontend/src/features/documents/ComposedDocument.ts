import { IApiError, isApiError } from '@/interfaces/IApiError';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_Document } from '@/models/api/generated/ApiGen_Concepts_Document';
import { ApiGen_Concepts_DocumentMetadataUpdate } from '@/models/api/generated/ApiGen_Concepts_DocumentMetadataUpdate';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { ApiGen_Mayan_DocumentDetail } from '@/models/api/generated/ApiGen_Mayan_DocumentDetail';
import { ApiGen_Mayan_DocumentMetadata } from '@/models/api/generated/ApiGen_Mayan_DocumentMetadata';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { ApiGen_Requests_DocumentUpdateRequest } from '@/models/api/generated/ApiGen_Requests_DocumentUpdateRequest';
import { ApiGen_Requests_DocumentUploadRelationshipResponse } from '@/models/api/generated/ApiGen_Requests_DocumentUploadRelationshipResponse';
import { ApiGen_Requests_DocumentUploadRequest } from '@/models/api/generated/ApiGen_Requests_DocumentUploadRequest';
import { EpochIsoDateTime, UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { exists } from '@/utils';
import { stringToNumber } from '@/utils/formUtils';

export interface ComposedDocument {
  mayanMetadata?: ApiGen_Mayan_DocumentMetadata[];
  pimsDocumentRelationship?: ApiGen_Concepts_DocumentRelationship;
  documentDetail?: ApiGen_Mayan_DocumentDetail;
  mayanFileId?: number;
}

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

  public static fromApi(relationship: ApiGen_Concepts_DocumentRelationship): DocumentRow {
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

export class BatchUploadFormModel {
  public documents: DocumentUploadFormData[] = [];
}

export class BatchUploadResponseModel {
  public readonly fileName: string;
  public readonly isSuccess: boolean;
  public readonly errorMessage: string;

  constructor(
    fileName: string,
    apiResponse: ApiGen_Requests_DocumentUploadRelationshipResponse | IApiError | undefined,
  ) {
    this.fileName = fileName;
    if (exists(apiResponse)) {
      if (isApiError(apiResponse)) {
        this.isSuccess = false;
        this.errorMessage = (apiResponse as IApiError).details;
      } else {
        this.isSuccess = true;
      }
    } else {
      this.isSuccess = false;
      this.errorMessage = 'Network error, please try again or contact your system administrator';
    }
  }
}

export class DocumentUploadFormData {
  public documentTypeId: string;
  public documentStatusCode: string;
  public documentMetadata: Record<string, string>;
  public isDocumentTypeChanged = false;
  public mayanMetadata: ApiGen_Mayan_DocumentTypeMetadataType[];
  public file: File | null;

  public constructor(
    initialStatus: string,
    documentType: string,
    metadata: ApiGen_Mayan_DocumentTypeMetadataType[],
  ) {
    this.documentStatusCode = initialStatus;
    this.documentTypeId = documentType;
    this.file = null;
    this.setMayanMetadata(metadata);
  }

  public setMayanMetadata(metadata: ApiGen_Mayan_DocumentTypeMetadataType[]) {
    this.mayanMetadata = metadata;
    this.documentMetadata = {};

    metadata.forEach(metaType => {
      this.documentMetadata[metaType.metadata_type?.id?.toString() || '-'] = '';
    });
  }

  public toRequestApi(
    documentTypes: ApiGen_Concepts_DocumentType[],
  ): ApiGen_Requests_DocumentUploadRequest {
    const documentType = documentTypes.find(x => x.id === Number(this.documentTypeId));
    const metadata: ApiGen_Concepts_DocumentMetadataUpdate[] = [];
    for (const key in this.documentMetadata) {
      const value = this.documentMetadata[key];
      metadata.push({
        metadataTypeId: Number(key),
        value: value,
        id: 0,
      });
    }

    return {
      documentTypeId: documentType?.id,
      documentId: null,
      documentStatusCode: this.documentStatusCode,
      file: this.file,
      documentMetadata: metadata,
      documentTypeMayanId: documentType?.mayanId,
    };
  }
}

export class DocumentUpdateFormData {
  public documentId: number;
  public mayanDocumentId: number;
  public documentStatusCode: string;
  public documentMetadata: Record<string, string>;
  public documentTypeId = '';

  public static fromApi(
    composedDocument: ComposedDocument,
    metadataTypes: ApiGen_Mayan_DocumentTypeMetadataType[],
  ): DocumentUpdateFormData {
    const model = new DocumentUpdateFormData();
    model.documentId = composedDocument.pimsDocumentRelationship?.document?.id || 0;
    model.mayanDocumentId =
      composedDocument.pimsDocumentRelationship?.document?.mayanDocumentId || 0;
    model.documentStatusCode =
      composedDocument.pimsDocumentRelationship?.document?.statusTypeCode?.id?.toString() || '';
    model.documentMetadata = {};
    metadataTypes.forEach(metaType => {
      const foundMetadata = composedDocument.mayanMetadata?.find(
        currentMeta => currentMeta?.metadata_type?.id === metaType.metadata_type?.id,
      );
      model.documentMetadata[metaType.metadata_type?.id?.toString() || '-'] =
        foundMetadata?.value ?? '';
    });
    const documentTypeLabel = composedDocument.pimsDocumentRelationship?.document?.documentType?.id;

    model.documentTypeId = documentTypeLabel?.toString() || '';

    return model;
  }

  public static toRequestApi(
    formData: DocumentUpdateFormData,
  ): ApiGen_Requests_DocumentUpdateRequest {
    const metadata: ApiGen_Concepts_DocumentMetadataUpdate[] = [];

    for (const key in formData.documentMetadata) {
      const value = formData.documentMetadata[key];
      const metadataTypeId = Number(key);
      metadata.push({
        value: value,
        metadataTypeId: metadataTypeId,
        id: 0,
      });
    }

    return {
      documentId: formData.documentId,
      mayanDocumentId: formData.mayanDocumentId,
      documentTypeId: stringToNumber(formData.documentTypeId),
      documentStatusCode: formData.documentStatusCode,
      documentMetadata: metadata,
    };
  }

  private constructor() {
    this.documentId = -1;
    this.mayanDocumentId = -1;
    this.documentTypeId = '';
    this.documentStatusCode = '';
    this.documentMetadata = {};
  }
}
