import { useFormikContext } from 'formik';
import { IContactSearchResult } from 'interfaces/IContactSearchResult';
import React, { useState } from 'react';

import { IContactInputViewProps } from './ContactInputView';

export type IContactInputContainerProps = {
  field: string;
  label?: string;
};

export const ContactInputContainer: React.FC<
  React.PropsWithChildren<
    IContactInputContainerProps & {
      View: React.FunctionComponent<React.PropsWithChildren<IContactInputViewProps>>;
    }
  >
> = ({ field, View, label }) => {
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
      displayErrorTooltips={true}
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
        showOnlyIndividuals: true,
      }}
    />
  );
};
