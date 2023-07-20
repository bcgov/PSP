import { AxiosResponse } from 'axios';
import { useCallback } from 'react';

import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_Lease } from '@/models/api/Lease';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

/**
 * hook that updates a lease.
 * @param leaseId
 */
export const useUpdateLease = () => {
  const { putApiLease } = useApiLeases();

  const updateApiLease = useApiRequestWrapper<
    (
      lease: Api_Lease,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<Api_Lease, any>>
  >({
    requestFunction: useCallback(
      async (lease: Api_Lease, userOverrideCodes: UserOverrideCode[] = []) =>
        await putApiLease(lease, userOverrideCodes),
      [putApiLease],
    ),
    requestName: 'updateLease',
    throwError: true,
    skipErrorLogCodes: [409],
  });

  return { updateApiLease };
};
