export interface IDocumentFilter {
  documentType?: number;
  status: string;
}

export const defaultDocumentFilter: IDocumentFilter = {
  documentType: undefined,
  status: '',
};
