import * as React from 'react';
import { useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';

import { FileTypes } from '@/constants/fileTypes';
import { usePropertyDetails } from '@/features/mapSideBar/hooks/usePropertyDetails';
import {
  IInventoryTabsProps,
  InventoryTabNames,
  TabInventoryView,
} from '@/features/mapSideBar/property/InventoryTabs';
import BcAssessmentTabView from '@/features/mapSideBar/property/tabs/bcAssessment/BcAssessmentTabView';
import LtsaTabView from '@/features/mapSideBar/property/tabs/ltsa/LtsaTabView';
import PropertyAssociationTabView from '@/features/mapSideBar/property/tabs/propertyAssociations/PropertyAssociationTabView';
import { PropertyDetailsTabView } from '@/features/mapSideBar/property/tabs/propertyDetails/detail/PropertyDetailsTabView';
import TakesDetailContainer from '@/features/mapSideBar/property/tabs/takes/detail/TakesDetailContainer';
import TakesDetailView from '@/features/mapSideBar/property/tabs/takes/detail/TakesDetailView';
import { PROPERTY_TYPES, useComposedProperties } from '@/hooks/repositories/useComposedProperties';
import { Api_PropertyFile } from '@/models/api/PropertyFile';

export interface IPropertyFileContainerProps {
  fileProperty: Api_PropertyFile;
  setEditFileProperty: () => void;
  setEditTakes: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IInventoryTabsProps>>;
  customTabs: TabInventoryView[];
  defaultTab: InventoryTabNames;
  fileContext?: FileTypes;
  withRouter?: boolean;
}

export const PropertyFileContainer: React.FunctionComponent<
  React.PropsWithChildren<IPropertyFileContainerProps>
> = props => {
  const pid = props.fileProperty?.property?.pid;
  const id = props.fileProperty?.property?.id;

  const composedProperties = useComposedProperties({
    pid,
    id,
    propertyTypes: [
      PROPERTY_TYPES.ASSOCIATIONS,
      PROPERTY_TYPES.LTSA,
      PROPERTY_TYPES.PIMS_API,
      PROPERTY_TYPES.BC_ASSESSMENT,
    ],
  });

  // After API property object has been received, we query relevant map layers to find
  // additional information which we store in a different model (IPropertyDetailsForm)
  const propertyViewForm = usePropertyDetails(composedProperties.apiWrapper?.response);

  const tabViews: TabInventoryView[] = [];
  const ltsaWrapper = composedProperties.ltsaWrapper;

  tabViews.push({
    content: (
      <LtsaTabView
        ltsaData={ltsaWrapper?.response}
        ltsaRequestedOn={ltsaWrapper?.requestedOn}
        loading={ltsaWrapper?.loading ?? false}
        pid={pid?.toString()}
      />
    ),
    key: InventoryTabNames.title,
    name: 'Title',
  });
  tabViews.push({
    content: (
      <BcAssessmentTabView
        summaryData={composedProperties.bcAssessmentWrapper?.response}
        requestedOn={composedProperties.bcAssessmentWrapper?.requestedOn}
        loading={composedProperties.bcAssessmentWrapper?.loading ?? false}
        pid={pid?.toString()}
      />
    ),
    key: InventoryTabNames.value,
    name: 'Value',
  });

  tabViews.push(...props.customTabs);

  if (!!id) {
    tabViews.push({
      content: (
        <PropertyDetailsTabView
          property={propertyViewForm}
          loading={composedProperties.composedLoading ?? false}
          setEditMode={editable => {
            props.setEditFileProperty();
          }}
        />
      ),
      key: InventoryTabNames.property,
      name: 'Property Details',
    });
  }
  if (!!id) {
    tabViews.push({
      content: (
        <PropertyAssociationTabView
          isLoading={composedProperties.propertyAssociationWrapper?.loading ?? false}
          associations={composedProperties.propertyAssociationWrapper?.response}
        />
      ),
      key: InventoryTabNames.pims,
      name: 'PIMS Files',
    });
  }

  if (props.fileContext === FileTypes.Acquisition) {
    tabViews.push({
      content: (
        <TakesDetailContainer
          fileProperty={props.fileProperty}
          onEdit={props.setEditTakes}
          View={TakesDetailView}
        ></TakesDetailContainer>
      ),
      key: InventoryTabNames.takes,
      name: 'Takes',
    });
  }

  const InventoryTabsView = props.View;
  let activeTab: InventoryTabNames;
  let setActiveTab: (tab: InventoryTabNames) => void;

  // Use state-based tabs OR route-based tabs (as passed in the 'withRouter' property)
  const [activeTabState, setActiveTabState] = useState<InventoryTabNames>(props.defaultTab);
  const history = useHistory();
  const params = useParams<{ tab?: string }>();

  if (!!props.withRouter) {
    activeTab = Object.values(InventoryTabNames).find(t => t === params.tab) ?? props.defaultTab;
    setActiveTab = (tab: InventoryTabNames) => {
      if (activeTab !== tab) {
        history.push(`${tab}`);
      }
    };
  } else {
    activeTab = activeTabState;
    setActiveTab = setActiveTabState;
  }

  return (
    <InventoryTabsView
      loading={composedProperties.composedLoading}
      tabViews={tabViews}
      defaultTabKey={props.defaultTab}
      activeTab={activeTab}
      setActiveTab={setActiveTab}
    />
  );
};

export default PropertyFileContainer;
