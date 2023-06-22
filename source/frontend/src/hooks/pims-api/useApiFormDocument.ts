import { ENVIRONMENT } from '@/constants/environment';
import { FileTypes } from '@/constants/fileTypes';
import CustomAxios from '@/customAxios';
import { Api_FormDocumentFile, Api_FormDocumentType } from '@/models/api/FormDocument';

export const getFormDocumentTypesApi = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FormDocumentType[]>(`/formDocument/`);

export const postFileFormApi = (fileType: FileTypes, form: Api_FormDocumentFile) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<Api_FormDocumentFile>(
    `/formDocument/${fileType}`,
    form,
  );

export const getFileForms = (fileType: FileTypes, fileId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FormDocumentFile[]>(
    `/formDocument/${fileType}/file/${fileId}`,
  );

export const getFileForm = (fileType: FileTypes, formFileId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FormDocumentFile>(
    `/formDocument/${fileType}/${formFileId}`,
  );

export const deleteFileForm = (fileType: FileTypes, formFileId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(
    `/formDocument/${fileType}/${formFileId}`,
  );
