import GenericModal, { ModalSize } from 'components/common/GenericModal';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { FaFile, FaUpload } from 'react-icons/fa';

import { DocumentUploadContainer } from './DocumentUploadContainer';

export interface IDocumentUploadModalProps {
  parentId: number;
  relationshipType: DocumentRelationshipType;
  display?: boolean;
  setDisplay?: (display: boolean) => void;
  onClose?: () => void;
  onUploadSuccess: () => void;
}

export const DocumentUploadModal: React.FunctionComponent<IDocumentUploadModalProps> = props => {
  return (
    <GenericModal
      display={props.display}
      setDisplay={props.setDisplay}
      title={
        <>
          <FaUpload />
          <span> Add a Document</span>
        </>
      }
      message={
        <DocumentUploadContainer
          parentId={props.parentId}
          relationshipType={props.relationshipType}
          onUploadSuccess={props.onUploadSuccess}
        />
      }
      modalSize={ModalSize.LARGE}
      okButtonText="Save"
      cancelButtonText="Cancel"
      handleCancel={props.onClose}
    ></GenericModal>
  );
};
