import GenericModal, { ModalSize } from 'components/common/GenericModal';
import { Api_Document } from 'models/api/Document';
import { FaEye } from 'react-icons/fa';

import { DocumentDetailContainer } from './DocumentDetailContainer';

export interface IDocumentDetailModalProps {
  display?: boolean;
  setDisplay?: (display: boolean) => void;
  pimsDocument?: Api_Document;
  onClose?: () => void;
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
          <DocumentDetailContainer pimsDocument={props.pimsDocument} />
        )
      }
      modalSize={ModalSize.LARGE}
      handleCancel={props.onClose}
      closeButton
      hideFooter
    ></GenericModal>
  );
};
