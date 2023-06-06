import { ENVIRONMENT } from 'constants/environment';
import CustomAxios from 'customAxios';
import { Api_CompensationPayee } from 'models/api/CompensationPayee';
import { Api_CompensationRequisition } from 'models/api/CompensationRequisition';

export const getCompensationRequisitionApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_CompensationRequisition>(
    `/compensation-requisitions/${compensationId}`,
  );

export const getCompensationRequisitionPayeeApi = (payeeId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_CompensationPayee>(
    `/compensation-requisitions/payees/${payeeId}`,
  );

export const putCompensationRequisitionApi = (compensation: Api_CompensationRequisition) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_CompensationRequisition>(
    `/compensation-requisitions/${compensation.id}`,
    compensation,
  );

export const deleteCompensationRequisitionApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(
    `/compensation-requisitions/${compensationId}`,
  );

export const getCompensationRequisitionPayeeApi = (compensationId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_CompensationPayee>(
    `/compensation-requisitions/${compensationId}/payee`,
  );
