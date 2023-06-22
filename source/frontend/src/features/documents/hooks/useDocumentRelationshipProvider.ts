import { AxiosError, AxiosResponse } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import { useApiDocuments } from '@/hooks/pims-api/useApiDocuments';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import {
  Api_DocumentRelationship,
  Api_DocumentUploadRequest,
  Api_DocumentUploadResponse,
} from '@/models/api/Document';

/**
 * hook that retrieves document relationship information.
 */
export const useDocumentRelationshipProvider = () => {
  const {
    getDocumentRelationshipApiCall,
    deleteDocumentRelationshipApiCall,
    uploadDocumentRelationshipApiCall,
  } = useApiDocuments();

  // Provides functionality to retrieve document relationship
  const { execute: retrieveDocumentRelationship, loading: retrieveDocumentRelationshipLoading } =
    useApiRequestWrapper<
      (
        relationshipType: DocumentRelationshipType,
        parentId: string,
      ) => Promise<AxiosResponse<Api_DocumentRelationship[], any>>
    >({
      requestFunction: useCallback(
        async (relationshipType: DocumentRelationshipType, parentId: string) =>
          await getDocumentRelationshipApiCall(relationshipType, parentId),
        [getDocumentRelationshipApiCall],
      ),
      requestName: 'retrieveDocumentRelationship',
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        } else {
          toast.error('Retrieve document relationship error. Check responses and try again.');
        }
      }, []),
    });

  // Provides functionality for deleting a document relationship
  const { execute: deleteDocumentRelationship, loading: deleteDocumentRelationshipLoading } =
    useApiRequestWrapper<
      (
        relationshipType: DocumentRelationshipType,
        documentRelationship: Api_DocumentRelationship,
      ) => Promise<AxiosResponse<boolean, any>>
    >({
      requestFunction: useCallback(
        async (
          relationshipType: DocumentRelationshipType,
          documentRelationship: Api_DocumentRelationship,
        ) => await deleteDocumentRelationshipApiCall(relationshipType, documentRelationship),
        [deleteDocumentRelationshipApiCall],
      ),
      requestName: 'deleteDocumentRelationship',
      onSuccess: useCallback((response?: boolean) => {
        if (response !== undefined && response) {
          toast.success('Deleted document relationship');
        } else {
          toast.error('Delete document relationship error. Check responses and try again.');
        }
      }, []),
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        } else {
          toast.error('Delete document relationship error. Check responses and try again.');
        }
      }, []),
    });

  // Provides functionality for uploading a document for a given relationship
  const { execute: uploadDocument, loading: uploadDocumentLoading } = useApiRequestWrapper<
    (
      relationshipType: DocumentRelationshipType,
      parentId: string,
      uploadRequest: Api_DocumentUploadRequest,
    ) => Promise<AxiosResponse<Api_DocumentUploadResponse, any>>
  >({
    requestFunction: useCallback(
      async (
        relationshipType: DocumentRelationshipType,
        parentId: string,
        uploadRequest: Api_DocumentUploadRequest,
      ) => await uploadDocumentRelationshipApiCall(relationshipType, parentId, uploadRequest),
      [uploadDocumentRelationshipApiCall],
    ),
    requestName: 'uploadDocumentRelationshipApiCall',
    onSuccess: useCallback(() => {
      toast.success('Uploaded document relationship');
    }, []),
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Upload document relationship error. Check responses and try again.');
      }
    }, []),
  });

  return {
    retrieveDocumentRelationship,
    retrieveDocumentRelationshipLoading,
    deleteDocumentRelationship,
    deleteDocumentRelationshipLoading,
    uploadDocument,
    uploadDocumentLoading,
  };
};
