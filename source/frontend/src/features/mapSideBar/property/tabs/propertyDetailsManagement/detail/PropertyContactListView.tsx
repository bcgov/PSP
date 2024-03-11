import { FaUserPlus } from 'react-icons/fa';
import { generatePath, useHistory, useRouteMatch } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import Claims from '@/constants/claims';
import { ApiGen_Concepts_PropertyContact } from '@/models/api/generated/ApiGen_Concepts_PropertyContact';

import { InventoryTabNames } from '../../../InventoryTabs';
import { PropertyEditForms } from '../../../PropertyRouter';
import PropertyContactList from './PropertyContactList';

export interface IPropertyContactListViewProps {
  isLoading: boolean;
  propertyContacts: ApiGen_Concepts_PropertyContact[];
  onDelete: (contactId: number) => void;
}

export const PropertyContactListView: React.FunctionComponent<IPropertyContactListViewProps> = ({
  isLoading,
  propertyContacts,
  onDelete,
}) => {
  const history = useHistory();
  const match = useRouteMatch<{ propertyId: string }>();
  return (
    <Section
      isCollapsable
      initiallyExpanded
      header={
        <SectionListHeader
          claims={[Claims.PROPERTY_EDIT]}
          title="Property Contact"
          addButtonText="Add a Contact"
          addButtonIcon={<FaUserPlus size={'2rem'} />}
          onAdd={() => {
            const path = generatePath(match.path, {
              propertyId: match.params.propertyId,
              tab: InventoryTabNames.management,
            });
            history.push(`${path}/${PropertyEditForms.UpdateContactContainer}?edit=true`);
          }}
        />
      }
    >
      <LoadingBackdrop show={isLoading} />
      <PropertyContactList
        propertyContacts={propertyContacts}
        handleEdit={contactId => {
          const path = generatePath(match.path, {
            propertyId: match.params.propertyId,
            tab: InventoryTabNames.management,
          });
          history.push(
            `${path}/${PropertyEditForms.UpdateContactContainer}/${contactId}?edit=true`,
          );
        }}
        handleDelete={onDelete}
      />
    </Section>
  );
};
