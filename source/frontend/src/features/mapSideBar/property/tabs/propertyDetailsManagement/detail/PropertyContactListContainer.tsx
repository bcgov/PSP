import { useCallback, useEffect, useState } from 'react';

import { usePropertyContactRepository } from '@/hooks/repositories/usePropertyContactRepository';
import { ApiGen_Concepts_PropertyContact } from '@/models/api/generated/ApiGen_Concepts_PropertyContact';

import { IPropertyContactListViewProps } from './PropertyContactListView';

interface IPropertyContactListContainerProps {
  propertyId: number;
  View: React.FC<IPropertyContactListViewProps>;
}

export const PropertyContactListContainer: React.FunctionComponent<
  IPropertyContactListContainerProps
> = ({ propertyId, View }) => {
  const [propertyContacts, setPropertyContacts] = useState<ApiGen_Concepts_PropertyContact[]>([]);

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
      onDelete={onDelete}
    />
  );
};
