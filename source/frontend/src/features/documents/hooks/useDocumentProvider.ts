import { AxiosError, AxiosResponse } from 'axios';
import { useApiDocuments } from 'hooks/pims-api/useApiDocuments';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { IApiError } from 'interfaces/IApiError';
import {
  Api_DocumentType,
  Api_DocumentUpdateRequest,
  Api_DocumentUpdateResponse,
} from 'models/api/Document';
import {
  Api_Storage_DocumentMetadata,
  Api_Storage_DocumentTypeMetadataType,
  DocumentQueryResult,
} from 'models/api/DocumentStorage';
import { ExternalResult } from 'models/api/ExternalResult';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

/**
 * hook that retrieves document information.
 */
export const useDocumentProvider = () => {
  const {
    getDocumentMetada,
    getDocumentTypeMetadata,
    getDocumentTypes,
    downloadDocumentFileApiCall,
    downloadDocumentFileLatestApiCall,
    updateDocumentMetadataApiCall,
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
        async (mayanDocumentId: number) => await getDocumentMetada(mayanDocumentId),
        [getDocumentMetada],
      ),
      requestName: 'retrieveDocumentMetadata',
      onSuccess: useCallback(() => toast.success('Document Metadata retrieved'), []),
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
        async (mayanDocumentId: number) => await getDocumentTypeMetadata(mayanDocumentId),
        [getDocumentTypeMetadata],
      ),
      requestName: 'retrieveDocumentTypeMetadata',
      onSuccess: useCallback(() => toast.success('Document Metadata retrieved'), []),
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        } else {
          toast.error('Retrieve document type metadata error. Check responses and try again.');
        }
      }, []),
    });

  // Provides functionality to retrieve document types
  const { execute: retrieveDocumentTypes, loading: retrieveDocumentTypesLoading } =
    useApiRequestWrapper<() => Promise<AxiosResponse<Api_DocumentType[], any>>>({
      requestFunction: useCallback(async () => await getDocumentTypes(), [getDocumentTypes]),
      requestName: 'retrieveDocumentTypes',
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        } else {
          toast.error('Retrieve document types error. Check responses and try again.');
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
    onSuccess: useCallback(() => {
      toast.success('Updated document metadata');
    }, []),
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Update document error. Check responses and try again.');
      }
    }, []),
  });

  // Provides functionality for download a document file
  const { execute: downloadDocumentFile, loading: downloadDocumentFileLoading } =
    useApiRequestWrapper<
      (
        mayanDocumentId: number,
        mayanFileId: number,
      ) => Promise<
        AxiosResponse<AxiosResponse<string | ArrayBuffer | ArrayBufferView | Blob, any>, any>
      >
    >({
      rawResponse: true,
      requestFunction: useCallback(
        async (mayanDocumentId: number, mayanFileId: number) =>
          await downloadDocumentFileApiCall(mayanDocumentId, mayanFileId),
        [downloadDocumentFileApiCall],
      ),
      requestName: 'DownloadDocumentFile',
      onSuccess: useCallback(() => toast.success('Download Downloaded'), []),
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        } else {
          toast.error('Download document error. Check responses and try again.');
        }
      }, []),
    });

  // Provides functionality for download the latest file for a document
  const {
    execute: downloadDocumentFileLatest,
    response: downloadDocumentFileLatestResponse,
    loading: downloadDocumentFileLatestLoading,
  } = useApiRequestWrapper<
    (
      documendId: number,
    ) => Promise<
      AxiosResponse<AxiosResponse<string | ArrayBuffer | ArrayBufferView | Blob, any>, any>
    >
  >({
    rawResponse: true,
    requestFunction: useCallback(
      async (mayanDocumentId: number) => await downloadDocumentFileLatestApiCall(mayanDocumentId),
      [downloadDocumentFileLatestApiCall],
    ),
    requestName: 'DownloadDocumentFileLatest',
    onSuccess: useCallback(() => toast.success('Download Downloaded Latest'), []),
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Download document latest error. Check responses and try again.');
      }
    }, []),
  });

  return {
    retrieveDocumentMetadata,
    retrieveDocumentMetadataLoading,
    downloadDocumentFile,
    downloadDocumentFileLoading,
    downloadDocumentFileLatest,
    downloadDocumentFileLatestResponse,
    downloadDocumentFileLatestLoading,
    retrieveDocumentTypeMetadata,
    retrieveDocumentTypeMetadataLoading,
    retrieveDocumentTypes,
    retrieveDocumentTypesLoading,
    updateDocument,
    updateDocumentLoading,
  };
};
