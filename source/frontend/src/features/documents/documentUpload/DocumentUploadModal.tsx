import { createRef, useState } from 'react';
import { FaUpload } from 'react-icons/fa';

import GenericModal, { ModalSize } from '@/components/common/GenericModal';
import { DocumentRelationshipType } from '@/constants/documentRelationshipType';

import DocumentUploadContainer, { IDocumentUploadContainerRef } from './DocumentUploadContainer';

export interface IDocumentUploadModalProps {
  parentId: string;
  relationshipType: DocumentRelationshipType;
  display?: boolean;
  setDisplay?: (display: boolean) => void;
  onUploadSuccess: () => void;
  onClose: () => void;
}

export const DocumentUploadModal: React.FunctionComponent<
  React.PropsWithChildren<IDocumentUploadModalProps>
> = props => {
  const [canUpload, setCanUpload] = useState(false);

  const uploadContainerRef = createRef<IDocumentUploadContainerRef>();

  const onSaveClick = () => {
    uploadContainerRef.current?.uploadDocument();
  };
  const onSuccess = () => {
    setCanUpload(false);
    props.onUploadSuccess();
  };
  return (
    <GenericModal
      variant="info"
      className=""
      display={props.display}
      setDisplay={props.setDisplay}
      closeButton={false}
      headerIcon={<FaUpload size={22} />}
      title={'Add a Document'}
      message={
        <DocumentUploadContainer
          ref={uploadContainerRef}
          parentId={props.parentId}
          relationshipType={props.relationshipType}
          onUploadSuccess={onSuccess}
          onCancel={props.onClose}
          setCanUpload={setCanUpload}
        />
      }
      modalSize={ModalSize.LARGE}
      handleOk={() => onSaveClick()}
      handleOkDisabled={!canUpload}
      handleCancel={() => {
        props.onClose();
      }}
      cancelButtonText="No"
      okButtonText="Yes"
    ></GenericModal>
  );
};
