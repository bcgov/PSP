import React, { useMemo, useState } from 'react';
import { useParams } from 'react-router-dom';

import { Claims } from '@/constants';
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
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { useLeaseStakeholderRepository } from '@/hooks/repositories/useLeaseStakeholderRepository';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_Association } from '@/models/api/generated/ApiGen_Concepts_Association';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseRenewal } from '@/models/api/generated/ApiGen_Concepts_LeaseRenewal';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { exists, isValidId } from '@/utils';

import CrownDetailsTabView from './tabs/crown/CrownDetailsTabView';
import { PropertyManagementTabView } from './tabs/propertyDetailsManagement/detail/PropertyManagementTabView';

export interface IPropertyContainerProps {
  composedPropertyState: ComposedPropertyState;
}

export interface LeaseAssociationInfo {
  leaseDetails: ApiGen_Concepts_Lease[];
  leaseStakeholders: ApiGen_Concepts_LeaseStakeholder[];
  leaseRenewals: ApiGen_Concepts_LeaseRenewal[];
  loading: boolean;
}

export const getLeaseInfo = async (
  leasesAssociations: ApiGen_Concepts_Association[],
  getLease: (leaseId: number) => Promise<ApiGen_Concepts_Lease>,
  getLeaseStakeholders: (leaseId: number) => Promise<ApiGen_Concepts_LeaseStakeholder[]>,
  getLeaseRenewals: (leaseId: number) => Promise<ApiGen_Concepts_LeaseRenewal[]>,
  setLeaseAssociationInfo: (info) => void,
) => {
  if (!leasesAssociations) return;
  setLeaseAssociationInfo({ leaseDetails: [], leaseStakeholders: [], loading: true });
  const leaseDetailPromises = leasesAssociations?.map(leaseAssociation =>
    getLease(leaseAssociation.id),
  );
  const leaseStakeholderPromises = leasesAssociations?.map(leaseAssociation =>
    getLeaseStakeholders(leaseAssociation.id),
  );
  const leaseRenewalPromises = leasesAssociations?.map(leaseAssociation =>
    getLeaseRenewals(leaseAssociation.id),
  );

  const leaseDetails = (await Promise.all(leaseDetailPromises)).flat();
  const leaseStakeholders = (await Promise.all(leaseStakeholderPromises)).flat();
  const leaseRenewals = (await Promise.all(leaseRenewalPromises)).flat();
  setLeaseAssociationInfo({
    leaseDetails: leaseDetails,
    leaseStakeholders: leaseStakeholders,
    leaseRenewals: leaseRenewals,
    loading: false,
  });
};

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const PropertyContainer: React.FunctionComponent<IPropertyContainerProps> = ({
  composedPropertyState,
}) => {
  const showPropertyInfoTab = isValidId(composedPropertyState?.id);
  const { hasClaim } = useKeycloakWrapper();
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
    composedPropertyState?.propertyAssociationWrapper?.response?.leaseAssociations;
  useMemo(
    () =>
      getLeaseInfo(
        leaseAssociations,
        getLease.execute,
        getLeaseStakeholders.execute,
        getLeaseRenewals.execute,
        setLeaseAssociationInfo,
      ),
    [
      setLeaseAssociationInfo,
      leaseAssociations,
      getLeaseStakeholders.execute,
      getLeaseRenewals.execute,
      getLease.execute,
    ],
  );

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

  if (exists(composedPropertyState.composedProperty?.crownTenureFeature)) {
    tabViews.push({
      content: (
        <CrownDetailsTabView
          crownFeature={composedPropertyState.composedProperty?.crownTenureFeature}
        />
      ),
      key: InventoryTabNames.crown,
      name: 'Crown',
    });
  }

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

  let defaultTab = InventoryTabNames.title;

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
        />
      ),
      key: InventoryTabNames.property,
      name: 'Property Details',
    });
    defaultTab = InventoryTabNames.property;
  }

  if (isValidId(composedPropertyState.propertyAssociationWrapper?.response?.id)) {
    tabViews.push({
      content: (
        <PropertyAssociationTabView
          isLoading={composedPropertyState.propertyAssociationWrapper!.loading}
          associations={composedPropertyState.propertyAssociationWrapper?.response}
          associatedLeases={LeaseAssociationInfo?.leaseDetails ?? []}
          associatedLeaseStakeholders={LeaseAssociationInfo?.leaseStakeholders ?? []}
          associatedLeaseRenewals={LeaseAssociationInfo?.leaseRenewals ?? []}
        />
      ),
      key: InventoryTabNames.pims,
      name: 'PIMS Files',
    });
  }

  if (
    composedPropertyState.apiWrapper?.response !== undefined &&
    showPropertyInfoTab &&
    hasClaim(Claims.MANAGEMENT_VIEW)
  ) {
    // After API property object has been received, we query relevant map layers to find
    // additional information which we store in a different model (IPropertyDetailsForm)
    tabViews.push({
      content: (
        <PropertyManagementTabView
          property={composedPropertyState.apiWrapper?.response}
          loading={composedPropertyState.apiWrapper?.loading ?? false}
        />
      ),
      key: InventoryTabNames.management,
      name: 'Management',
    });
    defaultTab = InventoryTabNames.management;
  }

  const params = useParams<{ tab?: string }>();
  const activeTab = Object.values(InventoryTabNames).find(t => t === params.tab) ?? defaultTab;
  return (
    <InventoryTabs
      loading={composedPropertyState.composedLoading ?? LeaseAssociationInfo.loading ?? false}
      tabViews={tabViews}
      defaultTabKey={defaultTab}
      activeTab={activeTab}
    />
  );
};

export default PropertyContainer;
