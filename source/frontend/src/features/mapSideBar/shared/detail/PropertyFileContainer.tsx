import { useMemo, useState } from 'react';
import { useParams } from 'react-router-dom';

import { Claims } from '@/constants';
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
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { useLeaseStakeholderRepository } from '@/hooks/repositories/useLeaseStakeholderRepository';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';
import { exists, getLatLng, isValidId } from '@/utils';

import { getLeaseInfo, LeaseAssociationInfo } from '../../property/PropertyContainer';
import CrownDetailsTabView from '../../property/tabs/crown/CrownDetailsTabView';
import PropertyResearchTabView from '../../property/tabs/propertyResearch/detail/PropertyResearchTabView';

export interface IPropertyFileContainerProps {
  fileProperty: ApiGen_Concepts_FileProperty;
  setEditing: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IInventoryTabsProps>>;
  customTabs: TabInventoryView[];
  defaultTab: InventoryTabNames;
  fileContext?: ApiGen_CodeTypes_FileTypes;
}

export const PropertyFileContainer: React.FunctionComponent<
  IPropertyFileContainerProps
> = props => {
  const pid = props.fileProperty?.property?.pid ?? undefined;
  const id = props.fileProperty?.property?.id ?? undefined;
  const location = props.fileProperty?.property?.location ?? undefined;
  const latLng = useMemo(() => getLatLng(location) ?? undefined, [location]);
  const { hasClaim } = useKeycloakWrapper();

  const composedProperties = useComposedProperties({
    pid,
    id,
    latLng,
    propertyTypes: [
      PROPERTY_TYPES.ASSOCIATIONS,
      PROPERTY_TYPES.LTSA,
      PROPERTY_TYPES.PIMS_API,
      PROPERTY_TYPES.BC_ASSESSMENT,
      PROPERTY_TYPES.CROWN_TENURES,
    ],
  });

  const { getLease } = useLeaseRepository();
  const { getLeaseStakeholders } = useLeaseStakeholderRepository();
  const { getLeaseRenewals } = useLeaseRepository();
  const [LeaseAssociationInfo, setLeaseAssociationInfo] = useState<LeaseAssociationInfo>({
    leaseDetails: [],
    leaseStakeholders: [],
    leaseRenewals: [],
    loading: false,
  });

  const leaseAssociations =
    composedProperties?.propertyAssociationWrapper?.response?.leaseAssociations;
  useMemo(
    () =>
      hasClaim(Claims.LEASE_VIEW)
        ? getLeaseInfo(
            leaseAssociations,
            getLease.execute,
            getLeaseStakeholders.execute,
            getLeaseRenewals.execute,
            setLeaseAssociationInfo,
          )
        : null,
    [
      leaseAssociations,
      getLease.execute,
      getLeaseStakeholders.execute,
      getLeaseRenewals.execute,
      hasClaim,
    ],
  );

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

  if (exists(composedProperties.composedProperty?.crownTenureFeature)) {
    tabViews.push({
      content: (
        <CrownDetailsTabView
          crownFeature={composedProperties.composedProperty?.crownTenureFeature}
        />
      ),
      key: InventoryTabNames.crown,
      name: 'Crown',
    });
  }

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

  if (props.fileContext === ApiGen_CodeTypes_FileTypes.Research) {
    tabViews.push({
      content: (
        <PropertyResearchTabView
          researchFileProperty={props.fileProperty as ApiGen_Concepts_ResearchFileProperty}
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
          loading={composedProperties?.apiWrapper?.loading ?? false}
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
          isLoading={
            composedProperties.propertyAssociationWrapper?.loading ??
            LeaseAssociationInfo.loading ??
            false
          }
          associations={composedProperties.propertyAssociationWrapper?.response}
          associatedLeaseStakeholders={LeaseAssociationInfo.leaseStakeholders}
          associatedLeaseRenewals={LeaseAssociationInfo.leaseRenewals}
          associatedLeases={LeaseAssociationInfo.leaseDetails}
        />
      ),
      key: InventoryTabNames.pims,
      name: 'PIMS Files',
    });
  }

  if (props.fileContext === ApiGen_CodeTypes_FileTypes.Acquisition) {
    tabViews.push({
      content: (
        <TakesDetailContainer
          fileProperty={props.fileProperty}
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
