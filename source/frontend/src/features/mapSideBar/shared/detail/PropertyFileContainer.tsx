import { useParams } from 'react-router-dom';

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
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';
import { isValidId } from '@/utils';

import PropertyResearchTabView from '../../property/tabs/propertyResearch/detail/PropertyResearchTabView';

export interface IPropertyFileContainerProps {
  fileProperty: ApiGen_Concepts_FileProperty;
  setEditing: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IInventoryTabsProps>>;
  customTabs: TabInventoryView[];
  defaultTab: InventoryTabNames;
  fileContext?: FileTypes;
}

export const PropertyFileContainer: React.FunctionComponent<
  React.PropsWithChildren<IPropertyFileContainerProps>
> = props => {
  const pid = props.fileProperty?.property?.pid ?? undefined;
  const id = props.fileProperty?.property?.id ?? undefined;

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

  if (props.fileContext === FileTypes.Research) {
    tabViews.push({
      content: (
        <PropertyResearchTabView
          researchFile={props.fileProperty as ApiGen_Concepts_ResearchFileProperty}
          setEditMode={props.setEditing}
        />
      ),
      key: InventoryTabNames.research,
      name: 'Property Research',
    });
  }

  tabViews.push(...props.customTabs);

  if (isValidId(id)) {
    tabViews.push({
      content: (
        <PropertyDetailsTabView
          property={propertyViewForm}
          loading={composedProperties.composedLoading ?? false}
        />
      ),
      key: InventoryTabNames.property,
      name: 'Property Details',
    });
  }
  if (isValidId(id)) {
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
          onEdit={props.setEditing}
          View={TakesDetailView}
        ></TakesDetailContainer>
      ),
      key: InventoryTabNames.takes,
      name: 'Takes',
    });
  }

  const InventoryTabsView = props.View;

  const params = useParams<{ tab?: string }>();
  const activeTab =
    Object.values(InventoryTabNames).find(t => t === params.tab) ?? props.defaultTab;

  return (
    <InventoryTabsView
      loading={composedProperties.composedLoading}
      tabViews={tabViews}
      defaultTabKey={props.defaultTab}
      activeTab={activeTab}
    />
  );
};

export default PropertyFileContainer;
