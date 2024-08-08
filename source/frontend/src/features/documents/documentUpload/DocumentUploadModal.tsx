import { createRef, useState } from 'react';
import { FaUpload } from 'react-icons/fa';

import GenericModal, { ModalSize } from '@/components/common/GenericModal';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';

import DocumentUploadContainer, { IDocumentUploadContainerRef } from './DocumentUploadContainer';

export interface IDocumentUploadModalProps {
  parentId: string;
  relationshipType: ApiGen_CodeTypes_DocumentRelationType;
  display?: boolean;
  setDisplay?: (display: boolean) => void;
  onUploadSuccess: () => void;
  onClose: () => void;
}

export const DocumentUploadModal: React.FunctionComponent<IDocumentUploadModalProps> = props => {
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
      display={props.display}
      setDisplay={props.setDisplay}
      closeButton={false}
      headerIcon={<FaUpload size={22} />}
      title="Add Document"
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
