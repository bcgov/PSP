import { FaEye } from 'react-icons/fa';

import GenericModal, { ModalSize } from '@/components/common/GenericModal';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';

import { DocumentDetailContainer } from './DocumentDetailContainer';

export interface IDocumentDetailModalProps {
  display?: boolean;
  relationshipType: ApiGen_CodeTypes_DocumentRelationType;
  pimsDocumentRelationship?: ApiGen_Concepts_DocumentRelationship;
  canEdit: boolean;
  setDisplay?: (display: boolean) => void;
  onUpdateSuccess?: () => void;
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
        props.pimsDocumentRelationship !== undefined && (
          <DocumentDetailContainer
            relationshipType={props.relationshipType}
            pimsDocumentRelationship={props.pimsDocumentRelationship}
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
