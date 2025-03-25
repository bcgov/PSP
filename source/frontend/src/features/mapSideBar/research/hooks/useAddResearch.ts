import { AxiosResponse } from 'axios';
import { useCallback } from 'react';

import { useApiResearchFile } from '@/hooks/pims-api/useApiResearchFile';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { useAxiosSuccessHandler } from '@/utils/axiosUtils';

/**
 * hook that adds a research file.
 */
export const useAddResearch = () => {
  const { postResearchFile } = useApiResearchFile();

  const { execute } = useApiRequestWrapper<
    (...args: any[]) => Promise<AxiosResponse<ApiGen_Concepts_ResearchFile, any>>
  >({
    requestFunction: useCallback(
      async (researchFile: ApiGen_Concepts_ResearchFile, userOverrideCodes: UserOverrideCode[]) =>
        await postResearchFile(researchFile, userOverrideCodes),
      [postResearchFile],
    ),
    requestName: 'AddResearchFile',
    onSuccess: useAxiosSuccessHandler(),
    throwError: true,
  });

  return { addResearchFile: execute };
};
