import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { ApiGen_Concepts_PropertyManagement } from '@/models/api/generated/ApiGen_Concepts_PropertyManagement';

export const getPropertyManagementApi = (propertyId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_PropertyManagement>(
    `/properties/${propertyId}/management`,
  );

export const putPropertyManagementApi = (
  propertyId: number,
  propertyManagement: ApiGen_Concepts_PropertyManagement,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_PropertyManagement>(
    `/properties/${propertyId}/management`,
    propertyManagement,
  );
