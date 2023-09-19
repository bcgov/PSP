import { useCallback, useEffect, useState } from 'react';

import { usePropertyContactRepository } from '@/hooks/repositories/usePropertyContactRepository';
import { Api_PropertyContact } from '@/models/api/Property';

import { IPropertyContactViewProps } from './PropertyContactView';

interface IPropertyContactContainerProps {
  propertyId: number;
  setEditMode: (isEditing: boolean) => void;
  View: React.FC<IPropertyContactViewProps>;
}

export const PropertyContactContainer: React.FunctionComponent<IPropertyContactContainerProps> = ({
  propertyId,
  setEditMode,
  View,
}) => {
  const [propertyContacts, setPropertyContacts] = useState<Api_PropertyContact[]>([]);

  const {
    getPropertyContacts: { execute: getContacts, loading },
    deletePropertyContact: { execute: deleteContact, loading: loadingDelete },
  } = usePropertyContactRepository();

  const fetchPropertyContacts = useCallback(async () => {
    const propertyContactsResponse = await getContacts(propertyId);
    if (propertyContactsResponse) {
      setPropertyContacts(propertyContactsResponse);
    }
  }, [getContacts, propertyId]);

  useEffect(() => {
    fetchPropertyContacts();
  }, [fetchPropertyContacts]);

  const onDelete = useCallback(
    (contactId: number) => {
      deleteContact(propertyId, contactId);
    },
    [deleteContact, propertyId],
  );

  return (
    <View
      isLoading={loading || loadingDelete}
      propertyContacts={propertyContacts}
      setEditMode={setEditMode}
      onDelete={onDelete}
    />
  );
};
