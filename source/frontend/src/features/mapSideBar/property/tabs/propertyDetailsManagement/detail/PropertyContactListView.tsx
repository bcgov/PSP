import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { Api_PropertyContact } from '@/models/api/Property';

import PropertyContactList from './PropertyContactList';

export interface IPropertyContactListViewProps {
  isLoading: boolean;
  propertyContacts: Api_PropertyContact[];
  setEditMode: (isEditing: boolean) => void;
  onDelete: (contactId: number) => void;
}

export const PropertyContactListView: React.FunctionComponent<IPropertyContactListViewProps> = ({
  isLoading,
  propertyContacts,
  setEditMode,
  onDelete,
}) => {
  return (
    <Section header="Property Contact">
      <LoadingBackdrop show={isLoading} />
      <PropertyContactList
        propertyContacts={propertyContacts}
        handleEdit={() => {
          //TODO
        }}
        handleDelete={onDelete}
      />
    </Section>
  );
};
