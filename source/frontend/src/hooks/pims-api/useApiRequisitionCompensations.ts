import { ENVIRONMENT } from 'constants/environment';
import CustomAxios from 'customAxios';
import { Api_Compensation } from 'models/api/Compensation';

export const postRequisitionCompensationApi = (fileId: number, compensation: Api_Compensation) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<Api_Compensation>(
    `/acquisitionFile/${fileId}/compensations`,
    compensation,
  );

export const getFileRequisitionCompensationsApi = (fileId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_Compensation[]>(
    `/acquisitionFile/${fileId}/compensations`,
  );

export const getRequisitionCompensationApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_Compensation>(
    `/compensations/${compensationId}`,
  );

export const deleteRequisitionCompensationApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(`/compensations/${compensationId}`);
