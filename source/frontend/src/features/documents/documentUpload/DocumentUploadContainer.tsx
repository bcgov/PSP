import { FormikProps } from 'formik';
import {
  ChangeEvent,
  forwardRef,
  useCallback,
  useEffect,
  useImperativeHandle,
  useRef,
  useState,
} from 'react';

import { SelectOption } from '@/components/common/form';
import * as API from '@/constants/API';
import { DocumentStatusType } from '@/constants/documentStatusType';
import { DocumentTypeName } from '@/constants/documentType';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { ApiGen_Requests_DocumentUploadRequest } from '@/models/api/generated/ApiGen_Requests_DocumentUploadRequest';

import { DocumentUploadFormData } from '../ComposedDocument';
import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import DocumentUploadForm from './DocumentUploadForm';

export interface IDocumentUploadContainerProps {
  ref: React.RefObject<
    React.FunctionComponent<React.PropsWithChildren<IDocumentUploadContainerProps>>
  >;
  parentId: string;
  relationshipType: ApiGen_CodeTypes_DocumentRelationType;
  onUploadSuccess: () => void;
  onCancel: () => void;
  setCanUpload: (canUpload: boolean) => void;
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
    closeButton: false,
    handleOk: () => {
      handleCancelConfirm();
    },
  });

  const formikRef = useRef<FormikProps<DocumentUploadFormData>>(null);

  const handleCancelClick = () => {
    if (formikRef.current?.dirty) {
      setDisplayModal(true);
    } else {
      handleCancelConfirm();
    }
  };

  const handleCancelConfirm = () => {
    setDisplayModal(false);
    formikRef.current?.resetForm();
    props.onCancel();
  };

  const isMounted = useIsMounted();
  const {
    retrieveDocumentMetadataLoading,
    retrieveDocumentTypeMetadata,
    getDocumentRelationshipTypes,
    getDocumentTypes,
  } = useDocumentProvider();

  const { uploadDocument, uploadDocumentLoading } = useDocumentRelationshipProvider();

  const [documentTypes, setDocumentTypes] = useState<ApiGen_Concepts_DocumentType[]>([]);
  const [documentType, setDocumentType] = useState<string>('');

  const [documentStatusOptions, setDocumentStatusOptions] = useState<SelectOption[]>([]);

  const [documentTypeMetadataTypes, setDocumentTypeMetadataTypes] = useState<
    ApiGen_Mayan_DocumentTypeMetadataType[]
  >([]);

  const onDocumentTypeChange = async (changeEvent: ChangeEvent<HTMLInputElement>) => {
    const documentTypeId = Number(changeEvent.target.value);
    await updateDocumentType(documentTypes.find(x => x.id === documentTypeId));
  };

  const updateDocumentType = useCallback(
    async (documentType?: ApiGen_Concepts_DocumentType) => {
      if (documentType === undefined) {
        return;
      }

      setDocumentType(documentType.id?.toString() || '');
      if (documentType.mayanId) {
        const results = await retrieveDocumentTypeMetadata(documentType.mayanId);
        if (results?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success) {
          setDocumentTypeMetadataTypes(results?.payload?.results || []);
        }
      } else {
        setDocumentTypeMetadataTypes([]);
      }
    },
    [retrieveDocumentTypeMetadata],
  );

  useEffect(() => {
    const retrievedDocumentStatusTypes = getOptionsByType(API.DOCUMENT_STATUS_TYPES);
    const fetch = async () => {
      if (props.relationshipType === ApiGen_CodeTypes_DocumentRelationType.Templates) {
        const axiosResponse = await getDocumentTypes();
        if (axiosResponse && isMounted()) {
          setDocumentTypes(axiosResponse.filter(x => x.documentType === DocumentTypeName.CDOGS));
          updateDocumentType(axiosResponse.find(x => x.documentType === DocumentTypeName.CDOGS));
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
    updateDocumentType,
    getOptionsByType,
  ]);

  const onUploadDocument = async (uploadRequest: ApiGen_Requests_DocumentUploadRequest) => {
    const result = await uploadDocument(props.relationshipType, props.parentId, uploadRequest);
    if (result !== undefined) {
      props.onUploadSuccess();
    }
  };

  useImperativeHandle(ref, () => ({
    uploadDocument() {
      formikRef.current?.submitForm();
    },
  }));

  const onDocumentSelected = () => {
    props.setCanUpload(true);
  };

  return (
    <DocumentUploadForm
      formikRef={formikRef}
      isLoading={retrieveDocumentMetadataLoading || uploadDocumentLoading}
      initialDocumentType={documentType}
      documentTypes={documentTypes}
      documentStatusOptions={documentStatusOptions}
      mayanMetadataTypes={documentTypeMetadataTypes}
      onDocumentTypeChange={onDocumentTypeChange}
      onDocumentSelected={onDocumentSelected}
      onUploadDocument={onUploadDocument}
      onCancel={handleCancelClick}
    />
  );
});

export default DocumentUploadContainer;
