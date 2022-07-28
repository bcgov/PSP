import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { Api_DocumentRelationship, Api_DocumentType } from 'models/api/Document';
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
      getDocumentTypes: () => api.get<Api_DocumentType[]>(`/documents/types`),

      getDocumentRelationship: (relationshipType: DocumentRelationshipType, parentId: number) =>
        api.get<Api_DocumentRelationship[]>(`/documents/${relationshipType}/${parentId}`),

      deleteDocumentRelationshipApiCall: (
        relationshipType: DocumentRelationshipType,
        documentRelationship: Api_DocumentRelationship,
      ) => api.delete<boolean>(`/documents/${relationshipType}`, { data: documentRelationship }),

      getDocumentMetada: (mayanDocumentId: number) =>
        api.get<ExternalResult<DocumentQueryResult<Mayan_DocumentMetadata>>>(
          `/documents/storage/${mayanDocumentId}/metadata`,
        ),

      downloadDocumentFileApiCall: (mayanDocumentId: number, mayanFileId: number) =>
        api.get<ExternalResult<FileDownload>>(
          `/documents/storage/${mayanDocumentId}/files/${mayanFileId}/download`,
        ),

      downloadDocumentFileLatestApiCall: (mayanDocumentId: number) =>
        api.get<ExternalResult<FileDownload>>(`/documents/storage/${mayanDocumentId}/download`),
    }),
    [api],
  );
};
