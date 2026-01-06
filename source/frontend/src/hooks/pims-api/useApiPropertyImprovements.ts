import { ENVIRONMENT } from '@/constants';
import CustomAxios from '@/customAxios';
import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';

export const getPropertyImprovementsApi = (propertyId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_PropertyImprovement[]>(
    `/properties/${propertyId}/improvements`,
  );

export const getPropertyImprovementApi = (propertyId: number, propertyImprovementId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_PropertyImprovement>(
    `/properties/${propertyId}/improvements/${propertyImprovementId}`,
  );

export const postPropertyImprovementsApi = (
  propertyId: number,
  propertyImprovement: ApiGen_Concepts_PropertyImprovement,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<ApiGen_Concepts_PropertyImprovement>(
    `/properties/${propertyId}/improvements`,
    propertyImprovement,
  );

export const putPropertyImprovementsApi = (
  propertyId: number,
  propertyImprovementId: number,
  propertyImprovement: ApiGen_Concepts_PropertyImprovement,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_PropertyImprovement>(
    `/properties/${propertyId}/improvements/${propertyImprovementId}`,
    propertyImprovement,
  );

export const deletePropertyImprovementsApi = (propertyId: number, propertyImprovementId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(
    `/properties/${propertyId}/improvements/${propertyImprovementId}`,
  );
