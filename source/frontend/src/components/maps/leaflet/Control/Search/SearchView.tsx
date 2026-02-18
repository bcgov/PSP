import { chain } from 'lodash';
import React, { useCallback, useMemo } from 'react';
import { FaPlus } from 'react-icons/fa';
import styled from 'styled-components';

import AcquisitionIcon from '@/assets/images/acquisition-icon.svg?react';
import DispositionIcon from '@/assets/images/disposition-icon.svg?react';
import LeaseIcon from '@/assets/images/lease-icon.svg?react';
import ManagementIcon from '@/assets/images/management-icon.svg?react';
import ResearchIcon from '@/assets/images/research-icon.svg?react';
import { MapFeatureData } from '@/components/common/mapFSM/models';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import MoreOptionsMenu, { MenuOption } from '@/components/common/MoreOptionsMenu';
import { Section } from '@/components/common/Section/Section';
import { SimpleSectionHeader } from '@/components/common/SimpleSectionHeader';
import { Claims } from '@/constants';
import { PropertyFilter } from '@/features/properties/filter';
import {
  defaultPropertyFilter,
  IPropertyFilter,
} from '@/features/properties/filter/IPropertyFilter';
import { ParcelDataset } from '@/features/properties/parcelList/models';
import { ParcelListContainer } from '@/features/properties/parcelList/ParcelListContainer';
import { ParcelListView } from '@/features/properties/parcelList/ParcelListView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { exists } from '@/utils';
import { isStrataCommonProperty } from '@/utils/propertyUtils';

import { HighwayListView } from './HighwayList';

export interface ISearchViewProps {
  onFilterChange: (filter: IPropertyFilter) => void;
  propertyFilter: IPropertyFilter;
  searchResult: MapFeatureData;
  pmbcSelectedFeatureDatasets?: SelectedFeatureDataset[];
  pimsSelectedFeatureDatasets?: SelectedFeatureDataset[];
  canAddToOpenFile?: boolean;
  onCreateResearchFile: (isPims: boolean) => void;
  onCreateAcquisitionFile: (isPims: boolean) => void;
  onCreateDispositionFile: (isPims: boolean) => void;
  onCreateLeaseFile: (isPims: boolean) => void;
  onCreateManagementFile: (isPims: boolean) => void;
  onAddToOpenFile: (isPims: boolean) => void;
}

