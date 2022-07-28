import GenericModal, { ModalSize } from 'components/common/GenericModal';
import { FaFile, FaUpload } from 'react-icons/fa';

import { DocumentUploadContainer } from './DocumentUploadContainer';

export interface IDocumentUploadModalProps {
  display?: boolean;
  setDisplay?: (display: boolean) => void;
  handleClose?: () => void;
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
      message={<DocumentUploadContainer />}
      modalSize={ModalSize.MEDIUM}
      okButtonText="Save"
      cancelButtonText="Cancel"
      handleCancel={props.handleClose}
    ></GenericModal>
  );
};
