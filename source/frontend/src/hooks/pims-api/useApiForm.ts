import { ENVIRONMENT } from 'constants/environment';
import { FileTypes } from 'constants/fileTypes';
import CustomAxios from 'customAxios';
import { Api_FileForm } from 'models/api/Form';

export const postFileForm = (fileType: FileTypes, form: Api_FileForm) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<Api_FileForm>(`/forms/${fileType}`, form);
