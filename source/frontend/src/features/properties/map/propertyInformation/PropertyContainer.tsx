import { usePropertyDetails } from 'features/mapSideBar/hooks/usePropertyDetails';
import BcAssessmentTabView from 'features/mapSideBar/tabs/bcAssessment/BcAssessmentTabView';
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
export const PropertyContainer: React.FunctionComponent<
  React.PropsWithChildren<IPropertyContainerProps>
> = ({ composedProperty, setEditMode }) => {
  const showPropertyInfoTab = composedProperty?.id !== undefined;

  const tabViews: TabInventoryView[] = [];

  tabViews.push({
    content: (
      <LtsaTabView
        ltsaData={composedProperty.ltsaWrapper?.response}
        ltsaRequestedOn={composedProperty.ltsaWrapper?.requestedOn}
        loading={composedProperty.ltsaWrapper?.loading ?? false}
        pid={
          composedProperty?.pid?.toString() ??
          composedProperty?.apiWrapper?.response?.pid?.toString()
        }
      />
    ),
    key: InventoryTabNames.title,
    name: 'Title',
  });

  tabViews.push({
    content: (
      <BcAssessmentTabView
        summaryData={composedProperty.bcAssessmentWrapper?.response}
        requestedOn={composedProperty.bcAssessmentWrapper?.requestedOn}
        loading={composedProperty.bcAssessmentWrapper?.loading ?? false}
        pid={
          composedProperty?.pid?.toString() ??
          composedProperty?.apiWrapper?.response?.pid?.toString()
        }
      />
    ),
    key: InventoryTabNames.value,
    name: 'Value',
  });

  var defaultTab = InventoryTabNames.title;

  // TODO: PSP-4406 this should have a loading flag
  const propertyViewForm = usePropertyDetails(composedProperty.apiWrapper?.response);

  if (showPropertyInfoTab) {
    // After API property object has been received, we query relevant map layers to find
    // additional information which we store in a different model (IPropertyDetailsForm)

    tabViews.push({
      content: (
        <PropertyDetailsTabView
          property={propertyViewForm}
          loading={composedProperty.apiWrapper?.loading ?? false}
          setEditMode={setEditMode}
        />
      ),
      key: InventoryTabNames.property,
      name: 'Property Details',
    });
    defaultTab = InventoryTabNames.property;
  }

  if (composedProperty.propertyAssociationWrapper?.response?.id !== undefined) {
    tabViews.push({
      content: (
        <PropertyAssociationTabView
          isLoading={composedProperty.propertyAssociationWrapper?.loading}
          associations={composedProperty.propertyAssociationWrapper?.response}
        />
      ),
      key: InventoryTabNames.pims,
      name: 'PIMS Files',
    });
  }

  const [activeTab, setActiveTab] = useState<InventoryTabNames>(defaultTab);

  return (
    <InventoryTabs
      loading={composedProperty.composedLoading}
      tabViews={tabViews}
      defaultTabKey={defaultTab}
      activeTab={activeTab}
      setActiveTab={setActiveTab}
    />
  );
};

export default PropertyContainer;
