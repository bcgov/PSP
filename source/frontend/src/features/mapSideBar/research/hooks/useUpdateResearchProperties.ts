import { AxiosResponse } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiResearchFile } from '@/hooks/pims-api/useApiResearchFile';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_ResearchFile } from '@/models/api/ResearchFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

/**
 * hook that updates a research file.
 */
export const useUpdateResearchProperties = () => {
  const { putResearchFileProperties } = useApiResearchFile();

  const { execute } = useApiRequestWrapper<
    (
      researchFile: Api_ResearchFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<Api_ResearchFile, any>>
  >({
    requestFunction: useCallback(
      async (researchFile: Api_ResearchFile, userOverrideCodes: UserOverrideCode[]) =>
        await putResearchFileProperties(researchFile, userOverrideCodes),
      [putResearchFileProperties],
    ),
    requestName: 'UpdateResearchFileProperties',
    onSuccess: useCallback(() => toast.success('Research File Properties updated'), []),
    throwError: true,
    skipErrorLogCodes: [409],
  });

  return { updateResearchFileProperties: execute };
};
