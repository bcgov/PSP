import { useFormikContext } from 'formik';
import { useState } from 'react';

import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import { IContactSearchResult } from '@/interfaces/IContactSearchResult';

import { IContactInputViewProps } from './ContactInputView';

export type IContactInputContainerProps = {
  field: string;
  label?: string;
  restrictContactType?: RestrictContactType;
  displayErrorAsTooltip?: boolean;
  onContactSelected?: (contact: IContactSearchResult) => void;
  placeholder?: string;
};

export const ContactInputContainer: React.FC<
  React.PropsWithChildren<
    IContactInputContainerProps & {
      View: React.FunctionComponent<React.PropsWithChildren<IContactInputViewProps>>;
    }
  >
> = ({
  field,
  View,
  label,
  restrictContactType,
  displayErrorAsTooltip = true,
  onContactSelected,
  placeholder,
}) => {
  const [showContactManager, setShowContactManager] = useState(false);
  const [selectedContacts, setSelectedContacts] = useState<IContactSearchResult[]>([]);
  const { setFieldValue, setFieldTouched } = useFormikContext<any>();

  const handleContactManagerOk = () => {
    setFieldValue(field, selectedContacts[0]);
    setShowContactManager(false);
    setSelectedContacts([]);
    if (onContactSelected !== undefined) {
      onContactSelected(selectedContacts[0]);
    }
  };

  return (
    <View
      field={field}
      onClear={() => {
        setFieldValue(field, null);
        setFieldTouched(field);
      }}
      label={label}
      displayErrorTooltips={displayErrorAsTooltip}
      setShowContactManager={() => {
        setShowContactManager(true);
      }}
      contactManagerProps={{
        selectedRows: selectedContacts,
        setSelectedRows: setSelectedContacts,
        display: showContactManager,
        setDisplay: setShowContactManager,
        isSingleSelect: true,
        handleModalOk: handleContactManagerOk,
        handleModalCancel: () => {
          setShowContactManager(false);
          setSelectedContacts([]);
        },
        showActiveSelector: true,
        restrictContactType: restrictContactType,
      }}
      placeholder={placeholder}
    />
  );
};