export const SearchView: React.FC<ISearchViewProps> = props => {
  const canAddToOpenFile = props.canAddToOpenFile;
  const onAddToOpenFile = props.onAddToOpenFile;
  const onCreateAcquisitionFile = props.onCreateAcquisitionFile;
  const onCreateDispositionFile = props.onCreateDispositionFile;
  const onCreateLeaseFile = props.onCreateLeaseFile;
  const onCreateManagementFile = props.onCreateManagementFile;
  const onCreateResearchFile = props.onCreateResearchFile;
  const keycloak = useKeycloakWrapper();

  const propertyProjections = useMemo(() => {
    const fallbackFeatures = props.searchResult?.fullyAttributedFeatures?.features ?? [];

    const baseParcels = props.pmbcSelectedFeatureDatasets?.length
      ? props.pmbcSelectedFeatureDatasets.map(dataset =>
          ParcelDataset.fromSelectedFeatureDataset(dataset),
        )
      : fallbackFeatures.map(feature => ParcelDataset.fromFullyAttributedFeature(feature));

    return chain(baseParcels)
      .groupBy(parcel => parcel.pmbcFeature?.properties?.PLAN_NUMBER)
      .map(group =>
        group.toSorted((a, b) => {
          const aIsStrata = a.pmbcFeature ? isStrataCommonProperty(a.pmbcFeature) : false;
          const bIsStrata = b.pmbcFeature ? isStrataCommonProperty(b.pmbcFeature) : false;

          if (aIsStrata === bIsStrata) return 0;
          return aIsStrata ? -1 : 1;
        }),
      )
      .value()
      .flat();
  }, [props.searchResult, props.pmbcSelectedFeatureDatasets]);

  const pimsPropertyProjections = useMemo(() => {
    const fallbackFeatures = props.searchResult?.pimsFeatures?.features ?? [];

    const baseParcels = props.pimsSelectedFeatureDatasets?.length
      ? props.pimsSelectedFeatureDatasets.map(dataset =>
          ParcelDataset.fromSelectedFeatureDataset(dataset),
        )
      : fallbackFeatures.map(feature => ParcelDataset.fromPimsFeature(feature));

    return chain(baseParcels)
      .groupBy(pimsParcel => pimsParcel?.pimsFeature?.properties?.SURVEY_PLAN_NUMBER)
      .map(planGroup =>
        planGroup.toSorted((a, b) => {
          const aIsStrata = a.pmbcFeature ? isStrataCommonProperty(a.pmbcFeature) : false;
          const bIsStrata = b.pmbcFeature ? isStrataCommonProperty(b.pmbcFeature) : false;

          if (aIsStrata === bIsStrata) return 0;
          return aIsStrata ? -1 : 1;
        }),
      )
      .value()
      .flat();
  }, [props.searchResult, props.pimsSelectedFeatureDatasets]);

  const createMenuOptions = useCallback(
    (isPims: boolean): MenuOption[] => {
      const options: MenuOption[] = [];

      if (keycloak.hasClaim(Claims.RESEARCH_ADD)) {
        options.push({
          label: 'Create Research File',
          onClick: () => onCreateResearchFile(isPims),
          icon: <ResearchIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
        });
      }
      if (keycloak.hasClaim(Claims.ACQUISITION_ADD)) {
        options.push({
          label: 'Create Acquisition File',
          onClick: () => onCreateAcquisitionFile(isPims),
          icon: <AcquisitionIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
        });
      }
      if (keycloak.hasClaim(Claims.MANAGEMENT_ADD)) {
        options.push({
          label: 'Create Management File',
          onClick: () => onCreateManagementFile(isPims),
          icon: <ManagementIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
        });
      }
      if (keycloak.hasClaim(Claims.LEASE_ADD)) {
        options.push({
          label: 'Create Lease File',
          onClick: () => onCreateLeaseFile(isPims),
          icon: <LeaseIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
        });
      }
      if (keycloak.hasClaim(Claims.DISPOSITION_ADD)) {
        options.push({
          label: 'Create Disposition File',
          onClick: () => onCreateDispositionFile(isPims),
          icon: <DispositionIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
        });
      }

      options.push({
        label: 'Add to Open File',
        onClick: () => onAddToOpenFile(isPims),
        icon: canAddToOpenFile ? <FaPlus size="1.5rem" /> : undefined,
        disabled: !canAddToOpenFile,
        tooltip: 'A file must be open and in "edit property" mode',
        separator: true, // Add a separator before the "Add to Open File" option
      });

      return options;
    },
    [
      keycloak,
      canAddToOpenFile,
      onAddToOpenFile,
      onCreateAcquisitionFile,
      onCreateDispositionFile,
      onCreateLeaseFile,
      onCreateManagementFile,
      onCreateResearchFile,
    ],
  );

  const pmbcMenuOptions = useMemo(() => createMenuOptions(false), [createMenuOptions]);

  const pimsMenuOptions = useMemo(() => createMenuOptions(true), [createMenuOptions]);

  return (
    <StyledWrapper
      onClick={e => {
        e.stopPropagation(); // prevent any clicks on the search sidebar from propogating to the map.
      }}
    >
      <Section className="my-0 pt-0" data-testid="search-control-filters-section">
        <PropertyFilter
          defaultFilter={{ ...defaultPropertyFilter }}
          propertyFilter={props.propertyFilter}
          onChange={props.onFilterChange}
          useGeocoder
        />
      </Section>
      <Section
        className="my-0 py-0"
        header={
          <SimpleSectionHeader title="Results (PMBC)">
            <MoreOptionsMenu
              options={pmbcMenuOptions}
              ariaLabel="search pmbc results more options"
            />
          </SimpleSectionHeader>
        }
        isCollapsable
        initiallyExpanded
        data-testid="pmbc-search-results-section"
      >
        <ParcelListContainer View={ParcelListView} parcels={propertyProjections} />
      </Section>
      <Section
        className="my-0 py-0"
        header={
          <SimpleSectionHeader title="Results (PIMS)">
            <MoreOptionsMenu
              options={pimsMenuOptions}
              ariaLabel="search pims results more options"
            />
          </SimpleSectionHeader>
        }
        isCollapsable
        initiallyExpanded
        data-testid="pims-search-results-section"
      >
        <ParcelListContainer View={ParcelListView} parcels={pimsPropertyProjections} />
      </Section>
      {exists(props.searchResult?.highwayPlanFeatures) &&
        props.searchResult.highwayPlanFeatures.features.length > 0 && (
          <HighwayListView searchResult={props.searchResult} />
        )}
    </StyledWrapper>
  );
};

const StyledWrapper = styled.div`
  height: 60%;
  margin-top: 1rem;
`;
