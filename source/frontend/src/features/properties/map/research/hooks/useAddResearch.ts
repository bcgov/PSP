import { AxiosResponse } from 'axios';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { useApiResearchFile } from 'hooks/pims-api/useApiResearchFile';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import { UserOverrideCode } from 'models/api/UserOverrideCode';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

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
