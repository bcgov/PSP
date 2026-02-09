import { ApiGen_Concepts_DocumentSearchFilter } from '@/models/api/generated/ApiGen_Concepts_DocumentSearchFilter';

export class DocumentSearchFilterModel {
  documentTypTypeCode = '';
  documentStatusTypeCode = '';
  documentName = '';
  content = '';
  searchBy = 'pid';
  pin: string;
  pid: string;
  plan: string;
  mayanDocumentIds: number[] | null;

  toApi(): ApiGen_Concepts_DocumentSearchFilter {
    return {
      content: this.content,
      documentName: this.documentName,
      documentTypTypeCode: this.documentTypTypeCode,
      documentStatusTypeCode: this.documentStatusTypeCode,
      pid: this.pid,
      pin: this.pin,
      plan: this.plan,
      mayanDocumentIds: this.mayanDocumentIds,
    };
  }

  static fromApi(base: ApiGen_Concepts_DocumentSearchFilter): DocumentSearchFilterModel {
    const newModel = new DocumentSearchFilterModel();
    newModel.documentName = base.documentName;

    return newModel;
  }
}
