import { ENVIRONMENT } from 'constants/environment';
import CustomAxios from 'customAxios';
import { Api_Compensation } from 'models/api/Compensation';

export const getCompensationRequisitionApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_Compensation>(
    `/compensation-requisitions/${compensationId}`,
  );

export const putCompensationRequisitionApi = (compensation: Api_Compensation) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_Compensation>(
    `/compensation-requisitions/${compensation.id}`,
    compensation,
  );

export const deleteCompensationRequisitionApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(
    `/compensation-requisitions/${compensationId}`,
  );
