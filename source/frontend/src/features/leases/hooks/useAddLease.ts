import { AxiosResponse } from 'axios';
import { useCallback } from 'react';

import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

/**
 * hook that adds a lease.
 */
export const useAddLease = () => {
  const { postLease } = useApiLeases();

  const addLease = useApiRequestWrapper<
    (
      lease: ApiGen_Concepts_Lease,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_Lease, any>>
  >({
    requestFunction: useCallback(
      async (lease: ApiGen_Concepts_Lease, userOverrideCodes: UserOverrideCode[] = []) =>
        await postLease(lease, userOverrideCodes),
      [postLease],
    ),
    requestName: 'addLease',
    throwError: true,
    skipErrorLogCodes: [409],
  });

  return { addLease };
};
