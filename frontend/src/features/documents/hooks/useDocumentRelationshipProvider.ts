import { AxiosError, AxiosResponse } from 'axios';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { useApiDocuments } from 'hooks/pims-api/useApiDocuments';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { IApiError } from 'interfaces/IApiError';
import {
  Api_DocumentRelationship,
  Api_DocumentUpdateRequest,
  Api_UploadRequest,
  Api_UploadResponse,
} from 'models/api/Document';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

/**
 * hook that retrieves document relationship information.
 */
export const useDocumentRelationshipProvider = () => {
  const {
    getDocumentRelationship,
    deleteDocumentRelationshipApiCall,
    uploadDocumentRelationshipApiCall,
    updateDocumentMetadataApiCall,
  } = useApiDocuments();

  // Provides functionality to retrieve document relationship
  const {
    execute: retrieveDocumentRelationship,
    loading: retrieveDocumentRelationshipLoading,
  } = useApiRequestWrapper<
    (
      relationshipType: DocumentRelationshipType,
      parentId: number,
    ) => Promise<AxiosResponse<Api_DocumentRelationship[], any>>
  >({
    requestFunction: useCallback(
      async (relationshipType: DocumentRelationshipType, parentId: number) =>
        await getDocumentRelationship(relationshipType, parentId),
      [getDocumentRelationship],
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
  const {
    execute: deleteDocumentRelationship,
    loading: deleteDocumentRelationshipLoading,
  } = useApiRequestWrapper<
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
      parentId: number,
      uploadRequest: Api_UploadRequest,
    ) => Promise<AxiosResponse<Api_UploadResponse, any>>
  >({
    requestFunction: useCallback(
      async (
        relationshipType: DocumentRelationshipType,
        parentId: number,
        uploadRequest: Api_UploadRequest,
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

  // Provides functionality for uploading a document for a given relationship
  const { execute: updateDocument, loading: updateDocumentLoading } = useApiRequestWrapper<
    (
      relationshipType: DocumentRelationshipType,
      documentId: number,
      updateRequest: Api_DocumentUpdateRequest,
    ) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (
        relationshipType: DocumentRelationshipType,
        documentId: number,
        updateRequest: Api_DocumentUpdateRequest,
      ) => await updateDocumentMetadataApiCall(relationshipType, documentId, updateRequest),
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
        toast.error('Update document relationship error. Check responses and try again.');
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
    updateDocument,
    updateDocumentLoading,
  };
};
