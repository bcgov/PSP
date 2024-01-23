import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { ApiGen_Concepts_PropertyContact } from '@/models/api/generated/ApiGen_Concepts_PropertyContact';

export const getPropertyContactsApi = (propertyId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_PropertyContact[]>(
    `/properties/${propertyId}/contacts`,
  );

export const getPropertyContactApi = (propertyId: number, contactId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_PropertyContact>(
    `/properties/${propertyId}/contacts/${contactId}`,
  );

export const postPropertyContactsApi = (
  propertyId: number,
  contact: ApiGen_Concepts_PropertyContact,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<ApiGen_Concepts_PropertyContact>(
    `/properties/${propertyId}/contacts`,
    contact,
  );

export const putPropertyContactsApi = (
  propertyId: number,
  contactId: number,
  contact: ApiGen_Concepts_PropertyContact,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_PropertyContact>(
    `/properties/${propertyId}/contacts/${contactId}`,
    contact,
  );

export const deletePropertyContactsApi = (propertyId: number, contactId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(
    `/properties/${propertyId}/contacts/${contactId}`,
  );
