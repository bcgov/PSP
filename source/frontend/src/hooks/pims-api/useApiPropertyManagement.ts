import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { Api_PropertyManagement } from '@/models/api/Property';

export const getPropertyManagementApi = (propertyId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_PropertyManagement>(
    `/properties/${propertyId}/management`,
  );

export const putPropertyManagementApi = (
  propertyId: number,
  propertyManagement: Api_PropertyManagement,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_PropertyManagement>(
    `/properties/${propertyId}/management`,
    propertyManagement,
  );
