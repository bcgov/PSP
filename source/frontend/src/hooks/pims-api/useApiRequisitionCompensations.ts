import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';

export const getCompensationRequisitionApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_CompensationRequisition>(
    `/compensation-requisitions/${compensationId}`,
  );

export const putCompensationRequisitionApi = (
  compensation: ApiGen_Concepts_CompensationRequisition,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_CompensationRequisition>(
    `/compensation-requisitions/${compensation.id}`,
    compensation,
  );

export const deleteCompensationRequisitionApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(
    `/compensation-requisitions/${compensationId}`,
  );

export const getCompensationRequisitionPropertiesApi = (
  fileType: ApiGen_CodeTypes_FileTypes,
  compensationId: number,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_AcquisitionFileProperty[]>(
    `/compensation-requisitions/${fileType}/${compensationId}/properties`,
  );

export const getFileCompensationsApi = (fileType: ApiGen_CodeTypes_FileTypes, fileId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_CompensationRequisition[]>(
    `/compensation-requisitions/${fileType}/${fileId}`,
  );

export const postFileCompensationRequisitionApi = (
  fileType: ApiGen_CodeTypes_FileTypes,
  compensationRequisition: ApiGen_Concepts_CompensationRequisition,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<ApiGen_Concepts_CompensationRequisition>(
    `/compensation-requisitions/${fileType}`,
    compensationRequisition,
  );
