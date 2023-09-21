import { Section } from '@/components/common/Section/Section';
import { Api_PropertyContact } from '@/models/api/Property';

import PropertyContactList from './PropertyContactList';

export interface IPropertyContactViewProps {
  isLoading: boolean;
  propertyContacts: Api_PropertyContact[];
  setEditMode: (isEditing: boolean) => void;
  onDelete: (contactId: number) => void;
}

export const PropertyContactView: React.FunctionComponent<IPropertyContactViewProps> = ({
  isLoading,
  propertyContacts,
  setEditMode,
  onDelete,
}) => {
  return (
    <Section header="Property Contact">
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
