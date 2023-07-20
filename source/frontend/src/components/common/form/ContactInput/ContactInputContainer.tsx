import { useFormikContext } from 'formik';
import React, { useState } from 'react';

import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import { IContactSearchResult } from '@/interfaces/IContactSearchResult';

import { IContactInputViewProps } from './ContactInputView';

export type IContactInputContainerProps = {
  field: string;
  label?: string;
  restrictContactType?: RestrictContactType;
  displayErrorAsTooltip?: boolean;
};

export const ContactInputContainer: React.FC<
  React.PropsWithChildren<
    IContactInputContainerProps & {
      View: React.FunctionComponent<React.PropsWithChildren<IContactInputViewProps>>;
    }
  >
> = ({ field, View, label, restrictContactType, displayErrorAsTooltip = true }) => {
  const [showContactManager, setShowContactManager] = useState(false);
  const [selectedContacts, setSelectedContacts] = useState<IContactSearchResult[]>([]);
  const { setFieldValue } = useFormikContext<any>();

  const handleContactManagerOk = () => {
    setFieldValue(field, selectedContacts[0]);
    setShowContactManager(false);
    setSelectedContacts([]);
  };

  return (
    <View
      field={field}
      onClear={() => setFieldValue(field, null)}
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
    />
  );
};
