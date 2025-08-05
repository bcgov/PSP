import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { ApiGen_Concepts_ManagementFileContact } from '@/models/api/generated/ApiGen_Concepts_ManagementFileContact';

export const getManagementFileContactsApi = (managementFileId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_ManagementFileContact[]>(
    `/managementfiles/${managementFileId}/contacts`,
  );

export const getManagementFileContactApi = (managementFileId: number, contactId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_ManagementFileContact>(
    `/managementfiles/${managementFileId}/contacts/${contactId}`,
  );

export const postManagementFileContactsApi = (
  managementFileId: number,
  contact: ApiGen_Concepts_ManagementFileContact,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<ApiGen_Concepts_ManagementFileContact>(
    `/managementfiles/${managementFileId}/contacts`,
    contact,
  );

export const putManagementFileContactsApi = (
  managementFileId: number,
  contactId: number,
  contact: ApiGen_Concepts_ManagementFileContact,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_ManagementFileContact>(
    `/managementfiles/${managementFileId}/contacts/${contactId}`,
    contact,
  );

export const deleteManagementFileContactsApi = (managementFileId: number, contactId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(
    `/managementfiles/${managementFileId}/contacts/${contactId}`,
  );
