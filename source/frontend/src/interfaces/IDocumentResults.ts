export interface IDocumentFilter {
  documentTypeId?: number;
  status: string;
  filename: string;
  parentName: string;
  parentType: string;
}

export const defaultDocumentFilter: IDocumentFilter = {
  documentTypeId: undefined,
  status: '',
  filename: '',
  parentName: '',
  parentType: '',
};
