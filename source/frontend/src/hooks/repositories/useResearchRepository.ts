import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { useApiResearchFile } from '../pims-api/useApiResearchFile';

/**
 * hook that interacts with the Research API.
 */
export const useResearchRepository = () => {
  const { getLastUpdatedByApi } = useApiResearchFile();

  const getLastUpdatedBy = useApiRequestWrapper<
    (researchFileId: number) => Promise<AxiosResponse<Api_LastUpdatedBy, any>>
  >({
    requestFunction: useCallback(
      async (researchFileId: number) => await getLastUpdatedByApi(researchFileId),
      [getLastUpdatedByApi],
    ),
    requestName: 'getLastUpdatedBy',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(
      'Failed to retreive last-updated-by information for research file.',
    ),
  });

  return useMemo(
    () => ({
      getLastUpdatedBy: getLastUpdatedBy,
    }),
    [getLastUpdatedBy],
  );
};
