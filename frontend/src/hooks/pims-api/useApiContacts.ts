import { IPagedItems } from 'interfaces';
import queryString from 'query-string';
import React from 'react';

import { IContactFilter } from '../../features/contacts/interfaces';
import { IContactSearchResult } from './../../interfaces/IContactSearchResult';
import { IPaginateRequest, useAxiosApi } from '.';

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
    }),
    [api],
  );
};

export type IPaginateContacts = IPaginateRequest<IContactFilter>;
