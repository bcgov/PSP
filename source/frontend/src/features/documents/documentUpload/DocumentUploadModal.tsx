import { FaUpload } from 'react-icons/fa';

import GenericModal, { ModalSize } from '@/components/common/GenericModal';
import { DocumentRelationshipType } from '@/constants/documentRelationshipType';

import { DocumentUploadContainer } from './DocumentUploadContainer';

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
  return (
    <GenericModal
      display={props.display}
      setDisplay={props.setDisplay}
      title={
        <>
          <FaUpload />
          <span className="ml-3">Add a Document</span>
        </>
      }
      message={
        <DocumentUploadContainer
          parentId={props.parentId}
          relationshipType={props.relationshipType}
          onUploadSuccess={props.onUploadSuccess}
          onCancel={props.onClose}
        />
      }
      modalSize={ModalSize.LARGE}
      hideFooter
    ></GenericModal>
  );
};
