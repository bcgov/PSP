import { ENVIRONMENT } from 'constants/environment';
import CustomAxios from 'customAxios';
import { Api_FormDocumentType } from 'models/api/formDocument';

export const getFormDocumentTypesApi = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FormDocumentType[]>(`/formDocument/`);
