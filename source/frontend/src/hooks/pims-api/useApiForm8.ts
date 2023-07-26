import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { Api_Form8 } from '@/models/api/Form8';

export const getForm8Api = (id: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_Form8>(`/form8/${id}`);

export const putForm8Api = (form8: Api_Form8) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_Form8>(`/form8/${form8.id}`, form8);
