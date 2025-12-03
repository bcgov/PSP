import React, { useEffect, useMemo, useState } from 'react';
import { useParams } from 'react-router-dom';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Claims, NoteTypes } from '@/constants';
import {
  InventoryTabNames,
  InventoryTabs,
  TabInventoryView,
} from '@/features/mapSideBar/property/InventoryTabs';
import BcAssessmentTabView from '@/features/mapSideBar/property/tabs/bcAssessment/BcAssessmentTabView';
import LtsaTabView from '@/features/mapSideBar/property/tabs/ltsa/LtsaTabView';
import PropertyAssociationTabView from '@/features/mapSideBar/property/tabs/propertyAssociations/PropertyAssociationTabView';
import { PropertyDetailsTabView } from '@/features/mapSideBar/property/tabs/propertyDetails/detail/PropertyDetailsTabView';
import NoteSummaryContainer from '@/features/notes/list/ManagementNoteSummaryContainer';
import NoteSummaryView from '@/features/notes/list/ManagementNoteSummaryView';
import NoteListContainer from '@/features/notes/list/NoteListContainer';
import NoteListView from '@/features/notes/list/NoteListView';
import ComposedPropertyState from '@/hooks/repositories/useComposedProperties';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { useLeaseStakeholderRepository } from '@/hooks/repositories/useLeaseStakeholderRepository';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_Association } from '@/models/api/generated/ApiGen_Concepts_Association';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseRenewal } from '@/models/api/generated/ApiGen_Concepts_LeaseRenewal';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { exists, firstOrNull, isPlanNumberSPCP, isValidId } from '@/utils';

import { LayerTabCollapsedView } from '../layer/LayerTabCollapsedView';
import { LayerTabContainer } from '../layer/LayerTabContainer';
import { LayerTabView } from '../layer/LayerTabView';
import PropertyDocumentsTab from '../shared/tabs/PropertyDocumentsTab';
import LtsaPlanTabView from './tabs/ltsa/LtsaPlanTabView';
import { toFormValues } from './tabs/propertyDetails/detail/PropertyDetailsTabView.helpers';
import { PropertyManagementTabView } from './tabs/propertyDetailsManagement/detail/PropertyManagementTabView';

