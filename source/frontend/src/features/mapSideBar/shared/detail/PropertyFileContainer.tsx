import { useEffect, useMemo, useState } from 'react';
import { useParams } from 'react-router-dom';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Claims, NoteTypes } from '@/constants';
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
import NoteSummaryContainer from '@/features/notes/list/ManagementNoteSummaryContainer';
import NoteSummaryView from '@/features/notes/list/ManagementNoteSummaryView';
import NoteListContainer from '@/features/notes/list/NoteListContainer';
import NoteListView from '@/features/notes/list/NoteListView';
import { PROPERTY_TYPES, useComposedProperties } from '@/hooks/repositories/useComposedProperties';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { useLeaseStakeholderRepository } from '@/hooks/repositories/useLeaseStakeholderRepository';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';
import { exists, firstOrNull, isPlanNumberSPCP, isValidId } from '@/utils';

import { LayerTabContainer } from '../../layer/LayerTabContainer';
import { LayerTabView } from '../../layer/LayerTabView';
import { getLeaseInfo, LeaseAssociationInfo } from '../../property/PropertyContainer';
import LtsaPlanTabView from '../../property/tabs/ltsa/LtsaPlanTabView';
import { toFormValues } from '../../property/tabs/propertyDetails/detail/PropertyDetailsTabView.helpers';
import { PropertyManagementTabView } from '../../property/tabs/propertyDetailsManagement/detail/PropertyManagementTabView';
import PropertyResearchTabView from '../../property/tabs/propertyResearch/detail/PropertyResearchTabView';
import ResearchStatusUpdateSolver from '../../research/tabs/fileDetails/ResearchStatusUpdateSolver';
import PropertyDocumentsTab from '../tabs/PropertyDocumentsTab';

export interface IPropertyFileContainerProps {
  fileProperty: ApiGen_Concepts_FileProperty;
  setEditing: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IInventoryTabsProps>>;
  customTabs: TabInventoryView[];
  defaultTab: InventoryTabNames;
  fileContext?: ApiGen_CodeTypes_FileTypes;
  statusSolver?: ResearchStatusUpdateSolver;
  onChildSuccess: () => void;
}

export const PropertyFileContainer: React.FunctionComponent<
  IPropertyFileContainerProps
