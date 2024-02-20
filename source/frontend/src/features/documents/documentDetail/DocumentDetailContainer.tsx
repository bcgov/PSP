import { FormikProps } from 'formik';
import { useCallback, useContext, useEffect, useMemo, useRef, useState } from 'react';

import { ModalProps } from '@/components/common/GenericModal';
import { ModalContext } from '@/contexts/modalContext';
import { useApiDocuments } from '@/hooks/pims-api/useApiDocuments';
import { getCancelModalProps } from '@/hooks/useModalContext';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { ApiGen_Requests_DocumentUpdateRequest } from '@/models/api/generated/ApiGen_Requests_DocumentUpdateRequest';
import { exists } from '@/utils/utils';

import { ComposedDocument, DocumentRow, DocumentUpdateFormData } from '../ComposedDocument';
import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { DocumentDetailForm } from './DocumentDetailForm';
import { DocumentDetailView } from './DocumentDetailView';

export interface IDocumentDetailContainerProps {
  relationshipType: ApiGen_CodeTypes_DocumentRelationType;
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
    ApiGen_Mayan_DocumentTypeMetadataType[]
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
          metadataResponse?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
          detailResponse?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
          isMounted()
        ) {
          const mayanMetadataResult = metadataResponse.payload?.results;

          let mayanFileId: number | undefined = undefined;
          if (exists(mayanMetadataResult) && mayanMetadataResult.length > 0) {
            const document = mayanMetadataResult[0].document;
            mayanFileId = document?.file_latest?.id;
          }

          setDocument(document => ({
            ...document,
            mayanMetadata: mayanMetadataResult ?? undefined,
            mayanFileId: mayanFileId ?? undefined,
            documentDetail: detailResponse.payload ?? undefined,
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
          if (
            axiosResponse?.data.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
            isMounted()
          ) {
            const results = axiosResponse?.data.payload?.results;
            setDocumentTypeMetadataTypes(results || []);
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

  const onUpdateDocument = async (updateRequest: ApiGen_Requests_DocumentUpdateRequest) => {
    if (props.pimsDocument.id) {
      const result = await updateDocument(props.pimsDocument.id, updateRequest);
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
