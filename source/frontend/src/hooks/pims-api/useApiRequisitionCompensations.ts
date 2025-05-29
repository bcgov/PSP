import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_CompReqAcqPayee } from '@/models/api/generated/ApiGen_Concepts_CompReqAcqPayee';
import { ApiGen_Concepts_CompReqLeasePayee } from '@/models/api/generated/ApiGen_Concepts_CompReqLeasePayee';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';

export const getCompensationRequisitionApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_CompensationRequisition>(
    `/compensation-requisitions/${compensationId}`,
  );

export const putCompensationRequisitionApi = (
  fileType: ApiGen_CodeTypes_FileTypes,
  compensation: ApiGen_Concepts_CompensationRequisition,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_CompensationRequisition>(
    `/compensation-requisitions/${fileType}/${compensation.id}`,
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

export const getCompensationRequisitionFinancialsApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_CompensationFinancial[]>(
    `/compensation-requisitions/${compensationId}/financials`,
  );

export const getCompensationRequisitionAcqPayeesApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_CompReqAcqPayee[]>(
    `/compensation-requisitions/${compensationId}/acquisition-payees`,
  );

export const getCompensationRequisitionLeasePayeesApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_CompReqLeasePayee[]>(
    `/compensation-requisitions/${compensationId}/lease-payees`,
  );

export const getCompensationRequisitionAtTimeApi = (compensationId: number, time: string) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_CompensationRequisition>(
    `/compensation-requisitions/${compensationId}/historical?time=${time}`,
  );

export const getCompensationRequisitionAcqPropertiesAtTimeApi = (
  compensationId: number,
  time: string,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_AcquisitionFileProperty[]>(
    `/compensation-requisitions/acquisition/${compensationId}/properties/historical?time=${time}`,
  );

export const getCompensationRequisitionLeasePropertiesAtTimeApi = (
  compensationId: number,
  time: string,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_PropertyLease[]>(
    `/compensation-requisitions/lease/${compensationId}/properties/historical?time=${time}`,
  );

export const getCompensationRequisitionAcqPayeesAtTimeApi = (
  compensationId: number,
  time: string,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_CompReqAcqPayee[]>(
    `/compensation-requisitions/${compensationId}/acquisition-payees/historical?time=${time}`,
  );

export const getCompensationRequisitionLeasePayeesAtTimeApi = (
  compensationId: number,
  time: string,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_CompReqLeasePayee[]>(
    `/compensation-requisitions/${compensationId}/lease-payees/historical?time=${time}`,
  );
