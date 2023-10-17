import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { Api_ExpropriationPayment } from '@/models/api/ExpropriationPayment';

export const getForm8Api = (id: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_ExpropriationPayment>(
    `/expropriation-payments/${id}`,
  );

export const putForm8Api = (form8: Api_ExpropriationPayment) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_ExpropriationPayment>(
    `/expropriation-payments/${form8.id}`,
    form8,
  );

export const deleteForm8Api = (id: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(`/expropriation-payments/${id}`);
