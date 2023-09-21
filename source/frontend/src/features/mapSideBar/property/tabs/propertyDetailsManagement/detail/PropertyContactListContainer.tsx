import { useCallback, useEffect, useState } from 'react';

import { usePropertyContactRepository } from '@/hooks/repositories/usePropertyContactRepository';
import { Api_PropertyContact } from '@/models/api/Property';

import { IPropertyContactListViewProps } from './PropertyContactListView';

interface IPropertyContactListContainerProps {
  propertyId: number;
  setEditMode: (isEditing: boolean) => void;
  View: React.FC<IPropertyContactListViewProps>;
}

export const PropertyContactListContainer: React.FunctionComponent<
  IPropertyContactListContainerProps
> = ({ propertyId, setEditMode, View }) => {
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
    async (contactId: number) => {
      const result = await deleteContact(propertyId, contactId);
      if (result === true) {
        fetchPropertyContacts();
      }
    },
    [deleteContact, fetchPropertyContacts, propertyId],
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
