import { ENVIRONMENT } from 'constants/environment';
import { FileTypes } from 'constants/fileTypes';
import CustomAxios from 'customAxios';
import { Api_FileForm } from 'models/api/Form';

export const postFileForm = (fileType: FileTypes, form: Api_FileForm) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<Api_FileForm>(`/forms/${fileType}`, form);
export const getFileForms = (fileType: FileTypes, fileId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FileForm[]>(
    `/forms/${fileType}/file/${fileId}`,
  );
export const getFileForm = (fileType: FileTypes, formFileId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FileForm>(
    `/forms/${fileType}/${formFileId}`,
  );
export const deleteFileForm = (fileType: FileTypes, formFileId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(`/forms/${fileType}/${formFileId}`);
