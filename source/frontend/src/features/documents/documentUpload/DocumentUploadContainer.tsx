import { SelectOption } from 'components/common/form';
import * as API from 'constants/API';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { DocumentStatusType } from 'constants/documentStatusType';
import { DocumentTypeName } from 'constants/documentType';
import { FormikProps } from 'formik';
import useIsMounted from 'hooks/useIsMounted';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { getCancelModalProps, useModalContext } from 'hooks/useModalContext';
import { Api_DocumentType, Api_DocumentUploadRequest } from 'models/api/Document';
import { Api_Storage_DocumentTypeMetadataType } from 'models/api/DocumentStorage';
import { ExternalResultStatus } from 'models/api/ExternalResult';
import { ChangeEvent, useCallback, useEffect, useRef, useState } from 'react';

import { DocumentUploadFormData } from '../ComposedDocument';
import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import DocumentUploadForm from './DocumentUploadForm';

export interface IDocumentUploadContainerProps {
  parentId: string;
  relationshipType: DocumentRelationshipType;
  onUploadSuccess: () => void;
  onCancel: () => void;
}

export const DocumentUploadContainer: React.FunctionComponent<
  React.PropsWithChildren<IDocumentUploadContainerProps>
> = props => {
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
  const { retrieveDocumentMetadataLoading, retrieveDocumentTypeMetadata, retrieveDocumentTypes } =
    useDocumentProvider();

  const { uploadDocument, uploadDocumentLoading } = useDocumentRelationshipProvider();

  const [documentTypes, setDocumentTypes] = useState<Api_DocumentType[]>([]);
  const [documentType, setDocumentType] = useState<string>('');

  const [documentStatusOptions, setDocumentStatusOptions] = useState<SelectOption[]>([]);

  const [documentTypeMetadataTypes, setDocumentTypeMetadataTypes] = useState<
    Api_Storage_DocumentTypeMetadataType[]
  >([]);

  const onDocumentTypeChange = async (changeEvent: ChangeEvent<HTMLInputElement>) => {
    const documentTypeId = Number(changeEvent.target.value);
    await updateDocumentType(documentTypes.find(x => x.id === documentTypeId));
  };

  const updateDocumentType = useCallback(
    async (documentType?: Api_DocumentType) => {
      if (documentType === undefined) {
        return;
      }

      setDocumentType(documentType.id?.toString() || '');
      if (documentType.mayanId) {
        const results = await retrieveDocumentTypeMetadata(documentType.mayanId);
        if (results?.status === ExternalResultStatus.Success) {
          setDocumentTypeMetadataTypes(results?.payload?.results);
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
      const axiosResponse = await retrieveDocumentTypes();
      if (axiosResponse && isMounted()) {
        if (props.relationshipType === DocumentRelationshipType.TEMPLATES) {
          setDocumentTypes(axiosResponse.filter(x => x.documentType === DocumentTypeName.CDOGS));
          updateDocumentType(axiosResponse.find(x => x.documentType === DocumentTypeName.CDOGS));
          setDocumentStatusOptions(
            retrievedDocumentStatusTypes.filter(x => x.value === DocumentStatusType.Final),
          );
        } else {
          setDocumentTypes(axiosResponse.filter(x => x.isDisabled !== true));
          setDocumentStatusOptions(retrievedDocumentStatusTypes);
        }
      }
    };

    fetch();
  }, [
    props.relationshipType,
    retrieveDocumentTypes,
    isMounted,
    updateDocumentType,
    getOptionsByType,
  ]);

  const onUploadDocument = async (uploadRequest: Api_DocumentUploadRequest) => {
    await uploadDocument(props.relationshipType, props.parentId, uploadRequest);
    props.onUploadSuccess();
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
      onUploadDocument={onUploadDocument}
      onCancel={handleCancelClick}
    />
  );
};
