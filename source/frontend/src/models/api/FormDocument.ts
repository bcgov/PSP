import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_FormDocumentType extends Api_ConcurrentVersion {
  documentId: number | null;
  formTypeCode: string;
  description: string;
  displayOrder: number | null;
}

export interface Api_FormDocumentFile extends Api_ConcurrentVersion {
  id: number | null;
  fileId: number;
  formDocumentType: Api_FormDocumentType;
}
