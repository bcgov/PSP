import { AxiosError, AxiosResponse } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import { useApiDocuments } from '@/hooks/pims-api/useApiDocuments';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import {
  Api_DocumentType,
  Api_DocumentUpdateRequest,
  Api_DocumentUpdateResponse,
} from '@/models/api/Document';
import {
  Api_FileDownload,
  Api_Storage_DocumentDetail,
  Api_Storage_DocumentMetadata,
  Api_Storage_DocumentTypeMetadataType,
  DocumentQueryResult,
} from '@/models/api/DocumentStorage';
import { ExternalResult } from '@/models/api/ExternalResult';

/**
 * hook that retrieves document information.
 */
export const useDocumentProvider = () => {
  const {
    getDocumentMetadataApiCall,
    getDocumentRelationshipTypesApiCall,
    getDocumentTypeMetadataApiCall,
    getDocumentDetailApiCall,
    downloadWrappedDocumentFileApiCall,
    downloadWrappedDocumentFileLatestApiCall,
    updateDocumentMetadataApiCall,
    getDocumentTypesApiCall,
  } = useApiDocuments();

  // Provides functionality to retrieve document metadata information
  const { execute: retrieveDocumentMetadata, loading: retrieveDocumentMetadataLoading } =
    useApiRequestWrapper<
      (
        mayanDocumentId: number,
      ) => Promise<
        AxiosResponse<ExternalResult<DocumentQueryResult<Api_Storage_DocumentMetadata>>, any>
      >
    >({
      requestFunction: useCallback(
        async (mayanDocumentId: number) => await getDocumentMetadataApiCall(mayanDocumentId),
        [getDocumentMetadataApiCall],
      ),
      requestName: 'retrieveDocumentMetadata',
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        } else {
          toast.error('Retrieve document metadata file error. Check responses and try again.');
        }
      }, []),
    });

  // Provides functionality to retrieve document type metadata information
  const { execute: retrieveDocumentTypeMetadata, loading: retrieveDocumentTypeMetadataLoading } =
    useApiRequestWrapper<
      (
        mayanDocumentId: number,
      ) => Promise<
        AxiosResponse<
          ExternalResult<DocumentQueryResult<Api_Storage_DocumentTypeMetadataType>>,
          any
        >
      >
    >({
      requestFunction: useCallback(
        async (mayanDocumentId: number) => await getDocumentTypeMetadataApiCall(mayanDocumentId),
        [getDocumentTypeMetadataApiCall],
      ),
      requestName: 'retrieveDocumentTypeMetadata',
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        } else {
          toast.error('Retrieve document type metadata error. Check responses and try again.');
        }
      }, []),
    });

  // Provides functionality to retrieve document types
  const { execute: getDocumentTypes, loading: getDocumentTypesLoading } = useApiRequestWrapper<
    () => Promise<AxiosResponse<Api_DocumentType[], any>>
  >({
    requestFunction: useCallback(
      async () => await getDocumentTypesApiCall(),
      [getDocumentTypesApiCall],
    ),
    requestName: 'getDocumentTypes',
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Retrieve document types error. Check responses and try again.');
      }
    }, []),
  });

  // Provides functionality to retrieve document types by relationships
  const { execute: getDocumentRelationshipTypes, loading: getDocumentRelationshipTypesLoading } =
    useApiRequestWrapper<
      (
        relationshipType: DocumentRelationshipType,
      ) => Promise<AxiosResponse<Api_DocumentType[], any>>
    >({
      requestFunction: useCallback(
        async (relationshipType: DocumentRelationshipType) =>
          await getDocumentRelationshipTypesApiCall(relationshipType),
        [getDocumentRelationshipTypesApiCall],
      ),
      requestName: 'getDocumentRelationshipTypes',
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        } else {
          toast.error('Retrieve document relationship types error. Check responses and try again.');
        }
      }, []),
    });

  // Provides functionality for uploading a document metadata
  const { execute: updateDocument, loading: updateDocumentLoading } = useApiRequestWrapper<
    (
      documentId: number,
      updateRequest: Api_DocumentUpdateRequest,
    ) => Promise<AxiosResponse<Api_DocumentUpdateResponse, any>>
  >({
    requestFunction: useCallback(
      async (documentId: number, updateRequest: Api_DocumentUpdateRequest) =>
        await updateDocumentMetadataApiCall(documentId, updateRequest),
      [updateDocumentMetadataApiCall],
    ),
    requestName: 'updateDocumentMetadataApiCall',
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Update document error. Check responses and try again.');
      }
    }, []),
  });

  // Provides functionality to view a document's details
  const { execute: retrieveDocumentDetail, loading: retrieveDocumentDetailLoading } =
    useApiRequestWrapper<
      (
        mayanDocumentId: number,
      ) => Promise<AxiosResponse<ExternalResult<Api_Storage_DocumentDetail>, any>>
    >({
      requestFunction: useCallback(
        async (mayanDocumentId: number) => await getDocumentDetailApiCall(mayanDocumentId),
        [getDocumentDetailApiCall],
      ),
      requestName: 'DownloadDocumentDetail',
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        }
      }, []),
    });

  // Provides functionality for download a document file
  const { execute: downloadWrappedDocumentFile, loading: downloadWrappedDocumentFileLoading } =
    useApiRequestWrapper<
      (
        mayanDocumentId: number,
        mayanFileId: number,
      ) => Promise<AxiosResponse<Api_FileDownload, any>>
    >({
      requestFunction: useCallback(
        async (mayanDocumentId: number, mayanFileId: number) =>
          await downloadWrappedDocumentFileApiCall(mayanDocumentId, mayanFileId),
        [downloadWrappedDocumentFileApiCall],
      ),
      requestName: 'DownloadDocumentFile',
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        }
      }, []),
    });

  // Provides functionality for download the latest file for a document
  const {
    execute: downloadWrappedDocumentFileLatest,
    response: downloadWrappedDocumentFileLatestResponse,
    loading: downloadWrappedDocumentFileLatestLoading,
  } = useApiRequestWrapper<(documendId: number) => Promise<AxiosResponse<Api_FileDownload, any>>>({
    requestFunction: useCallback(
      async (mayanDocumentId: number) =>
        await downloadWrappedDocumentFileLatestApiCall(mayanDocumentId),
      [downloadWrappedDocumentFileLatestApiCall],
    ),
    requestName: 'DownloadDocumentFileLatest',
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      }
    }, []),
  });

  return {
    retrieveDocumentMetadata,
    retrieveDocumentMetadataLoading,
    downloadWrappedDocumentFile,
    downloadWrappedDocumentFileLoading,
    downloadWrappedDocumentFileLatest,
    downloadWrappedDocumentFileLatestResponse,
    downloadWrappedDocumentFileLatestLoading,
    retrieveDocumentTypeMetadata,
    retrieveDocumentTypeMetadataLoading,
    getDocumentTypes,
    getDocumentTypesLoading,
    getDocumentRelationshipTypes,
    getDocumentRelationshipTypesLoading,
    retrieveDocumentDetail,
    retrieveDocumentDetailLoading,
    updateDocument,
    updateDocumentLoading,
  };
};
