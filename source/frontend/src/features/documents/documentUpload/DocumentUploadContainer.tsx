import { FormikProps } from 'formik/dist/types';
import { forwardRef, useCallback, useEffect, useImperativeHandle, useRef, useState } from 'react';

import { SelectOption } from '@/components/common/form';
import * as API from '@/constants/API';
import { DocumentStatusType } from '@/constants/documentStatusType';
import { DocumentTypeName } from '@/constants/documentType';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import useIsMounted from '@/hooks/util/useIsMounted';
import { Dictionary } from '@/interfaces/Dictionary';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { ApiGen_Requests_DocumentUploadRelationshipResponse } from '@/models/api/generated/ApiGen_Requests_DocumentUploadRelationshipResponse';
import { exists } from '@/utils';

import { BatchUploadFormModel, BatchUploadResponseModel } from '../ComposedDocument';
import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import DocumentUploadForm from './DocumentUploadForm';

export interface IDocumentUploadContainerProps {
  ref: React.RefObject<
    React.FunctionComponent<React.PropsWithChildren<IDocumentUploadContainerProps>>
  >;
  parentId: string;
  relationshipType: ApiGen_CodeTypes_DocumentRelationType;
  onUploadSuccess: (results: BatchUploadResponseModel[]) => void;
  onCancel: () => void;
  setCanUpload: (canUpload: boolean) => void;
  maxDocumentCount: number;
}

export interface IDocumentUploadContainerRef {
  uploadDocument: () => void;
}

const DocumentUploadContainer = forwardRef<
  IDocumentUploadContainerRef,
  IDocumentUploadContainerProps
>((props, ref) => {
  const deleteModalProps = getCancelModalProps();

  const { getOptionsByType } = useLookupCodeHelpers();

  const { setDisplayModal } = useModalContext({
    ...deleteModalProps,
    handleOk: () => {
      handleCancelConfirm();
    },
  });

  const formikRef = useRef<FormikProps<BatchUploadFormModel>>(null);

  const handleCancelConfirm = () => {
    setDisplayModal(false);
    formikRef.current?.resetForm();
    props.onCancel();
  };

  const isMounted = useIsMounted();
  const { retrieveDocumentTypeMetadata, getDocumentRelationshipTypes, getDocumentTypes } =
    useDocumentProvider();

  const [isUploading, setIsUploading] = useState(false);
  const { uploadDocument } = useDocumentRelationshipProvider();

  const [documentTypes, setDocumentTypes] = useState<ApiGen_Concepts_DocumentType[]>([]);

  const [documentStatusOptions, setDocumentStatusOptions] = useState<SelectOption[]>([]);

  const [documentTypeMetadataTypes, setDocumentTypeMetadataTypes] = useState<
    Dictionary<ApiGen_Mayan_DocumentTypeMetadataType[]>
  >({});

  const getDocumentMetadata = useCallback(
    async (documentType?: ApiGen_Concepts_DocumentType) => {
      if (documentType === undefined) {
        return;
      }

      if (documentTypeMetadataTypes[documentType.id.toString()] === undefined) {
        if (documentType.mayanId) {
          const results = await retrieveDocumentTypeMetadata(documentType.mayanId);

          if (results?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success) {
            setDocumentTypeMetadataTypes(prevState => ({
              ...prevState,
              [documentType.id.toString()]: results.payload.results,
            }));
            return results.payload.results;
          }
        } else {
          console.error('Document type does not have a mayan id type');
        }
      } else {
        return documentTypeMetadataTypes[documentType.id.toString()];
      }
    },
    [documentTypeMetadataTypes, retrieveDocumentTypeMetadata],
  );

  useEffect(() => {
    const retrievedDocumentStatusTypes = getOptionsByType(API.DOCUMENT_STATUS_TYPES);
    const fetch = async () => {
      if (props.relationshipType === ApiGen_CodeTypes_DocumentRelationType.Templates) {
        const response = await getDocumentTypes();
        if (exists(response) && isMounted()) {
          setDocumentTypes(response.filter(x => x.documentType === DocumentTypeName.CDOGS));
          //getDocumentMetadata(response.find(x => x.documentType === DocumentTypeName.CDOGS));
          setDocumentStatusOptions(
            retrievedDocumentStatusTypes.filter(x => x.value === DocumentStatusType.Final),
          );
        }
      } else {
        const axiosResponse = await getDocumentRelationshipTypes(props.relationshipType);
        if (axiosResponse && isMounted()) {
          setDocumentTypes(axiosResponse.filter(x => x.isDisabled !== true));
          setDocumentStatusOptions(retrievedDocumentStatusTypes);
        }
      }
    };

    fetch();
  }, [
    props.relationshipType,
    getDocumentTypes,
    getDocumentRelationshipTypes,
    isMounted,
    getOptionsByType,
    getDocumentMetadata,
  ]);

  const onUploadDocument = async (batchRequest: BatchUploadFormModel) => {
    const uploadDocumentTasks = batchRequest.documents.map(d => {
      return uploadDocument(props.relationshipType, props.parentId, d.toRequestApi(documentTypes));
    });
    setIsUploading(true);
    const tasksResult: (IApiError | ApiGen_Requests_DocumentUploadRelationshipResponse)[] =
      await Promise.all(uploadDocumentTasks);
    setIsUploading(false);
    if (tasksResult !== undefined) {
      const batchResults = tasksResult.map<BatchUploadResponseModel>((r, index) => {
        const fileName = batchRequest.documents[index].file.name;
        return new BatchUploadResponseModel(fileName, r);
      });
      props.onUploadSuccess(batchResults);
    }
  };

  useImperativeHandle(ref, () => ({
    uploadDocument() {
      formikRef.current?.submitForm();
    },
  }));

  const onDocumentsSelected = (documentCount: number) => {
    if (documentCount > 0 && documentCount <= props.maxDocumentCount) {
      props.setCanUpload(true);
    } else {
      props.setCanUpload(false);
    }
  };

  return (
    <DocumentUploadForm
      formikRef={formikRef}
      isLoading={isUploading}
      initialDocumentType={''}
      documentTypes={documentTypes}
      maxDocumentCount={props.maxDocumentCount}
      documentStatusOptions={documentStatusOptions}
      getDocumentMetadata={getDocumentMetadata}
      onDocumentsSelected={onDocumentsSelected}
      onUploadDocument={onUploadDocument}
    />
  );
});

export default DocumentUploadContainer;
