export interface IDocumentFilter {
  documentTypeId?: number;
  status: string;
}

export const defaultDocumentFilter: IDocumentFilter = {
  documentTypeId: undefined,
  status: '',
};
