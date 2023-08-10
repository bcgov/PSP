import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { Api_ExpropriationPayment } from '@/models/api/Form8';

export const getForm8Api = (id: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_ExpropriationPayment>(`/form8/${id}`);

export const putForm8Api = (form8: Api_ExpropriationPayment) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_ExpropriationPayment>(`/form8/${form8.id}`, form8);
