import {
  Api_Document,
  Api_DocumentMetadataUpdate,
  Api_DocumentType,
  Api_DocumentUpdateRequest,
  Api_DocumentUploadRequest,
} from 'models/api/Document';
import { Api_Storage_DocumentMetadata } from 'models/api/DocumentStorage';

export interface ComposedDocument {
  mayanMetadata?: Api_Storage_DocumentMetadata[];
  pimsDocument?: Api_Document;
  mayanFileId?: number;
}

export class DocumentUploadFormData {
  public documentTypeId: string;
  public documentStatusCode: string;
  public documentMetadata: Record<string, string>;

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

  public constructor(initialStatus: string) {
    this.documentTypeId = '';
    this.documentStatusCode = initialStatus;
    this.documentMetadata = {};
  }
}

export class DocumentUpdateFormData {
  public documentId: number;
  public mayanDocumentId: number;
  public documentStatusCode: string;
  public documentMetadata: Record<string, string>;

  public static fromApi(composedDocument: ComposedDocument): DocumentUpdateFormData {
    var model = new DocumentUpdateFormData();
    model.documentId = composedDocument.pimsDocument?.id || 0;
    model.mayanDocumentId = composedDocument.pimsDocument?.mayanDocumentId || 0;
    model.documentStatusCode = composedDocument.pimsDocument?.statusTypeCode?.id?.toString() || '';
    model.documentMetadata = {};
    composedDocument?.mayanMetadata?.forEach(x => {
      model.documentMetadata[x.metadata_type.id || 'T'] = x.value;
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
