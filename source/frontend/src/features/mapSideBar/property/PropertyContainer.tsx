import React, { useState } from 'react';

import { usePropertyDetails } from '@/features/mapSideBar/hooks/usePropertyDetails';
import {
  InventoryTabNames,
  InventoryTabs,
  TabInventoryView,
} from '@/features/mapSideBar/property/InventoryTabs';
import BcAssessmentTabView from '@/features/mapSideBar/property/tabs/bcAssessment/BcAssessmentTabView';
import LtsaTabView from '@/features/mapSideBar/property/tabs/ltsa/LtsaTabView';
import PropertyAssociationTabView from '@/features/mapSideBar/property/tabs/propertyAssociations/PropertyAssociationTabView';
import { PropertyDetailsTabView } from '@/features/mapSideBar/property/tabs/propertyDetails/detail/PropertyDetailsTabView';
import ComposedPropertyState from '@/hooks/repositories/useComposedProperties';

import { PropertyManagementTabView } from './tabs/propertyDetailsManagement/detail/PropertyManagementTabView';

export interface IPropertyContainerProps {
  composedPropertyState: ComposedPropertyState;
  setEditMode: (isEditing: boolean) => void;
}

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const PropertyContainer: React.FunctionComponent<
  React.PropsWithChildren<IPropertyContainerProps>
> = ({ composedPropertyState, setEditMode }) => {
  const showPropertyInfoTab = composedPropertyState?.id !== undefined;

  const tabViews: TabInventoryView[] = [];

  tabViews.push({
    content: (
      <LtsaTabView
        ltsaData={composedPropertyState.ltsaWrapper?.response}
        ltsaRequestedOn={composedPropertyState.ltsaWrapper?.requestedOn}
        loading={composedPropertyState.ltsaWrapper?.loading ?? false}
        pid={
          composedPropertyState?.pid?.toString() ??
          composedPropertyState?.apiWrapper?.response?.pid?.toString()
        }
      />
    ),
    key: InventoryTabNames.title,
    name: 'Title',
  });

  tabViews.push({
    content: (
      <BcAssessmentTabView
        summaryData={composedPropertyState.bcAssessmentWrapper?.response}
        requestedOn={composedPropertyState.bcAssessmentWrapper?.requestedOn}
        loading={composedPropertyState.bcAssessmentWrapper?.loading ?? false}
        pid={
          composedPropertyState?.pid?.toString() ??
          composedPropertyState?.apiWrapper?.response?.pid?.toString()
        }
      />
    ),
    key: InventoryTabNames.value,
    name: 'Value',
  });

  var defaultTab = InventoryTabNames.title;

  // TODO: PSP-4406 this should have a loading flag
  const propertyViewForm = usePropertyDetails(composedPropertyState.apiWrapper?.response);

  if (showPropertyInfoTab) {
    // After API property object has been received, we query relevant map layers to find
    // additional information which we store in a different model (IPropertyDetailsForm)

    tabViews.push({
      content: (
        <PropertyDetailsTabView
          property={propertyViewForm}
          loading={composedPropertyState.apiWrapper?.loading ?? false}
          setEditMode={setEditMode}
        />
      ),
      key: InventoryTabNames.property,
      name: 'Property Details',
    });
    defaultTab = InventoryTabNames.property;
  }

  if (composedPropertyState.propertyAssociationWrapper?.response?.id !== undefined) {
    tabViews.push({
      content: (
        <PropertyAssociationTabView
          isLoading={composedPropertyState.propertyAssociationWrapper?.loading}
          associations={composedPropertyState.propertyAssociationWrapper?.response}
        />
      ),
      key: InventoryTabNames.pims,
      name: 'PIMS Files',
    });
  }

  if (composedPropertyState.apiWrapper?.response !== undefined && showPropertyInfoTab) {
    // After API property object has been received, we query relevant map layers to find
    // additional information which we store in a different model (IPropertyDetailsForm)

    tabViews.push({
      content: (
        <PropertyManagementTabView
          property={composedPropertyState.apiWrapper?.response}
          loading={composedPropertyState.apiWrapper?.loading ?? false}
          setEditMode={setEditMode}
        />
      ),
      key: InventoryTabNames.management,
      name: 'Management',
    });
    defaultTab = InventoryTabNames.management;
  }

  const [activeTab, setActiveTab] = useState<InventoryTabNames>(defaultTab);

  return (
    <InventoryTabs
      loading={composedPropertyState.composedLoading}
      tabViews={tabViews}
      defaultTabKey={defaultTab}
      activeTab={activeTab}
      setActiveTab={setActiveTab}
    />
  );
};

export default PropertyContainer;
