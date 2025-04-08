import { AxiosResponse } from 'axios';
import { useCallback } from 'react';

import { useApiResearchFile } from '@/hooks/pims-api/useApiResearchFile';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { useAxiosSuccessHandler } from '@/utils/axiosUtils';

/**
 * hook that updates a research file.
 */
export const useUpdateResearchProperties = () => {
  const { putResearchFileProperties } = useApiResearchFile();

  const { execute } = useApiRequestWrapper<
    (
      researchFile: ApiGen_Concepts_ResearchFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_ResearchFile, any>>
  >({
    requestFunction: useCallback(
      async (researchFile: ApiGen_Concepts_ResearchFile, userOverrideCodes: UserOverrideCode[]) =>
        await putResearchFileProperties(researchFile, userOverrideCodes),
      [putResearchFileProperties],
    ),
    requestName: 'UpdateResearchFileProperties',
    onSuccess: useAxiosSuccessHandler(),
    throwError: true,
  });

  return { updateResearchFileProperties: execute };
};
