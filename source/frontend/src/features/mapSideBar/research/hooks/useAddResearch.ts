import { AxiosResponse } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiResearchFile } from '@/hooks/pims-api/useApiResearchFile';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_ResearchFile } from '@/models/api/ResearchFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

/**
 * hook that adds a research file.
 */
export const useAddResearch = () => {
  const { postResearchFile } = useApiResearchFile();

  const { execute } = useApiRequestWrapper<
    (...args: any[]) => Promise<AxiosResponse<Api_ResearchFile, any>>
  >({
    requestFunction: useCallback(
      async (researchFile: Api_ResearchFile, userOverrideCodes: UserOverrideCode[]) =>
        await postResearchFile(researchFile, userOverrideCodes),
      [postResearchFile],
    ),
    requestName: 'AddResearchFile',
    onSuccess: useCallback(() => toast.success('Research File saved'), []),
    throwError: true,
    skipErrorLogCodes: [409],
  });

  return { addResearchFile: execute };
};
