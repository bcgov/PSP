import { FaEye } from 'react-icons/fa';

import GenericModal, { ModalSize } from '@/components/common/GenericModal';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';

import { DocumentRow } from '../ComposedDocument';
import { DocumentDetailContainer } from './DocumentDetailContainer';

export interface IDocumentDetailModalProps {
  display?: boolean;
  relationshipType: ApiGen_CodeTypes_DocumentRelationType;
  pimsDocument?: DocumentRow;
  canEdit: boolean;
  setDisplay?: (display: boolean) => void;
  onUpdateSuccess: () => void;
  onClose: () => void;
}

export const DocumentDetailModal: React.FunctionComponent<
  React.PropsWithChildren<IDocumentDetailModalProps>
> = props => {
  return (
    <GenericModal
      variant="info"
      headerIcon={<FaEye size={22} />}
      display={props.display}
      setDisplay={props.setDisplay}
      title={
        <>
          <span> View Document Information</span>
        </>
      }
      message={
        props.pimsDocument !== undefined && (
          <DocumentDetailContainer
            relationshipType={props.relationshipType}
            pimsDocument={props.pimsDocument}
            onUpdateSuccess={props.onUpdateSuccess}
            canEdit={props.canEdit}
          />
        )
      }
      modalSize={ModalSize.LARGE}
      handleCancel={props.onClose}
      hideFooter
    ></GenericModal>
  );
};
