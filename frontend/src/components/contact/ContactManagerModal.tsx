import GenericModal, { ModalSize } from 'components/common/GenericModal';
import { IContactSearchResult } from 'interfaces';

import ContactManagerView from './ContactManagerView/ContactManagerView';

export interface ILeasePropertiesProps {
  setSelectedRows: (selectedContacts: IContactSearchResult[]) => void;
  selectedRows: IContactSearchResult[];
  showAddButton?: boolean;
  showActiveSelector?: boolean;
  handleModalOk?: Function;
  handleModalCancel?: Function;
}

export const ContactManagerModal: React.FunctionComponent<ILeasePropertiesProps> = props => {
  return (
    <GenericModal
      title="Select a contact"
      message={
        <>
          <p>
            Individuals and contacts must already be in the Contact Manager and be an active contact
            to be found in this search.
          </p>
          <ContactManagerView
            setSelectedRows={props.setSelectedRows}
            selectedRows={props.selectedRows}
            showAddButton={props.showAddButton}
            showActiveSelector={props.showActiveSelector}
            noInitialSearch
            isSummary
            showSelectedRowCount
          />
        </>
      }
      okButtonText="Select"
      cancelButtonText="Cancel"
      handleOk={props.handleModalOk}
      handleCancel={props.handleModalCancel}
      size={ModalSize.XLARGE}
    ></GenericModal>
  );
};
