import GenericModal, { ModalSize } from 'components/common/GenericModal';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { Api_Document } from 'models/api/Document';
import { FaEye } from 'react-icons/fa';

import { DocumentDetailContainer } from './DocumentDetailContainer';

export interface IDocumentDetailModalProps {
  display?: boolean;
  relationshipType: DocumentRelationshipType;
  setDisplay?: (display: boolean) => void;
  pimsDocument?: Api_Document;
  onUpdateSuccess: () => void;
  onClose: () => void;
}

export const DocumentDetailModal: React.FunctionComponent<IDocumentDetailModalProps> = props => {
  return (
    <GenericModal
      display={props.display}
      setDisplay={props.setDisplay}
      title={
        <>
          <FaEye />
          <span> View Document Information</span>
        </>
      }
      message={
        props.pimsDocument !== undefined && (
          <DocumentDetailContainer
            relationshipType={props.relationshipType}
            pimsDocument={props.pimsDocument}
            onUpdateSuccess={props.onUpdateSuccess}
          />
        )
      }
      modalSize={ModalSize.LARGE}
      handleCancel={props.onClose}
      closeButton
      hideFooter
    ></GenericModal>
  );
};
