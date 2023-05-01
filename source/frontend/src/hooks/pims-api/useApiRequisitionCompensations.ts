import { ENVIRONMENT } from 'constants/environment';
import CustomAxios from 'customAxios';
import { Api_Compensation } from 'models/api/Compensation';

export const getRequisitionCompensationApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_Compensation>(
    `/compensation-requisitions/${compensationId}`,
  );

export const deleteRequisitionCompensationApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(
    `/compensation-requisitions/${compensationId}`,
  );
