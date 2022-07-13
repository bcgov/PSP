import axios, { AxiosError } from 'axios';
import { usePropertyDetails } from 'features/mapSideBar/hooks/usePropertyDetails';
import {
  InventoryTabNames,
  InventoryTabs,
  TabInventoryView,
} from 'features/mapSideBar/tabs/InventoryTabs';
import LtsaTabView from 'features/mapSideBar/tabs/ltsa/LtsaTabView';
import PropertyAssociationTabView from 'features/mapSideBar/tabs/propertyAssociations/PropertyAssociationTabView';
import { PropertyDetailsTabView } from 'features/mapSideBar/tabs/propertyDetails/detail/PropertyDetailsTabView';
import useIsMounted from 'hooks/useIsMounted';
import { useLtsa } from 'hooks/useLtsa';
import { useProperties } from 'hooks/useProperties';
import { usePropertyAssociations } from 'hooks/usePropertyAssociations';
import { IApiError } from 'interfaces/IApiError';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { Api_PropertyAssociations } from 'models/api/Property';
import React, { useEffect, useState } from 'react';
import { pidFormatter } from 'utils';
import ComposedProperty from './ComposedProperty';

export interface IPropertyContainerProps {
  composedProperty: ComposedProperty;
}

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const PropertyContainer: React.FunctionComponent<IPropertyContainerProps> = ({
  composedProperty,
}) => {
  /*if (isMounted() && propInfo.pid === pidFormatter(pid)) {
    setApiProperty(propInfo);
    setShowPropertyInfoTab(true);
  }*/
  //const [showPropertyInfoTab, setShowPropertyInfoTab] = useState(true);

  const showPropertyInfoTab = composedProperty.apiProperty !== undefined;
  const activeTab = InventoryTabNames.property;

  const tabViews: TabInventoryView[] = [];

  tabViews.push({
    content: (
      <LtsaTabView
        ltsaData={composedProperty.ltsaData}
        ltsaRequestedOn={composedProperty.ltsaDataRequestedOn}
        loading={composedProperty.ltsaLoading}
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

  if (showPropertyInfoTab) {
    // After API property object has been received, we query relevant map layers to find
    // additional information which we store in a different model (IPropertyDetailsForm)
    const propertyViewForm = usePropertyDetails(composedProperty.apiProperty);

    tabViews.push({
      content: <PropertyDetailsTabView property={propertyViewForm} loading={propertyLoading} />,
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
