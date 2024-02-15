import { AxiosError, AxiosResponse } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiDocumentGeneration } from '@/hooks/pims-api/useApiDocumentGeneration';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { DocumentGenerationRequest } from '@/models/api/DocumentGenerationRequest';
import { ApiGen_Requests_ExternalResponse } from '@/models/api/generated/ApiGen_Requests_ExternalResponse';
import { ApiGen_Requests_FileDownloadResponse } from '@/models/api/generated/ApiGen_Requests_FileDownloadResponse';

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
    ) => Promise<
      AxiosResponse<ApiGen_Requests_ExternalResponse<ApiGen_Requests_FileDownloadResponse>, any>
    >
  >({
    requestFunction: useCallback(
      async (generateRequest: DocumentGenerationRequest) => await generate(generateRequest),
      [generate],
    ),
    requestName: 'GenerateDocumentDownloadWrapped',
    throwError: true,
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else if (axiosError?.response?.status === 404) {
        toast.error(
          'The requested document template was not found. This indicates that the PIMS administrator needs to add a template to the system for this document type. Please contact your system administrator, and ask them to add a template for this document type.',
          { autoClose: false },
        );
      }
    }, []),
  });

  return {
    generateDocumentDownloadWrappedRequest,
    generateDocumentDownloadWrappedResponse,
    generateDocumentDownloadWrappedLoading,
  };
};
