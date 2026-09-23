import { useState } from 'react';
import { FaUser } from 'react-icons/fa';

import { PlusButton } from '@/components/common/buttons';
import GenericModal, { ModalSize } from '@/components/common/GenericModal';
import { RestrictContactType } from '@/constants/contacts';
import { IContactSearchResult } from '@/interfaces';

import ContactCreateForm from './ContactManagerView/ContactCreateForm';
import ContactManagerView from './ContactManagerView/ContactManagerView';
import { IContactFilter } from './ContactManagerView/IContactFilter';

export interface IContactManagerModalProps {
  display?: boolean;
  setDisplay?: (display: boolean) => void;
  setSelectedRows: (selectedContacts: IContactSearchResult[]) => void;
  selectedRows: IContactSearchResult[];
  showActiveSelector?: boolean;
  handleModalOk?: () => void;
  handleModalCancel?: () => void;
  isSingleSelect?: boolean;
  restrictContactType?: RestrictContactType[];
  isSummary?: boolean;
}

export const ContactManagerModal: React.FunctionComponent<
  React.PropsWithChildren<IContactManagerModalProps>
> = props => {
  // Handles to show or not the Create New Contact button
  const [isCreatingContact, setIsCreatingContact] = useState(false);
  // Contains the contact created to be added to the results
  const [newContact, setNewContact] = useState<IContactSearchResult>();
  // Preserve the last search filter while ContactManagerView is unmounted.
  const [searchFilter, setSearchFilter] = useState<IContactFilter>();

  const hasValidContactTypes = Boolean(
    props.restrictContactType?.includes(RestrictContactType.ONLY_INDIVIDUALS) &&
      props.restrictContactType?.includes(RestrictContactType.ONLY_ORGANIZATIONS),
  );

  const showCreateContactButton = !isCreatingContact && hasValidContactTypes;

  const resetModalState = () => {
    setSearchFilter(undefined);
    setNewContact(undefined);
    setIsCreatingContact(false);
  };

  const handleModalOk = () => {
    resetModalState();
    props.handleModalOk?.();
  };

  const handleModalCancel = () => {
    resetModalState();
    props.handleModalCancel?.();
  };

  return (
    <GenericModal
      variant="info"
      display={props.display}
      headerIcon={<FaUser size={20} />}
      setDisplay={props.setDisplay}
      title={isCreatingContact ? 'Add contact' : 'Select Contact'}
      message={
        isCreatingContact ? (
          <ContactCreateForm
            onSaved={contact => {
              props.setSelectedRows([contact]);
              setNewContact(contact);
              setIsCreatingContact(false);
            }}
            onCancel={() => setIsCreatingContact(false)}
          />
        ) : (
          <ContactManagerView
            setSelectedRows={props.setSelectedRows}
            selectedRows={props.selectedRows}
            showActiveSelector={props.showActiveSelector}
            noInitialSearch={props.selectedRows.length === 0}
            isSingleSelect={props.isSingleSelect}
            restrictContactType={props.restrictContactType}
            isSummary={props.isSummary ?? true}
            showSelectedRowCount
            createdContact={newContact}
            initialSearchFilter={searchFilter}
            onFilterChanged={setSearchFilter}
          />
        )
      }
      okButtonText={isCreatingContact ? undefined : 'Select'}
      cancelButtonText="Cancel"
      handleOk={handleModalOk}
      handleCancel={isCreatingContact ? () => setIsCreatingContact(false) : handleModalCancel}
      hideFooter={isCreatingContact}
      footerContent={
        showCreateContactButton && (
          <PlusButton
            variant="secondary"
            data-testid="create-new-contact"
            toolId="create-new-contact-tooltip"
            onClick={() => setIsCreatingContact(true)}
          >
            <span>Create New Contact</span>
          </PlusButton>
        )
      }
      modalSize={ModalSize.XLARGE}
    />
  );
};
