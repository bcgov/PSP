import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { Api_PropertyContact } from '@/models/api/Property';

export const getPropertyContactsApi = (propertyId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_PropertyContact[]>(
    `/properties/${propertyId}/contacts`,
  );

export const putPropertyContactsApi = (propertyId: number, contact: Api_PropertyContact) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_PropertyContact>(
    `/properties/${propertyId}/contacts`,
    contact,
  );

export const deletePropertyContactsApi = (propertyId: number, contactId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(
    `/properties/${propertyId}/contacts/${contactId}`,
  );