export interface IPropertyContainerProps {
  composedPropertyState: ComposedPropertyState;
  onChildSuccess: () => void;
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
  onChildSuccess,
}) => {
  const { setFullWidthSideBar } = useMapStateMachine();

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
      hasClaim,
      leaseAssociations,
      getLease.execute,
      getLeaseStakeholders.execute,
      getLeaseRenewals.execute,
    ],
  );

  const retrievedPid =
    composedPropertyState?.pid?.toString() ??
    composedPropertyState?.apiWrapper?.response?.pid?.toString();

  const retrievedPlanNumber =
    composedPropertyState?.planNumber?.toString() ??
    composedPropertyState?.apiWrapper?.response?.planNumber?.toString();

  const tabViews: TabInventoryView[] = [];
  let defaultTab = InventoryTabNames.title;

  tabViews.push({
    content: (
      <LtsaTabView
        ltsaData={composedPropertyState.ltsaWrapper?.response}
        ltsaRequestedOn={composedPropertyState.ltsaWrapper?.requestedOn}
        loading={composedPropertyState.ltsaWrapper?.loading ?? false}
        pid={retrievedPid}
      />
    ),
    key: InventoryTabNames.title,
    name: 'Title',
  });

  if (exists(retrievedPlanNumber) && isPlanNumberSPCP(retrievedPlanNumber)) {
    tabViews.push({
      content: (
        <LtsaPlanTabView
          spcpData={composedPropertyState.spcpWrapper?.response}
          ltsaRequestedOn={composedPropertyState.spcpWrapper?.requestedOn}
          loading={composedPropertyState.spcpWrapper?.loading ?? false}
          planNumber={retrievedPlanNumber}
        />
      ),
      key: InventoryTabNames.plan,
      name: 'Plan',
    });
  }

  tabViews.push({
    content: (
      <BcAssessmentTabView
        summaryData={composedPropertyState.bcAssessmentWrapper?.response}
        requestedOn={composedPropertyState.bcAssessmentWrapper?.requestedOn}
        loading={composedPropertyState.bcAssessmentWrapper?.loading ?? false}
        pid={retrievedPid}
      />
    ),
    key: InventoryTabNames.value,
    name: 'Value',
  });

  if (showPropertyInfoTab) {
    // After API property object has been received, we query relevant map layers to find
    // additional information which we store in a different model (IPropertyDetailsForm)

    tabViews.push({
      content: (
        <PropertyDetailsTabView
          property={{
            ...toFormValues(composedPropertyState?.apiWrapper?.response),
            electoralDistrict: firstOrNull(
              composedPropertyState?.composedProperty?.featureDataset?.electoralFeatures,
            ),
            isALR: composedPropertyState?.composedProperty?.featureDataset?.alrFeatures?.length > 0,
            firstNations: {
              bandName:
                firstOrNull(
                  composedPropertyState?.composedProperty?.featureDataset?.firstNationFeatures,
                )?.properties.BAND_NAME || '',
              reserveName:
                firstOrNull(
                  composedPropertyState?.composedProperty?.featureDataset?.firstNationFeatures,
                )?.properties.ENGLISH_NAME || '',
            },
          }}
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
    exists(composedPropertyState.apiWrapper?.response) &&
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

  if (exists(composedPropertyState?.composedProperty)) {
    const composedProperty = composedPropertyState?.composedProperty;
    const featureDataset = composedPropertyState?.composedProperty?.featureDataset;
    if (featureDataset?.parcelFeatures?.length > 0) {
      tabViews.push({
        content: (
          <LayerTabContainer
            composedProperty={composedPropertyState?.composedProperty}
            activeTab={InventoryTabNames.pmbc}
            View={LayerTabView}
          />
        ),
        key: InventoryTabNames.pmbc,
        name: 'PMBC',
      });
    }
    if (featureDataset?.pimsFeatures?.length > 0 && !exists(composedProperty?.id)) {
      tabViews.push({
        content: (
          <LayerTabContainer
            composedProperty={composedPropertyState?.composedProperty}
            activeTab={InventoryTabNames.pims}
            View={LayerTabView}
          />
        ),
        key: InventoryTabNames.pims,
        name: 'PIMS',
      });
    }
    if (
      featureDataset?.crownLandInclusionsFeatures?.length +
        featureDataset?.crownLandInventoryFeatures?.length +
        featureDataset?.crownLandLeasesFeatures?.length +
        featureDataset?.crownLandLicensesFeatures?.length +
        featureDataset?.crownLandTenuresFeatures?.length >
      0
    ) {
      tabViews.push({
        content: (
          <LayerTabContainer
            composedProperty={composedPropertyState?.composedProperty}
            activeTab={InventoryTabNames.crown}
            View={LayerTabView}
          />
        ),
        key: InventoryTabNames.crown,
        name: 'Crown',
      });
    }
    if (featureDataset?.highwayFeatures?.length > 0) {
      tabViews.push({
        content: (
          <LayerTabContainer
            composedProperty={composedPropertyState?.composedProperty}
            activeTab={InventoryTabNames.highway}
            View={LayerTabView}
          />
        ),
        key: InventoryTabNames.highway,
        name: 'HWY',
      });
    }
    if (
      featureDataset?.municipalityFeatures?.length > 0 ||
      featureDataset?.electoralFeatures?.length > 0 ||
      featureDataset?.alrFeatures?.length > 0 ||
      (featureDataset?.firstNationFeatures?.length > 0 &&
        !composedPropertyState.alrLoading &&
        !composedPropertyState.electoralLoading &&
        !composedPropertyState.electoralLoading &&
        !composedPropertyState.firstNationsLoading)
    ) {
      tabViews.push({
        content: (
          <LayerTabContainer
            composedProperty={composedPropertyState?.composedProperty}
            activeTab={InventoryTabNames.other}
            View={LayerTabCollapsedView}
          />
        ),
        key: InventoryTabNames.other,
        name: 'Other',
      });
    }
  }

  if (exists(composedPropertyState.apiWrapper?.response) && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: (
        <PropertyDocumentsTab
          fileId={composedPropertyState.apiWrapper.response.id}
          onSuccess={onChildSuccess}
        />
      ),
      key: InventoryTabNames.documents,
      name: 'Documents',
    });
  }

  if (exists(composedPropertyState?.apiWrapper?.response) && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: (
        <>
          <NoteListContainer
            type={NoteTypes.Property}
            entityId={composedPropertyState.apiWrapper.response.id}
            onSuccess={onChildSuccess}
            View={NoteListView}
          />
          <NoteSummaryContainer
            associationType={NoteTypes.Management_File}
            entityId={composedPropertyState.apiWrapper.response.id}
            onSuccess={onChildSuccess}
            NoteListView={NoteSummaryView}
          />
        </>
      ),
      key: InventoryTabNames.notes,
      name: 'Notes',
    });
  }

  const params = useParams<{ tab?: string }>();
  const activeTab = Object.values(InventoryTabNames).find(t => t === params.tab) ?? defaultTab;

  useEffect(() => {
    if (activeTab === InventoryTabNames.documents || activeTab === InventoryTabNames.notes) {
      setFullWidthSideBar(true);
    } else {
      setFullWidthSideBar(false);
    }
    return () => setFullWidthSideBar(false);
  }, [activeTab, setFullWidthSideBar]);
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
