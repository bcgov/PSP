import { AxiosResponse } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { useAxiosErrorHandler } from '@/utils/axiosUtils';

import { useApiBctfaOwnership } from '../pims-api/useApiBctfaOwnership';

/**
 * repository for bcfta ownership
 */
export const useBctfaOwnershipRepository = () => {
  const { updateBctfaOwnership } = useApiBctfaOwnership();

  // Provides functionality to download a template of a specific type using uploaded json
  const updateBctfaOwnershipApi = useApiRequestWrapper<
    (file: File) => Promise<AxiosResponse<number[], any>>
  >({
    requestFunction: useCallback(
      async (file: File) => await updateBctfaOwnership(file),
      [updateBctfaOwnership],
    ),
    requestName: 'UpdateBctfaOwnershipApi',
    throwError: true,
    onSuccess: (pids: number[]) =>
      toast.info(`Successfully updated the ownership of ${pids.length} BCTFA properties.`),
    onError: useAxiosErrorHandler(
      'Failed to update BCTFA Ownership. Check that the file is valid and try uploading again',
    ),
  });

  return {
    updateBctfaOwnershipApi,
  };
};
