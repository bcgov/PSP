import { Api_DocumentType } from 'models/api/Document';
import {
  DocumentQueryResult,
  FileDownload,
  Mayan_DocumentMetadata,
} from 'models/api/DocumentManagement';
import { ExternalResult } from 'models/api/ExternalResult';
import React from 'react';

import { useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the note endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */

export const useApiDocuments = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getDocumentTypes: () => api.get<Api_DocumentType[]>(`/documents/document-types`),
      getDocumentMetada: (mayanDocumentId: number) =>
        api.get<ExternalResult<DocumentQueryResult<Mayan_DocumentMetadata>>>(
          `/documents/${mayanDocumentId}/metadata`,
        ),
      downloadDocumentFileApiCall: (mayanDocumentId: number, mayanFileId: number) =>
        api.get<ExternalResult<FileDownload>>(
          `/documents/${mayanDocumentId}/files/${mayanFileId}/download`,
        ),
      downloadDocumentFileLatestApiCall: (mayanDocumentId: number) =>
        api.get<ExternalResult<FileDownload>>(`/documents/${mayanDocumentId}/download`),
      deleteDocumentActivityApiCall: (documentId: number, activityId: number) =>
        api.delete<boolean>(`/documents/${documentId}/activity/${activityId}`),
    }),
    [api],
  );
};
