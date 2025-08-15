import { Feature, Geometry } from 'geojson';
import { chain } from 'lodash';
import React, { useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';
import styled from 'styled-components';

import AcquisitionIcon from '@/assets/images/acquisition-icon.svg?react';
import DispositionIcon from '@/assets/images/disposition-icon.svg?react';
import LeaseIcon from '@/assets/images/lease-icon.svg?react';
import ManagementIcon from '@/assets/images/management-icon.svg?react';
import ResearchIcon from '@/assets/images/research-icon.svg?react';
import { Button } from '@/components/common/buttons';
import { MapFeatureData } from '@/components/common/mapFSM/models';
import MoreOptionsMenu, { MenuOption } from '@/components/common/MoreOptionsMenu';
import { Scrollable } from '@/components/common/Scrollable/Scrollable';
import { Section } from '@/components/common/Section/Section';
import { SimpleSectionHeader } from '@/components/common/SimpleSectionHeader';
import { Claims } from '@/constants';
import { PropertyFilter } from '@/features/properties/filter';
import {
  defaultPropertyFilter,
  IPropertyFilter,
} from '@/features/properties/filter/IPropertyFilter';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';
import { exists } from '@/utils';
import { isStrataCommonProperty, pidFormatter } from '@/utils/propertyUtils';

export interface ISearchViewProps {
  onFilterChange: (filter: IPropertyFilter) => void;
  propertyFilter: IPropertyFilter;
  searchResult: MapFeatureData;
  canAddToOpenFile?: boolean;
  onCreateResearchFile: () => void;
  onCreateAcquisitionFile: () => void;
  onCreateDispositionFile: () => void;
  onCreateLeaseFile: () => void;
  onCreateManagementFile: () => void;
  onAddToOpenFile: () => void;
}

interface PropertyProjection<T> {
  isStrataLot: boolean;
  pid: string | null;
  pin: string | null;
  plan: string | null;
  feature: Feature<Geometry, T> | null;
}

export const SearchView: React.FC<ISearchViewProps> = props => {
  const history = useHistory();
  const keycloak = useKeycloakWrapper();

  const groupedFeatures = chain(props.searchResult?.fullyAttributedFeatures.features)
    .groupBy(feature => feature?.properties?.PLAN_NUMBER)
    .map(
      planGroup =>
        planGroup
          ?.map<PropertyProjection<PMBC_FullyAttributed_Feature_Properties>>(x => ({
            pid: x.properties.PID_FORMATTED,
            pin: exists(x.properties.PIN) ? String(x.properties.PIN) : null,
            isStrataLot: isStrataCommonProperty(x),
            feature: x,
            plan: x.properties.PLAN_NUMBER,
          }))
          .sort((a, b) => {
            if (a.isStrataLot === b.isStrataLot) return 0;
            if (a.isStrataLot) return -1;
            if (b.isStrataLot) return 1;
            return 0;
          }) ?? [],
    );

  const propertyProjections = groupedFeatures.value().flatMap(x => x) ?? [];

  const pimsGroupedFeatures = chain(props.searchResult?.pimsLocationFeatures.features)
    .groupBy(feature => feature?.properties?.SURVEY_PLAN_NUMBER)
    .map(
      planGroup =>
        planGroup
          ?.map<PropertyProjection<PIMS_Property_Location_View>>(x => ({
            pid: x.properties.PID_PADDED,
            pin: exists(x.properties.PIN) ? String(x.properties.PIN) : null,
            isStrataLot: false,
            feature: x,
            plan: x.properties.SURVEY_PLAN_NUMBER,
          }))
          .sort((a, b) => {
            if (a.isStrataLot === b.isStrataLot) return 0;
            if (a.isStrataLot) return -1;
            if (b.isStrataLot) return 1;
            return 0;
          }) ?? [],
    );

  const pimsPropertyProjections = pimsGroupedFeatures.value().flatMap(x => x) ?? [];

  const onOpenPropertyList = () => {
    history.push('/properties/list');
  };

  const menuOptions: MenuOption[] = useMemo(() => {
    const options: MenuOption[] = [];

    if (keycloak.hasClaim(Claims.RESEARCH_ADD)) {
      options.push({
        label: 'Create Research File',
        onClick: props.onCreateResearchFile,
        icon: <ResearchIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.ACQUISITION_ADD)) {
      options.push({
        label: 'Create Acquisition File',
        onClick: props.onCreateAcquisitionFile,
        icon: <AcquisitionIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.MANAGEMENT_ADD)) {
      options.push({
        label: 'Create Management File',
        onClick: props.onCreateManagementFile,
        icon: <ManagementIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.LEASE_ADD)) {
      options.push({
        label: 'Create Lease File',
        onClick: props.onCreateLeaseFile,
        icon: <LeaseIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.DISPOSITION_ADD)) {
      options.push({
        label: 'Create Disposition File',
        onClick: props.onCreateDispositionFile,
        icon: <DispositionIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }

    options.push({
      label: 'Add to Open File',
      onClick: props.onAddToOpenFile,
      icon: props.canAddToOpenFile ? <FaPlus size="1.5rem" /> : undefined,
      disabled: !props.canAddToOpenFile,
      tooltip: 'A file must be open and in "edit property" mode',
      separator: true, // Add a separator before the "Add to Open File" option
    });

    return options;
  }, [
    keycloak,
    props.canAddToOpenFile,
    props.onAddToOpenFile,
    props.onCreateAcquisitionFile,
    props.onCreateDispositionFile,
    props.onCreateLeaseFile,
    props.onCreateManagementFile,
    props.onCreateResearchFile,
  ]);

  return (
    <>
      <StyledWrapper>
        <Section>
          <Button onClick={onOpenPropertyList}>Search PIMS information</Button>
        </Section>
        <Section>
          <PropertyFilter
            defaultFilter={{ ...defaultPropertyFilter }}
            propertyFilter={props.propertyFilter}
            onChange={props.onFilterChange}
            useGeocoder
          />
        </Section>
        <Section
          header={
            <SimpleSectionHeader title="Results (PMBC)">
              <MoreOptionsMenu options={menuOptions} />
            </SimpleSectionHeader>
          }
          isCollapsable
          initiallyExpanded
        >
          <StyledScrollable className="pb-4">
            {propertyProjections.map((property, index) => (
              <StyledRow key={`feature-${property.pid}-${index}`} index={index}>
                {property.isStrataLot && <Col>Common Property ({property.plan})</Col>}
                {property.pid && <Col>PID: {property.pid} </Col>}
                {property.pin && <Col>PIN: {property.pin} </Col>}
              </StyledRow>
            ))}
          </StyledScrollable>
        </Section>
        <Section header="Results (PIMS)" isCollapsable initiallyExpanded>
          <StyledScrollable className="pb-4">
            {pimsPropertyProjections.map((property, index) => (
              <StyledRow key={`feature-${property.pid}-${index}`} index={index}>
                {property.isStrataLot && <Col>Common Property ({property.plan})</Col>}
                {property.pid && <Col>PID: {pidFormatter(property.pid)} </Col>}
                {property.pin && <Col>PIN: {property.pin} </Col>}
              </StyledRow>
            ))}
          </StyledScrollable>
        </Section>
      </StyledWrapper>
    </>
  );
};

const StyledWrapper = styled.div`
  height: 60%;
`;

const StyledRow = styled(Row)<{ index: number }>`
  color: rgb(1, 51, 102);
  font-weight: bold;
  padding-top: 0.8rem;
  cursor: pointer;
  background-color: ${p => (p.index % 2 === 0 ? '#f5f6f8' : 'none')};

  padding: 0.8rem;

  &:hover {
    color: var(--surface-color-primary-button-hover);
    text-decoration: underline;
  }
`;

const StyledScrollable = styled(Scrollable)`
  overflow: auto;
  font-size: 1.4rem;
`;
