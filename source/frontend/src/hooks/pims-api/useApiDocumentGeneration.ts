import React from 'react';

import { Api_FileDownload } from '@/models/api/DocumentStorage';
import { ExternalResult } from '@/models/api/ExternalResult';

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
        api.post<ExternalResult<Api_FileDownload>>(
          `/documentGeneration/template/generate/download-wrapped`,
          request,
        ),
    }),
    [api],
  );
};
