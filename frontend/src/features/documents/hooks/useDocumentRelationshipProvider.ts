import { AxiosError, AxiosResponse } from 'axios';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { useApiDocuments } from 'hooks/pims-api/useApiDocuments';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { IApiError } from 'interfaces/IApiError';
import { Api_DocumentRelationship } from 'models/api/Document';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

/**
 * hook that retrieves document relationship information.
 */
export const useDocumentRelationshipProvider = () => {
  const { getDocumentRelationship, deleteDocumentRelationshipApiCall } = useApiDocuments();

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
    onSuccess: useCallback(() => toast.success('Document relationship retrieved'), []),
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
      parentId: number,
    ) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (relationshipType: DocumentRelationshipType, parentId: number) =>
        await deleteDocumentRelationshipApiCall(relationshipType, parentId),
      [deleteDocumentRelationshipApiCall],
    ),
    requestName: 'deleteDocumentRelationship',
    onSuccess: useCallback(() => toast.success('Delete document relationship'), []),
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Delete document relationship error. Check responses and try again.');
      }
    }, []),
  });

  return {
    retrieveDocumentRelationship,
    retrieveDocumentRelationshipLoading,
    deleteDocumentRelationship,
    deleteDocumentRelationshipLoading,
  };
};
