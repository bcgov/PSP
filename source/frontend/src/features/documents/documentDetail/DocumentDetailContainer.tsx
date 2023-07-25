import { FormikProps } from 'formik';
import { useCallback, useContext, useEffect, useMemo, useRef, useState } from 'react';

import { ModalProps } from '@/components/common/GenericModal';
import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import { ModalContext } from '@/contexts/modalContext';
import { useApiDocuments } from '@/hooks/pims-api/useApiDocuments';
import { getCancelModalProps } from '@/hooks/useModalContext';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import useIsMounted from '@/hooks/util/useIsMounted';
import { Api_DocumentUpdateRequest } from '@/models/api/Document';
import { Api_Storage_DocumentTypeMetadataType } from '@/models/api/DocumentStorage';
import { ExternalResultStatus } from '@/models/api/ExternalResult';

import { ComposedDocument, DocumentRow, DocumentUpdateFormData } from '../ComposedDocument';
import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { DocumentDetailForm } from './DocumentDetailForm';
import { DocumentDetailView } from './DocumentDetailView';

export interface IDocumentDetailContainerProps {
  relationshipType: DocumentRelationshipType;
  pimsDocument: DocumentRow;
  onUpdateSuccess: () => void;
}

export const DocumentDetailContainer: React.FunctionComponent<
  React.PropsWithChildren<IDocumentDetailContainerProps>
> = props => {
  const [document, setDocument] = useState<ComposedDocument>({
    pimsDocumentRelationship: DocumentRow.toApi(props.pimsDocument),
  });
  const [documentTypeMetadataTypes, setDocumentTypeMetadataTypes] = useState<
    Api_Storage_DocumentTypeMetadataType[]
  >([]);
  const [isEditable, setIsEditable] = useState<boolean>(false);

  const {
    retrieveDocumentMetadata,
    retrieveDocumentMetadataLoading,
    updateDocument,
    updateDocumentLoading,
    retrieveDocumentDetail,
    retrieveDocumentDetailLoading,
  } = useDocumentProvider();
  const { getDocumentTypeMetadataApiCall } = useApiDocuments();

  const formikRef = useRef<FormikProps<DocumentUpdateFormData>>(null);

  const deleteModalProps = getCancelModalProps();

  const { setDisplayModal, setModalContent } = useContext(ModalContext);

  const isMounted = useIsMounted();

  const confirmCancel = useCallback(() => {
    setDisplayModal(false);
    setIsEditable(false);
  }, [setDisplayModal]);

  const customModalProps: ModalProps = useMemo(() => {
    return {
      ...deleteModalProps,
      display: false,
      closeButton: false,
      handleOk: confirmCancel,
    };
  }, [deleteModalProps, confirmCancel]);

  useDeepCompareEffect(() => {
    setModalContent(customModalProps);
  }, [customModalProps]);

  const onCancelClick = () => {
    if (formikRef.current?.dirty === true) {
      setDisplayModal(true);
    } else {
      setIsEditable(false);
    }
  };

  useEffect(() => {
    const fetch = async () => {
      if (props.pimsDocument.mayanDocumentId !== undefined) {
        const metadataPromise = retrieveDocumentMetadata(props.pimsDocument.mayanDocumentId);
        const detailPromise = retrieveDocumentDetail(props.pimsDocument.mayanDocumentId);
        const [metadataResponse, detailResponse] = await Promise.all([
          metadataPromise,
          detailPromise,
        ]);
        if (
          metadataResponse?.status === ExternalResultStatus.Success &&
          detailResponse?.status === ExternalResultStatus.Success &&
          isMounted()
        ) {
          let mayanMetadataResult = metadataResponse.payload.results;

          let mayanFileId: number | undefined = undefined;
          if (mayanMetadataResult !== undefined && mayanMetadataResult?.length > 0) {
            const document = mayanMetadataResult[0]?.document;
            mayanFileId = document.file_latest.id;
          }

          setDocument(document => ({
            ...document,
            mayanMetadata: mayanMetadataResult,
            mayanFileId: mayanFileId,
            documentDetail: detailResponse.payload,
          }));
        }
      }
    };

    fetch();
  }, [
    props.pimsDocument.mayanDocumentId,
    retrieveDocumentMetadata,
    isMounted,
    retrieveDocumentDetail,
  ]);

  useEffect(() => {
    const fetch = async () => {
      if (props.pimsDocument.mayanDocumentId !== undefined) {
        const mayanDocumentTypeId = props.pimsDocument.documentType?.mayanId;
        if (mayanDocumentTypeId) {
          const axiosResponse = await getDocumentTypeMetadataApiCall(mayanDocumentTypeId);
          if (axiosResponse?.data.status === ExternalResultStatus.Success && isMounted()) {
            let results = axiosResponse?.data.payload.results;
            setDocumentTypeMetadataTypes(results);
          }
        }
      }
    };
    fetch();
  }, [
    props.pimsDocument.mayanDocumentId,
    props.pimsDocument.documentType?.mayanId,
    getDocumentTypeMetadataApiCall,
    isMounted,
  ]);

  const onUpdateDocument = async (updateRequest: Api_DocumentUpdateRequest) => {
    if (props.pimsDocument.id) {
      let result = await updateDocument(props.pimsDocument.id, updateRequest);
      result && props.onUpdateSuccess();
    }
  };

  if (isEditable === false) {
    return (
      <DocumentDetailView
        document={document}
        isLoading={
          retrieveDocumentMetadataLoading || updateDocumentLoading || retrieveDocumentDetailLoading
        }
        setIsEditable={setIsEditable}
      />
    );
  } else {
    return (
      <DocumentDetailForm
        formikRef={formikRef}
        document={document}
        isLoading={
          retrieveDocumentMetadataLoading || updateDocumentLoading || retrieveDocumentDetailLoading
        }
        onUpdate={onUpdateDocument}
        onCancel={onCancelClick}
        mayanMetadataTypes={documentTypeMetadataTypes}
      />
    );
  }
};
