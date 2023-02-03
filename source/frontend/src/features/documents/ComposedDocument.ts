import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import {
  Api_Document,
  Api_DocumentMetadataUpdate,
  Api_DocumentRelationship,
  Api_DocumentType,
  Api_DocumentUpdateRequest,
  Api_DocumentUploadRequest,
} from 'models/api/Document';
import {
  Api_Storage_DocumentDetail,
  Api_Storage_DocumentMetadata,
  Api_Storage_DocumentTypeMetadataType,
} from 'models/api/DocumentStorage';
import Api_TypeCode from 'models/api/TypeCode';

export interface ComposedDocument {
  mayanMetadata?: Api_Storage_DocumentMetadata[];
  pimsDocument?: Api_Document;
  documentDetail?: Api_Storage_DocumentDetail;
  mayanFileId?: number;
}

export class DocumentRow {
  id?: number;
  mayanDocumentId?: number;
  documentType?: Api_DocumentType;
  statusTypeCode?: Api_TypeCode<string>;
  fileName?: string;
  isFileAvailable?: boolean;
  appCreateTimestamp?: string;
  appCreateUserid?: string;
  relationshipId?: number;
  relationshipType?: DocumentRelationshipType;
  parentId?: number;

  public static fromApi(relationship: Api_DocumentRelationship): DocumentRow {
    const row: DocumentRow = new DocumentRow();
    row.id = relationship.document?.id;
    row.documentType = relationship.document?.documentType;
    row.mayanDocumentId = relationship.document?.mayanDocumentId;
    row.statusTypeCode = relationship.document?.statusTypeCode;
    row.fileName = relationship.document?.fileName;
    row.appCreateTimestamp = relationship.document?.appCreateTimestamp;
    row.appCreateUserid = relationship.document?.appCreateUserid;

    row.relationshipId = relationship.id;
    row.relationshipType = relationship.relationshipType;
    row.parentId = relationship.parentId;
    return row;
  }

  public static toApi(document: DocumentRow): Api_DocumentRelationship {
    return {
      id: document.relationshipId,
      relationshipType: document.relationshipType,
      parentId: document.parentId,
      document: {
        id: document.id,
        mayanDocumentId: document.mayanDocumentId,
        documentType: document.documentType,
        statusTypeCode: document.statusTypeCode,
        fileName: document.fileName,
      },
    };
  }
}

export class DocumentUploadFormData {
  public documentTypeId: string;
  public documentStatusCode: string;
  public documentMetadata: Record<string, string>;

  public constructor(
    initialStatus: string,
    documentType: string,
    metadata: Api_Storage_DocumentTypeMetadataType[],
  ) {
    this.documentStatusCode = initialStatus;
    this.documentTypeId = documentType;
    this.documentMetadata = {};

    metadata.forEach(metaType => {
      this.documentMetadata[metaType.metadata_type?.id?.toString() || '-'] = '';
    });
  }

  public toRequestApi(file: File, documentType: Api_DocumentType): Api_DocumentUploadRequest {
    var metadata: Api_DocumentMetadataUpdate[] = [];
    for (const key in this.documentMetadata) {
      const value = this.documentMetadata[key];
      metadata.push({
        metadataTypeId: Number(key),
        value: value,
      });
    }

    return {
      documentType: documentType,
      documentStatusCode: this.documentStatusCode,
      file: file,
      documentMetadata: metadata,
    };
  }
}

export class DocumentUpdateFormData {
  public documentId: number;
  public mayanDocumentId: number;
  public documentStatusCode: string;
  public documentMetadata: Record<string, string>;

  public static fromApi(
    composedDocument: ComposedDocument,
    metadataTypes: Api_Storage_DocumentTypeMetadataType[],
  ): DocumentUpdateFormData {
    var model = new DocumentUpdateFormData();
    model.documentId = composedDocument.pimsDocument?.id || 0;
    model.mayanDocumentId = composedDocument.pimsDocument?.mayanDocumentId || 0;
    model.documentStatusCode = composedDocument.pimsDocument?.statusTypeCode?.id?.toString() || '';
    model.documentMetadata = {};
    metadataTypes.forEach(metaType => {
      var foundMetadata = composedDocument.mayanMetadata?.find(
        currentMeta => currentMeta.metadata_type.id === metaType.metadata_type?.id,
      );
      model.documentMetadata[metaType.metadata_type?.id?.toString() || '-'] =
        foundMetadata?.value ?? '';
    });
    return model;
  }

  public toRequestApi(): Api_DocumentUpdateRequest {
    var metadata: Api_DocumentMetadataUpdate[] = [];

    for (const key in this.documentMetadata) {
      const value = this.documentMetadata[key];
      const metadataTypeId = Number(key);
      metadata.push({
        value: value,
        metadataTypeId: metadataTypeId,
      });
    }

    return {
      documentId: this.documentId,
      mayanDocumentId: this.mayanDocumentId,
      documentStatusCode: this.documentStatusCode,
      documentMetadata: metadata,
    };
  }

  private constructor() {
    this.documentId = -1;
    this.mayanDocumentId = -1;
    this.documentStatusCode = '';
    this.documentMetadata = {};
  }
}
