import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';

/**
 * hook that interacts with the Organization API.
 */
export const useOrganizationRepository = () => {
  const { getOrganizationConcept } = useApiContacts();

  const getOrganizationDetail = useApiRequestWrapper<
    (orgId: number) => Promise<AxiosResponse<ApiGen_Concepts_Organization, any>>
  >({
    requestFunction: useCallback(
      async (orgId: number) => await getOrganizationConcept(orgId),
      [getOrganizationConcept],
    ),
    requestName: 'getOrganization',
  });

  return useMemo(
    () => ({
      getOrganizationDetail,
    }),
    [getOrganizationDetail],
  );
};
