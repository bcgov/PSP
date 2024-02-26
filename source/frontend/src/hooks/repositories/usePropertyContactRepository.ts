import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_PropertyContact } from '@/models/api/generated/ApiGen_Concepts_PropertyContact';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import {
  deletePropertyContactsApi,
  getPropertyContactApi,
  getPropertyContactsApi,
  postPropertyContactsApi,
  putPropertyContactsApi,
} from '../pims-api/useApiPropertyContacts';

/**
 * hook that interacts with the property contacts API.
 */
export const usePropertyContactRepository = () => {
  const getPropertyContacts = useApiRequestWrapper<
    (propertyId: number) => Promise<AxiosResponse<ApiGen_Concepts_PropertyContact[], any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number) => await getPropertyContactsApi(propertyId),
      [],
    ),
    requestName: 'getPropertyContacts',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load property contacts.'),
  });

  const getPropertyContact = useApiRequestWrapper<
    (
      propertyId: number,
      contactId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_PropertyContact, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, contactId: number) =>
        await getPropertyContactApi(propertyId, contactId),
      [],
    ),
    requestName: 'getPropertyContact',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load property contacts.'),
  });

  const updatePropertyContact = useApiRequestWrapper<
    (
      propertyId: number,
      contactId: number,
      contact: ApiGen_Concepts_PropertyContact,
    ) => Promise<AxiosResponse<ApiGen_Concepts_PropertyContact, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, contactId: number, contact: ApiGen_Concepts_PropertyContact) =>
        await putPropertyContactsApi(propertyId, contactId, contact),
      [],
    ),
    requestName: 'updatePropertyContact',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to update property contact.'),
  });

  const createPropertyContact = useApiRequestWrapper<
    (
      propertyId: number,
      contact: ApiGen_Concepts_PropertyContact,
    ) => Promise<AxiosResponse<ApiGen_Concepts_PropertyContact, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, contact: ApiGen_Concepts_PropertyContact) =>
        await postPropertyContactsApi(propertyId, contact),
      [],
    ),
    requestName: 'updatePropertyContact',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to update property contact.'),
  });

  const deletePropertyContact = useApiRequestWrapper<
    (propertyId: number, contactId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, contactId: number) =>
        await deletePropertyContactsApi(propertyId, contactId),
      [],
    ),
    requestName: 'deletePropertyContact',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(
      'Failed to delete property contact. Refresh the page to try again.',
    ),
  });

  return useMemo(
    () => ({
      getPropertyContacts: getPropertyContacts,
      getPropertyContact: getPropertyContact,
      updatePropertyContact: updatePropertyContact,
      createPropertyContact: createPropertyContact,
      deletePropertyContact: deletePropertyContact,
    }),
    [
      getPropertyContacts,
      getPropertyContact,
      updatePropertyContact,
      createPropertyContact,
      deletePropertyContact,
    ],
  );
};
