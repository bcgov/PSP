import queryString from 'query-string';
import React from 'react';

import { IContactFilter } from '@/components/contact/ContactManagerView/IContactFilter';
import { IContactSearchResult } from '@/interfaces';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';

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
        api.get<ApiGen_Base_Page<IContactSearchResult>>(
          `/contacts/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getPersonConcept: (id: number) => api.get<ApiGen_Concepts_Person>(`/persons/concept/${id}`),
      postPerson: (person: ApiGen_Concepts_Person, userOverride: boolean) =>
        api.post<ApiGen_Concepts_Person>(`/persons?userOverride=${userOverride}`, person),
      putPerson: (person: ApiGen_Concepts_Person) =>
        api.put<ApiGen_Concepts_Person>(`/persons/${person.id}`, person),
      getOrganization: (id: number) =>
        api.get<ApiGen_Concepts_Organization>(`/organizations/${id}`),
      getOrganizationConcept: (id: number) =>
        api.get<ApiGen_Concepts_Organization>(`/organizations/concept/${id}`),
      postOrganization: (organization: ApiGen_Concepts_Organization, userOverride: boolean) =>
        api.post<ApiGen_Concepts_Organization>(
          `/organizations?userOverride=${userOverride}`,
          organization,
        ),
      putOrganization: (organization: ApiGen_Concepts_Organization) =>
        api.put<ApiGen_Concepts_Organization>(`/organizations/${organization.id}`, organization),
    }),
    [api],
  );
};

export type IPaginateContacts = IPaginateRequest<IContactFilter>;
