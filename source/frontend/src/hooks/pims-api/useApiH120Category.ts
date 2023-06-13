import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { Api_H120Category } from '@/models/api/H120Category';

export const getH120Categories = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_H120Category[]>(`/h120category`);
