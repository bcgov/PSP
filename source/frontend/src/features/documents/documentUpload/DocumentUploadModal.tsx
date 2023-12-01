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
      variant="info"
      display={props.display}
      setDisplay={props.setDisplay}
      closeButton={false}
      headerIcon={<FaUpload size={22} />}
      title={'Add a Document'}
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
