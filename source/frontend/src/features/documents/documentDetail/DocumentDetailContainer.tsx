import { FormikProps } from 'formik/dist/types';
import { ChangeEvent, useCallback, useContext, useEffect, useMemo, useRef, useState } from 'react';

import { ModalProps } from '@/components/common/GenericModal';
import { DocumentTypeName } from '@/constants/documentType';
import { ModalContext } from '@/contexts/modalContext';
import { useApiDocuments } from '@/hooks/pims-api/useApiDocuments';
import { getCancelModalProps } from '@/hooks/useModalContext';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
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
  const [documentTypeUpdated, setDocumentTypeUpdated] = useState<boolean>(false);
  const [documentTypes, setDocumentTypes] = useState<ApiGen_Concepts_DocumentType[]>([]);

  const {
    retrieveDocumentMetadata,
    retrieveDocumentMetadataLoading,
    updateDocument,
    updateDocumentLoading,
    retrieveDocumentDetail,
    retrieveDocumentDetailLoading,
    getDocumentTypes,
    getDocumentRelationshipTypes,
    retrieveDocumentTypeMetadata,
  } = useDocumentProvider();
  const { getDocumentTypeMetadataApiCall } = useApiDocuments();

  const formikRef = useRef<FormikProps<DocumentUpdateFormData>>(null);

  const deleteModalProps = useMemo(() => getCancelModalProps(), []);

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

  const onUpdateDocument = async (updateRequest: ApiGen_Requests_DocumentUpdateRequest) => {
    if (props.pimsDocument.id) {
      const result = await updateDocument(props.pimsDocument.id, updateRequest);
      if (exists(result)) {
        props.onUpdateSuccess && props.onUpdateSuccess();
      }
    }
  };

  const fetchDocumentTypes = useCallback(async () => {
    if (props.relationshipType === ApiGen_CodeTypes_DocumentRelationType.Templates) {
      const response = await getDocumentTypes();
      if (exists(response) && isMounted()) {
        setDocumentTypes(response.filter(x => x.documentType === DocumentTypeName.CDOGS));
      }
    } else {
      const axiosResponse = await getDocumentRelationshipTypes(props.relationshipType);
      if (axiosResponse && isMounted()) {
        setDocumentTypes(axiosResponse.filter(x => x.isDisabled !== true));
      }
    }
  }, [getDocumentRelationshipTypes, getDocumentTypes, isMounted, props.relationshipType]);

  const getDocumentMetadata = useCallback(
    async (documentType?: ApiGen_Concepts_DocumentType) => {
      if (
        documentType === undefined ||
        props.relationshipType === ApiGen_CodeTypes_DocumentRelationType.Templates
      ) {
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
    [documentTypeMetadataTypes, props.relationshipType, retrieveDocumentTypeMetadata],
  );

  const updateDocumentType = useCallback(
    async (documentType?: ApiGen_Concepts_DocumentType) => {
      if (!exists(documentType)) {
        return;
      }
      if (documentType.mayanId) {
        const retrievedMetadata = await getDocumentMetadata(documentType);
        if (exists(retrievedMetadata)) {
          setDocumentTypeMetadataTypes(retrievedMetadata);
        }
      }
    },
    [getDocumentMetadata],
  );

  const onDocumentTypeChange = useCallback(
    async (changeEvent: ChangeEvent<HTMLInputElement>) => {
      if (changeEvent.target.value) {
        const documentTypeId = Number(changeEvent.target.value);
        if (documentTypeId !== props.pimsDocument.documentType.id) {
          await updateDocumentType(documentTypes.find(x => x.id === documentTypeId));
          setDocumentTypeUpdated(true);
        } else {
          setDocumentTypeUpdated(false);
        }
      } else {
        formikRef.current?.setFieldValue('documentTypeId', null);
        setDocumentTypeUpdated(false);
      }
    },
    [documentTypes, props.pimsDocument.documentType.id, updateDocumentType],
  );

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
      if (props.relationshipType === ApiGen_CodeTypes_DocumentRelationType.Templates) {
        return;
      }

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
    props.relationshipType,
  ]);

  useEffect(() => {
    if (isEditable) {
      fetchDocumentTypes();
    }
  }, [fetchDocumentTypes, isEditable]);

  if (isEditable) {
    return (
      <DocumentDetailForm
        formikRef={formikRef}
        document={document}
        mayanMetadataTypes={documentTypeMetadataTypes}
        documentTypes={documentTypes}
        isLoading={
          retrieveDocumentMetadataLoading || updateDocumentLoading || retrieveDocumentDetailLoading
        }
        relationshipType={props.relationshipType}
        documentTypeUpdated={documentTypeUpdated}
        onUpdate={onUpdateDocument}
        onCancel={onCancelClick}
        onDocumentTypeChange={onDocumentTypeChange}
      />
    );
  } else {
    return (
      <DocumentDetailView
        document={document}
        isLoading={
          retrieveDocumentMetadataLoading || updateDocumentLoading || retrieveDocumentDetailLoading
        }
        setIsEditable={setIsEditable}
      />
    );
  }
};

export default DocumentDetailContainer;
