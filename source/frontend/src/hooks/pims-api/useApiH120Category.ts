import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { ApiGen_Concepts_H120Category } from '@/models/api/generated/ApiGen_Concepts_H120Category';

export const getH120Categories = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_H120Category[]>(`/h120category`);
