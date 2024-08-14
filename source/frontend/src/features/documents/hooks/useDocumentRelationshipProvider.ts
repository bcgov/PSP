import { AxiosError, AxiosResponse } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiDocuments } from '@/hooks/pims-api/useApiDocuments';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Requests_DocumentUploadRelationshipResponse } from '@/models/api/generated/ApiGen_Requests_DocumentUploadRelationshipResponse';
import { ApiGen_Requests_DocumentUploadRequest } from '@/models/api/generated/ApiGen_Requests_DocumentUploadRequest';

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
        relationshipType: ApiGen_CodeTypes_DocumentRelationType,
        parentId: string,
      ) => Promise<AxiosResponse<ApiGen_Concepts_DocumentRelationship[], any>>
    >({
      requestFunction: useCallback(
        async (relationshipType: ApiGen_CodeTypes_DocumentRelationType, parentId: string) =>
          await getDocumentRelationshipApiCall(relationshipType, parentId),
        [getDocumentRelationshipApiCall],
      ),
      requestName: 'retrieveDocumentRelationship',
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
          return Promise.resolve();
        } else {
          toast.error('Retrieve document relationship error. Check responses and try again.');
          return Promise.reject(axiosError);
        }
      }, []),
    });

  // Provides functionality for deleting a document relationship
  const { execute: deleteDocumentRelationship, loading: deleteDocumentRelationshipLoading } =
    useApiRequestWrapper<
      (
        relationshipType: ApiGen_CodeTypes_DocumentRelationType,
        documentRelationship: ApiGen_Concepts_DocumentRelationship,
      ) => Promise<AxiosResponse<boolean, any>>
    >({
      requestFunction: useCallback(
        async (
          relationshipType: ApiGen_CodeTypes_DocumentRelationType,
          documentRelationship: ApiGen_Concepts_DocumentRelationship,
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
          return Promise.resolve();
        } else {
          toast.error('Delete document relationship error. Check responses and try again.');
          return Promise.reject(axiosError);
        }
      }, []),
    });

  // Provides functionality for uploading a document for a given relationship
  const { execute: uploadDocument, loading: uploadDocumentLoading } = useApiRequestWrapper<
    (
      relationshipType: ApiGen_CodeTypes_DocumentRelationType,
      parentId: string,
      uploadRequest: ApiGen_Requests_DocumentUploadRequest,
    ) => Promise<AxiosResponse<ApiGen_Requests_DocumentUploadRelationshipResponse, any>>
  >({
    requestFunction: useCallback(
      async (
        relationshipType: ApiGen_CodeTypes_DocumentRelationType,
        parentId: string,
        uploadRequest: ApiGen_Requests_DocumentUploadRequest,
      ) => await uploadDocumentRelationshipApiCall(relationshipType, parentId, uploadRequest),
      [uploadDocumentRelationshipApiCall],
    ),
    requestName: 'uploadDocumentRelationshipApiCall',
    onSuccess: useCallback(() => {
      toast.success('Uploaded document relationship');
    }, []),
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response?.data.error);
        return Promise.resolve();
      } else {
        toast.error(axiosError?.response?.data.error);
        return Promise.reject(axiosError);
      }
    }, []),
    returnApiError: true,
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
