import { usePropertyDetails } from 'features/mapSideBar/hooks/usePropertyDetails';
import {
  InventoryTabNames,
  InventoryTabs,
  TabInventoryView,
} from 'features/mapSideBar/tabs/InventoryTabs';
import LtsaTabView from 'features/mapSideBar/tabs/ltsa/LtsaTabView';
import PropertyAssociationTabView from 'features/mapSideBar/tabs/propertyAssociations/PropertyAssociationTabView';
import { PropertyDetailsTabView } from 'features/mapSideBar/tabs/propertyDetails/detail/PropertyDetailsTabView';
import React, { useState } from 'react';

import ComposedProperty from './ComposedProperty';

export interface IPropertyContainerProps {
  composedProperty: ComposedProperty;
  setEditMode: (isEditing: boolean) => void;
}

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const PropertyContainer: React.FunctionComponent<IPropertyContainerProps> = ({
  composedProperty,
  setEditMode,
}) => {
  const showPropertyInfoTab = composedProperty.apiProperty !== undefined;

  const tabViews: TabInventoryView[] = [];

  tabViews.push({
    content: (
      <LtsaTabView
        ltsaData={composedProperty.ltsaData}
        ltsaRequestedOn={composedProperty.ltsaDataRequestedOn}
        loading={composedProperty.ltsaLoading}
        pid={composedProperty?.pid ?? composedProperty?.apiProperty?.pid}
      />
    ),
    key: InventoryTabNames.title,
    name: 'Title',
  });

  tabViews.push({
    content: <></>,
    key: InventoryTabNames.value,
    name: 'Value',
  });

  var defaultTab = InventoryTabNames.title;

  // TODO: PSP-4406 this should have a loading flag
  const propertyViewForm = usePropertyDetails(composedProperty.apiProperty);

  if (showPropertyInfoTab) {
    // After API property object has been received, we query relevant map layers to find
    // additional information which we store in a different model (IPropertyDetailsForm)

    tabViews.push({
      content: (
        <PropertyDetailsTabView
          property={propertyViewForm}
          loading={composedProperty.apiPropertyLoading}
          setEditMode={setEditMode}
        />
      ),
      key: InventoryTabNames.property,
      name: 'Property Details',
    });
    defaultTab = InventoryTabNames.property;
  }

  if (composedProperty.propertyAssociations?.id !== undefined) {
    tabViews.push({
      content: (
        <PropertyAssociationTabView
          isLoading={composedProperty.propertyAssociationsLoading}
          associations={composedProperty.propertyAssociations}
        />
      ),
      key: InventoryTabNames.pims,
      name: 'PIMS Files',
    });
  }

  const [activeTab, setActiveTab] = useState<InventoryTabNames>(defaultTab);

  return (
    <InventoryTabs
      tabViews={tabViews}
      defaultTabKey={defaultTab}
      activeTab={activeTab}
      setActiveTab={setActiveTab}
    />
  );
};

export default PropertyContainer;
