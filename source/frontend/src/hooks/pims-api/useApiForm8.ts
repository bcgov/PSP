import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { ApiGen_Concepts_ExpropriationPayment } from '@/models/api/generated/ApiGen_Concepts_ExpropriationPayment';

export const getForm8Api = (id: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_ExpropriationPayment>(
    `/expropriation-payments/${id}`,
  );

export const putForm8Api = (form8: ApiGen_Concepts_ExpropriationPayment) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_ExpropriationPayment>(
    `/expropriation-payments/${form8.id}`,
    form8,
  );

export const deleteForm8Api = (id: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(`/expropriation-payments/${id}`);
