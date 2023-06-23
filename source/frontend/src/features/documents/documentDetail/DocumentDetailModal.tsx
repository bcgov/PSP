import { FaEye } from 'react-icons/fa';

import GenericModal, { ModalSize } from '@/components/common/GenericModal';
import { DocumentRelationshipType } from '@/constants/documentRelationshipType';

import { DocumentRow } from '../ComposedDocument';
import { DocumentDetailContainer } from './DocumentDetailContainer';

export interface IDocumentDetailModalProps {
  display?: boolean;
  relationshipType: DocumentRelationshipType;
  setDisplay?: (display: boolean) => void;
  pimsDocument?: DocumentRow;
  onUpdateSuccess: () => void;
  onClose: () => void;
}

export const DocumentDetailModal: React.FunctionComponent<
  React.PropsWithChildren<IDocumentDetailModalProps>
> = props => {
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
