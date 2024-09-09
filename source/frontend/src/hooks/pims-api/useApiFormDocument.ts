import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_FormDocumentFile } from '@/models/api/generated/ApiGen_Concepts_FormDocumentFile';
import { ApiGen_Concepts_FormDocumentType } from '@/models/api/generated/ApiGen_Concepts_FormDocumentType';

export const getFormDocumentTypesApi = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_FormDocumentType[]>(
    `/formDocument/`,
  );

export const postFileFormApi = (
  fileType: ApiGen_CodeTypes_FileTypes,
  form: ApiGen_Concepts_FormDocumentFile,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<ApiGen_Concepts_FormDocumentFile>(
    `/formDocument/${fileType}`,
    form,
  );

export const getFileForms = (fileType: ApiGen_CodeTypes_FileTypes, fileId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_FormDocumentFile[]>(
    `/formDocument/${fileType}/file/${fileId}`,
  );

export const getFileForm = (fileType: ApiGen_CodeTypes_FileTypes, formFileId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_FormDocumentFile>(
    `/formDocument/${fileType}/${formFileId}`,
  );

export const deleteFileForm = (fileType: ApiGen_CodeTypes_FileTypes, formFileId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(
    `/formDocument/${fileType}/${formFileId}`,
  );
