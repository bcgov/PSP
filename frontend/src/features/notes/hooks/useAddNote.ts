import { AxiosResponse } from 'axios';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { useApiResearchFile } from 'hooks/pims-api/useApiResearchFile';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import { useCallback } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

/**
 * hook that adds a note.
 */
export const useAddNote = () => {
  const { postResearchFile } = useApiResearchFile();

  const { execute } = useApiRequestWrapper<
    (...args: any[]) => Promise<AxiosResponse<Api_ResearchFile, any>>
  >({
    requestFunction: useCallback(
      async (researchFile: Api_ResearchFile) => await postResearchFile(researchFile),
      [postResearchFile],
    ),
    requestName: 'AddNote',
    onSuccess: useAxiosSuccessHandler('Note saved'),
    onError: useAxiosErrorHandler(),
  });

  return { addResearchFile: execute };
};
