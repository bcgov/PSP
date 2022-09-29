export interface IDocumentFilter {
  documentTypeId?: number;
  status: string;
  filename: string;
}

export const defaultDocumentFilter: IDocumentFilter = {
  documentTypeId: undefined,
  status: '',
  filename: '',
};
