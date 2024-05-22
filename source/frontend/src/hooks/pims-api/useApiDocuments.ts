import React from 'react';

import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { ApiGen_Mayan_DocumentDetail } from '@/models/api/generated/ApiGen_Mayan_DocumentDetail';
import { ApiGen_Mayan_DocumentMetadata } from '@/models/api/generated/ApiGen_Mayan_DocumentMetadata';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { ApiGen_Mayan_QueryResponse } from '@/models/api/generated/ApiGen_Mayan_QueryResponse';
import { ApiGen_Requests_DocumentUpdateRequest } from '@/models/api/generated/ApiGen_Requests_DocumentUpdateRequest';
import { ApiGen_Requests_DocumentUpdateResponse } from '@/models/api/generated/ApiGen_Requests_DocumentUpdateResponse';
import { ApiGen_Requests_DocumentUploadRequest } from '@/models/api/generated/ApiGen_Requests_DocumentUploadRequest';
import { ApiGen_Requests_DocumentUploadResponse } from '@/models/api/generated/ApiGen_Requests_DocumentUploadResponse';
import { ApiGen_Requests_ExternalResponse } from '@/models/api/generated/ApiGen_Requests_ExternalResponse';
import { ApiGen_Requests_FileDownloadResponse } from '@/models/api/generated/ApiGen_Requests_FileDownloadResponse';

import { ApiGen_Mayan_FilePage } from './../../models/api/generated/ApiGen_Mayan_FilePage';
import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the note endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */

export const useApiDocuments = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getDocumentTypesApiCall: () => api.get<ApiGen_Concepts_DocumentType[]>(`/documents/types`),

      getDocumentRelationshipTypesApiCall: (
        relationshipType: ApiGen_CodeTypes_DocumentRelationType,
      ) => api.get<ApiGen_Concepts_DocumentType[]>(`/documents/categories/${relationshipType}`),

      getDocumentTypeMetadataApiCall: (mayanDocumentTypeId: number) =>
        api.get<
          ApiGen_Requests_ExternalResponse<
            ApiGen_Mayan_QueryResponse<ApiGen_Mayan_DocumentTypeMetadataType>
          >
        >(`/documents/storage/types/${mayanDocumentTypeId}/metadata`),

      getDocumentRelationshipApiCall: (
        relationshipType: ApiGen_CodeTypes_DocumentRelationType,
        parentId: string,
      ) =>
        api.get<ApiGen_Concepts_DocumentRelationship[]>(
          `/documents/${relationshipType}/${parentId}`,
        ),

      deleteDocumentRelationshipApiCall: (
        relationshipType: ApiGen_CodeTypes_DocumentRelationType,
        documentRelationship: ApiGen_Concepts_DocumentRelationship,
      ) => api.delete<boolean>(`/documents/${relationshipType}`, { data: documentRelationship }),

      getDocumentMetadataApiCall: (mayanDocumentId: number) =>
        api.get<
          ApiGen_Requests_ExternalResponse<
            ApiGen_Mayan_QueryResponse<ApiGen_Mayan_DocumentMetadata>
          >
        >(`/documents/storage/${mayanDocumentId}/metadata`),

      getDocumentDetailApiCall: (mayanDocumentId: number) =>
        api.get<ApiGen_Requests_ExternalResponse<ApiGen_Mayan_DocumentDetail>>(
          `/documents/storage/${mayanDocumentId}/detail`,
        ),

      downloadWrappedDocumentFileApiCall: (mayanDocumentId: number, mayanFileId: number) =>
        api.get<ApiGen_Requests_FileDownloadResponse>(
          `/documents/storage/${mayanDocumentId}/files/${mayanFileId}/download-wrapped`,
        ),

      downloadWrappedDocumentFileLatestApiCall: (mayanDocumentId: number) =>
        api.get<ApiGen_Requests_FileDownloadResponse>(
          `/documents/storage/${mayanDocumentId}/download-wrapped`,
        ),

      uploadDocumentRelationshipApiCall: (
        relationshipType: ApiGen_CodeTypes_DocumentRelationType,
        parentId: string,
        uploadRequest: ApiGen_Requests_DocumentUploadRequest,
      ) => {
        const formData = new FormData();
        formData.append('file', uploadRequest.file);
        formData.append('documentTypeMayanId', uploadRequest.documentTypeMayanId?.toString() || '');
        formData.append('documentTypeId', uploadRequest.documentTypeId?.toString() || '');
        formData.append('documentStatusCode', uploadRequest.documentStatusCode || '');

        uploadRequest.documentMetadata?.forEach((metadata, index) => {
          formData.append(
            'DocumentMetadata[' + index + '].MetadataTypeId',
            metadata.metadataTypeId.toString(),
          );
          formData.append('DocumentMetadata[' + index + '].Value', metadata.value ?? '');
          index++;
        });

        return api.post<ApiGen_Requests_DocumentUploadResponse>(
          `/documents/upload/${relationshipType}/${parentId}`,
          formData,
        );
      },
      updateDocumentMetadataApiCall: (
        documentId: number,
        updateRequest: ApiGen_Requests_DocumentUpdateRequest,
      ) => {
        return api.put<ApiGen_Requests_DocumentUpdateResponse>(
          `/documents/${documentId}/metadata`,
          updateRequest,
        );
      },

      downloadDocumentFilePageImageApiCall: (
        mayanDocumentId: number,
        mayanDocumentFileId: number,
        mayanDocumentFilePageId: number,
      ) =>
        api.get<Blob>(
          `/documents/storage/${mayanDocumentId}/file/${mayanDocumentFileId}/pages/${mayanDocumentFilePageId}`,
          { responseType: 'blob' },
        ),

      getDocumentFilePageListApiCall: (mayanDocumentId: number, mayanDocumentFileId: number) =>
        api.get<ApiGen_Mayan_FilePage[]>(
          `/documents/storage/${mayanDocumentId}/file/${mayanDocumentFileId}/pages/`,
        ),
    }),
    [api],
  );
};
