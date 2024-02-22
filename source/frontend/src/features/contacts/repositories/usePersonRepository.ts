import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';

/**
 * hook that interacts with the Person API.
 */
export const usePersonRepository = () => {
  const { getPersonConcept } = useApiContacts();

  const getPersonDetail = useApiRequestWrapper<
    (personId: number) => Promise<AxiosResponse<ApiGen_Concepts_Person, any>>
  >({
    requestFunction: useCallback(
      async (personId: number) => await getPersonConcept(personId),
      [getPersonConcept],
    ),
    requestName: 'getPerson',
  });

  return useMemo(
    () => ({
      getPersonDetail,
    }),
    [getPersonDetail],
  );
};
