import GenericModal, { ModalSize } from 'components/common/GenericModal';
import { Api_Document } from 'models/api/Document';
import { FaFile } from 'react-icons/fa';

import { DocumentDetailContainer } from './DocumentDetailContainer';

export interface IDocumentDetailModalProps {
  display?: boolean;
  setDisplay?: (display: boolean) => void;
  pimsDocument?: Api_Document;
  handleClose?: () => void;
}

export const DocumentDetailModal: React.FunctionComponent<IDocumentDetailModalProps> = props => {
  return (
    <GenericModal
      display={props.display}
      setDisplay={props.setDisplay}
      title={
        <>
          <FaFile />
          <span> View Document Information</span>
        </>
      }
      message={
        props.pimsDocument !== undefined && (
          <DocumentDetailContainer pimsDocument={props.pimsDocument} />
        )
      }
      modalSize={ModalSize.MEDIUM}
      handleCancel={props.handleClose}
      closeButton
      hideFooter
    ></GenericModal>
  );
};
