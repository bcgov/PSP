import { FileTypes } from 'constants/fileTypes';
import { usePropertyDetails } from 'features/mapSideBar/hooks/usePropertyDetails';
import BcAssessmentTabView from 'features/mapSideBar/tabs/bcAssessment/BcAssessmentTabView';
import {
  IInventoryTabsProps,
  InventoryTabNames,
  TabInventoryView,
} from 'features/mapSideBar/tabs/InventoryTabs';
import LtsaTabView from 'features/mapSideBar/tabs/ltsa/LtsaTabView';
import PropertyAssociationTabView from 'features/mapSideBar/tabs/propertyAssociations/PropertyAssociationTabView';
import { PropertyDetailsTabView } from 'features/mapSideBar/tabs/propertyDetails/detail/PropertyDetailsTabView';
import TakesDetailContainer from 'features/mapSideBar/tabs/takes/detail/TakesDetailContainer';
import TakesDetailView from 'features/mapSideBar/tabs/takes/detail/TakesDetailView';
import { PROPERTY_TYPES, useComposedProperties } from 'hooks/useComposedProperties';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import * as React from 'react';
import { useState } from 'react';

export interface IPropertyFileContainerProps {
  fileProperty: Api_PropertyFile;
  setEditFileProperty: () => void;
  setEditTakes: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IInventoryTabsProps>>;
  customTabs: TabInventoryView[];
  defaultTab: InventoryTabNames;
  fileContext?: FileTypes;
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

  const [activeTab, setActiveTab] = useState<InventoryTabNames>(props.defaultTab);
  const InventoryTabsView = props.View;

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
