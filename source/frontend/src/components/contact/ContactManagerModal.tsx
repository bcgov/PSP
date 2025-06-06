import { FaUser } from 'react-icons/fa';

import GenericModal, { ModalSize } from '@/components/common/GenericModal';
import { IContactSearchResult } from '@/interfaces';

import { RestrictContactType } from './ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import ContactManagerView from './ContactManagerView/ContactManagerView';

export interface IContactManagerModalProps {
  display?: boolean;
  setDisplay?: (display: boolean) => void;
  setSelectedRows: (selectedContacts: IContactSearchResult[]) => void;
  selectedRows: IContactSearchResult[];
  showActiveSelector?: boolean;
  handleModalOk?: () => void;
  handleModalCancel?: () => void;
  isSingleSelect?: boolean;
  restrictContactType?: RestrictContactType;
  isSummary?: boolean;
}

export const ContactManagerModal: React.FunctionComponent<
  React.PropsWithChildren<IContactManagerModalProps>
> = props => {
  return (
    <GenericModal
      variant="info"
      display={props.display}
      headerIcon={<FaUser size={20} />}
      setDisplay={props.setDisplay}
      title="Select Contact"
      message={
        <>
          <p>
            Individuals and contacts must already be in the Contact Manager and be an active contact
            to be found in this search.
          </p>
          <ContactManagerView
            setSelectedRows={props.setSelectedRows}
            selectedRows={props.selectedRows}
            showActiveSelector={props.showActiveSelector}
            noInitialSearch={props.selectedRows.length === 0}
            isSingleSelect={props.isSingleSelect}
            restrictContactType={props.restrictContactType}
            isSummary={props.isSummary ?? true}
            showSelectedRowCount
          />
        </>
      }
      okButtonText="Select"
      cancelButtonText="Cancel"
      handleOk={props.handleModalOk}
      handleCancel={props.handleModalCancel}
      modalSize={ModalSize.XLARGE}
    ></GenericModal>
  );
};
