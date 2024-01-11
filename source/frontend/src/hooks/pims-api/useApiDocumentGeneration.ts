import React from 'react';

import { ApiGen_Requests_ExternalResponse } from '@/models/api/generated/ApiGen_Requests_ExternalResponse';
import { ApiGen_Requests_FileDownloadResponse } from '@/models/api/generated/ApiGen_Requests_FileDownloadResponse';

import { DocumentGenerationRequest } from './../../models/api/DocumentGenerationRequest';
import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the document generation endpoints
 * @returns Object containing functions to make requests to the PIMS API.
 */

export const useApiDocumentGeneration = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      generateDocumentDownloadWrapped: (request: DocumentGenerationRequest) =>
        api.post<ApiGen_Requests_ExternalResponse<ApiGen_Requests_FileDownloadResponse>>(
          `/documentGeneration/template/generate/download-wrapped`,
          request,
        ),
    }),
    [api],
  );
};
