import { AxiosError, AxiosResponse } from 'axios';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { IApiError } from 'interfaces/IApiError';
import { Api_FileDownload } from 'models/api/DocumentStorage';
import { ExternalResult } from 'models/api/ExternalResult';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiDocumentGeneration } from '../../../hooks/pims-api/useApiDocumentGeneration';
import { DocumentGenerationRequest } from '../../../models/api/DocumentGenerationRequest';

/**
 * repository for document generation
 */
export const useDocumentGenerationRepository = () => {
  const { generateDocumentDownloadWrapped: generate } = useApiDocumentGeneration();

  // Provides functionality to download a template of a specific type using uploaded json
  const {
    execute: generateDocumentDownloadWrappedRequest,
    response: generateDocumentDownloadWrappedResponse,
    loading: generateDocumentDownloadWrappedLoading,
  } = useApiRequestWrapper<
    (
      generateRequest: DocumentGenerationRequest,
    ) => Promise<AxiosResponse<ExternalResult<Api_FileDownload>, any>>
  >({
    requestFunction: useCallback(
      async (generateRequest: DocumentGenerationRequest) => await generate(generateRequest),
      [generate],
    ),
    requestName: 'GenerateDocumentDownloadWrapped',
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      }
    }, []),
  });

  return {
    generateDocumentDownloadWrappedRequest,
    generateDocumentDownloadWrappedResponse,
    generateDocumentDownloadWrappedLoading,
  };
};
