import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { FormikProps } from 'formik';
import useIsMounted from 'hooks/useIsMounted';
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
  parentId: number;
  relationshipType: DocumentRelationshipType;
  onUploadSuccess: () => void;
  onCancel: () => void;
}

export const DocumentUploadContainer: React.FunctionComponent<IDocumentUploadContainerProps> = props => {
  const deleteModalProps = getCancelModalProps();

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
    retrieveDocumentTypes,
  } = useDocumentProvider();

  const { uploadDocument, uploadDocumentLoading } = useDocumentRelationshipProvider();

  const [documentTypes, setDocumentTypes] = useState<Api_DocumentType[]>([]);
  const [documentType, setDocumentType] = useState<string>('');

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
    const fetch = async () => {
      const axiosResponse = await retrieveDocumentTypes();
      if (axiosResponse && isMounted()) {
        if (props.relationshipType === DocumentRelationshipType.TEMPLATES) {
          setDocumentTypes(axiosResponse.filter(x => x.documentType === 'CDOGS Template'));
          updateDocumentType(axiosResponse.find(x => x.documentType === 'CDOGS Template'));
        } else {
          setDocumentTypes(axiosResponse);
        }
      }
    };

    fetch();
  }, [props.relationshipType, retrieveDocumentTypes, isMounted, updateDocumentType]);

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
      mayanMetadataTypes={documentTypeMetadataTypes}
      onDocumentTypeChange={onDocumentTypeChange}
      onUploadDocument={onUploadDocument}
      onCancel={handleCancelClick}
    />
  );
};
