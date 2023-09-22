import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import Claims from '@/constants/claims';
import { Api_PropertyContact } from '@/models/api/Property';

import { EditManagementState, PropertyEditForms } from '../../../PropertyViewSelector';
import PropertyContactList from './PropertyContactList';

export interface IPropertyContactListViewProps {
  isLoading: boolean;
  propertyContacts: Api_PropertyContact[];
  setEditManagementState: (state: EditManagementState | null) => void;
  onDelete: (contactId: number) => void;
}

export const PropertyContactListView: React.FunctionComponent<IPropertyContactListViewProps> = ({
  isLoading,
  propertyContacts,
  setEditManagementState,
  onDelete,
}) => {
  return (
    <Section
      header={
        <SectionListHeader
          claims={[Claims.PROPERTY_EDIT]}
          title={'Property Contact'}
          addButtonText={'Add a Contact'}
          addButtonIcon={'person'}
          onAdd={() =>
            setEditManagementState({
              form: PropertyEditForms.UpdateContactContainer,
              childId: null,
            })
          }
        />
      }
    >
      <LoadingBackdrop show={isLoading} />
      <PropertyContactList
        propertyContacts={propertyContacts}
        handleEdit={contactId => {
          setEditManagementState({
            form: PropertyEditForms.UpdateContactContainer,
            childId: contactId,
          });
        }}
        handleDelete={onDelete}
      />
    </Section>
  );
};
