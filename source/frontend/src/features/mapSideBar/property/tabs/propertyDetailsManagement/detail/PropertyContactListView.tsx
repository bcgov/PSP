import { FaUserPlus } from 'react-icons/fa';
import { generatePath, useHistory, useRouteMatch } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import Claims from '@/constants/claims';
import { ApiGen_Concepts_PropertyContact } from '@/models/api/generated/ApiGen_Concepts_PropertyContact';
import { exists } from '@/utils';

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
  const matchProperty = useRouteMatch<{ propertyId: string }>();
  const matchPropertyFile = useRouteMatch<{
    id: string;
    menuIndex: string;
    filePropertyId: string;
  }>();
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
            if (exists(matchProperty.params.propertyId)) {
              const path = generatePath(matchProperty.path, {
                propertyId: matchProperty.params.propertyId,
                tab: InventoryTabNames.management,
              });
              history.push(`${path}/${PropertyEditForms.UpdateContactContainer}?edit=true`);
            } else {
              const path = generatePath(matchPropertyFile.path, {
                id: matchPropertyFile.params.id,
                filePropertyId: matchPropertyFile.params.filePropertyId,
                menuIndex: matchPropertyFile.params.menuIndex,
                tab: InventoryTabNames.management,
              });
              history.push(`${path}/${PropertyEditForms.UpdateContactContainer}?edit=true`);
            }
          }}
        />
      }
    >
      <LoadingBackdrop show={isLoading} />
      <PropertyContactList
        propertyContacts={propertyContacts}
        handleEdit={contactId => {
          if (exists(matchProperty.params.propertyId)) {
            const path = generatePath(matchProperty.path, {
              propertyId: matchProperty.params.propertyId,
              tab: InventoryTabNames.management,
            });
            history.push(
              `${path}/${PropertyEditForms.UpdateContactContainer}/${contactId}?edit=true`,
            );
          } else {
            const path = generatePath(matchPropertyFile.path, {
              id: matchPropertyFile.params.id,
              menuIndex: matchPropertyFile.params.menuIndex,
              tab: InventoryTabNames.management,
            });
            history.push(
              `${path}/${PropertyEditForms.UpdateContactContainer}/${contactId}?edit=true`,
            );
          }
        }}
        handleDelete={onDelete}
      />
    </Section>
  );
};
