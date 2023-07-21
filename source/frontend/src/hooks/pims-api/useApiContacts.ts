import queryString from 'query-string';
import React from 'react';

import { IContactFilter } from '@/components/contact/ContactManagerView/IContactFilter';
import { IContactSearchResult, IPagedItems } from '@/interfaces';
import { IEditableOrganization, IEditablePerson } from '@/interfaces/editable-contact';
import { IContact } from '@/interfaces/IContact';
import { Api_Organization } from '@/models/api/Organization';
import { Api_Person } from '@/models/api/Person';

import { IPaginateRequest } from './interfaces/IPaginateRequest';
import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the contacts endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiContacts = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getContacts: (params: IPaginateContacts | null) =>
        api.get<IPagedItems<IContactSearchResult>>(
          `/contacts/search?${params ? queryString.stringify(params) : ''}`,
        ),
      // This endpoint returns contact data in read-only form, including formatting some fields; e.g. full name = first + middle + last
      getContact: (id: string) => api.get<IContact>(`/contacts/${id}`),
      // This is different than getContact above. This endpoints returns person data that can be edited in a form
      getPerson: (id: number) => api.get<IEditablePerson>(`/persons/${id}`),
      getPersonConcept: (id: number) => api.get<Api_Person>(`/persons/concept/${id}`),
      postPerson: (person: IEditablePerson, userOverride: boolean) =>
        api.post<IEditablePerson>(`/persons?userOverride=${userOverride}`, person),
      putPerson: (person: IEditablePerson) =>
        api.put<IEditablePerson>(`/persons/${person.id}`, person),
      getOrganization: (id: number) => api.get<IEditableOrganization>(`/organizations/${id}`),
      getOrganizationConcept: (id: number) =>
        api.get<Api_Organization>(`/organizations/concept/${id}`),
      postOrganization: (organization: IEditableOrganization, userOverride: boolean) =>
        api.post<IEditableOrganization>(
          `/organizations?userOverride=${userOverride}`,
          organization,
        ),
      putOrganization: (organization: IEditableOrganization) =>
        api.put<IEditableOrganization>(`/organizations/${organization.id}`, organization),
    }),
    [api],
  );
};

export type IPaginateContacts = IPaginateRequest<IContactFilter>;