> = props => {
  const pid = props.fileProperty?.property?.pid ?? undefined;
  const id = props.fileProperty?.property?.id ?? undefined;
  const planNumber = props.fileProperty?.property?.planNumber ?? undefined;
  const boundary = props.fileProperty?.property?.boundary ?? undefined;
  const { hasClaim } = useKeycloakWrapper();

  const { setFullWidthSideBar } = useMapStateMachine();

  const composedProperties = useComposedProperties({
    pid,
    id,
    boundary,
    propertyTypes: propertyFileTabData,
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

  if (exists(planNumber) && isPlanNumberSPCP(planNumber)) {
    tabViews.push({
      content: (
        <LtsaPlanTabView
          spcpData={composedProperties.spcpWrapper?.response}
          ltsaRequestedOn={composedProperties.spcpWrapper?.requestedOn}
          loading={composedProperties.spcpWrapper?.loading ?? false}
          planNumber={planNumber}
        />
      ),
      key: InventoryTabNames.plan,
      name: 'Plan',
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
          isFileFinalStatus={!props.statusSolver?.canEditProperties()}
        />
      ),
      key: InventoryTabNames.research,
      name: 'Property Research',
    });
  }

  if (props.fileContext === ApiGen_CodeTypes_FileTypes.Management) {
    tabViews.push({
      content: (
        <PropertyManagementTabView
          property={composedProperties.apiWrapper?.response}
          loading={composedProperties.apiWrapper?.loading ?? false}
        />
      ),
      key: InventoryTabNames.management,
      name: 'Management',
    });
  }

  tabViews.push(...props.customTabs);

  if (isValidId(id)) {
    tabViews.push({
      content: (
        <PropertyDetailsTabView
          property={{
            ...toFormValues(props.fileProperty.property),
            electoralDistrict: firstOrNull(composedProperties?.composedProperty?.electoralFeatures),
            isALR: composedProperties?.composedProperty?.alrFeatures?.length > 0,
            firstNations: {
              bandName:
                firstOrNull(composedProperties?.composedProperty?.firstNationFeatures)?.properties
                  .BAND_NAME || '',
              reserveName:
                firstOrNull(composedProperties?.composedProperty?.firstNationFeatures)?.properties
                  .ENGLISH_NAME || '',
            },
          }}
          loading={composedProperties?.composedLoading ?? false}
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

  if (exists(composedProperties?.composedProperty)) {
    const composedProperty = composedProperties?.composedProperty;
    if (composedProperty?.parcelMapFeatureCollection?.features?.length > 0) {
      tabViews.push({
        content: (
          <LayerTabContainer
            composedProperty={composedProperties?.composedProperty}
            activeTab={InventoryTabNames.pmbc}
            View={LayerTabView}
          />
        ),
        key: InventoryTabNames.pmbc,
        name: 'PMBC',
      });
    }
    if (
      composedProperty?.pimsGeoserverFeatureCollection?.features?.length > 0 &&
      !exists(composedProperty?.id)
    ) {
      tabViews.push({
        content: (
          <LayerTabContainer
            composedProperty={composedProperties?.composedProperty}
            activeTab={InventoryTabNames.pims}
            View={LayerTabView}
          />
        ),
        key: InventoryTabNames.pims,
        name: 'PIMS',
      });
    }
    if (
      composedProperty?.crownInclusionFeatures?.length +
        composedProperty?.crownInventoryFeatures?.length +
        composedProperty?.crownLeaseFeatures?.length +
        composedProperty?.crownLeaseFeatures?.length +
        composedProperty?.crownLicenseFeatures?.length +
        composedProperty?.crownTenureFeatures?.length >
      0
    ) {
      tabViews.push({
        content: (
          <LayerTabContainer
            composedProperty={composedProperties?.composedProperty}
            activeTab={InventoryTabNames.crown}
            View={LayerTabView}
          />
        ),
        key: InventoryTabNames.crown,
        name: 'Crown',
      });
    }
    if (composedProperty?.highwayFeatures?.length > 0) {
      tabViews.push({
        content: (
          <LayerTabContainer
            composedProperty={composedProperties?.composedProperty}
            activeTab={InventoryTabNames.highway}
            View={LayerTabView}
          />
        ),
        key: InventoryTabNames.highway,
        name: 'HWY',
      });
    }
    if (composedProperty?.municipalityFeatures?.length > 0) {
      tabViews.push({
        content: (
          <LayerTabContainer
            composedProperty={composedProperties?.composedProperty}
            activeTab={InventoryTabNames.other}
            View={LayerTabView}
          />
        ),
        key: InventoryTabNames.other,
        name: 'Other',
      });
    }
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

  if (exists(composedProperties.apiWrapper?.response) && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: (
        <PropertyDocumentsTab
          fileId={composedProperties.apiWrapper.response.id}
          onSuccess={props.onChildSuccess}
        />
      ),
      key: InventoryTabNames.document,
      name: 'Property Documents',
    });
  }

  if (exists(composedProperties?.apiWrapper?.response) && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: (
        <>
          <NoteListContainer
            type={NoteTypes.Property}
            entityId={composedProperties.apiWrapper.response.id}
            onSuccess={props.onChildSuccess}
            View={NoteListView}
          />
          <NoteSummaryContainer
            associationType={NoteTypes.Management_File}
            entityId={composedProperties.apiWrapper.response.id}
            onSuccess={props.onChildSuccess}
            NoteListView={NoteSummaryView}
          />
        </>
      ),
      key: InventoryTabNames.notes,
      name: 'Notes',
    });
  }

  const InventoryTabsView = props.View;

  const params = useParams<{ tab?: string }>();
  const activeTab =
    Object.values(InventoryTabNames).find(t => t === params.tab) ?? props.defaultTab;

  useEffect(() => {
    if (activeTab === InventoryTabNames.document || activeTab === InventoryTabNames.notes) {
      setFullWidthSideBar(true);
    } else {
      setFullWidthSideBar(false);
    }
    return () => setFullWidthSideBar(false);
  }, [activeTab, setFullWidthSideBar]);

  return (
    <InventoryTabsView
      loading={composedProperties.composedLoading}
      tabViews={tabViews}
      defaultTabKey={props.defaultTab}
      activeTab={activeTab}
    />
  );
};

const propertyFileTabData = [
  PROPERTY_TYPES.ASSOCIATIONS,
  PROPERTY_TYPES.LTSA,
  PROPERTY_TYPES.PIMS_API,
  PROPERTY_TYPES.BC_ASSESSMENT,
  PROPERTY_TYPES.PARCEL_MAP,
  PROPERTY_TYPES.PIMS_GEOSERVER,
  PROPERTY_TYPES.CROWN_TENURES,
  PROPERTY_TYPES.CROWN_INCLUSIONS,
  PROPERTY_TYPES.CROWN_INVENTORY,
  PROPERTY_TYPES.CROWN_LEASES,
  PROPERTY_TYPES.CROWN_LICENSES,
  PROPERTY_TYPES.CROWN_SURVEYS,
  PROPERTY_TYPES.MUNICIPALITY,
  PROPERTY_TYPES.HIGHWAYS,
];

export default PropertyFileContainer;
