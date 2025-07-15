import { Feature, Geometry } from 'geojson';
import { chain } from 'lodash';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { MapFeatureData } from '@/components/common/mapFSM/models';
import { Scrollable } from '@/components/common/Scrollable/Scrollable';
import { Section } from '@/components/common/Section/Section';
import { PropertyFilter } from '@/features/properties/filter';
import {
  defaultPropertyFilter,
  IPropertyFilter,
} from '@/features/properties/filter/IPropertyFilter';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';
import { exists } from '@/utils';
import { isStrataCommonProperty } from '@/utils/propertyUtils';

import { PropertyQuickInfoContainer } from './PropertyQuickInfoContainer';

export interface ISearchViewProps {
  onFilterChange: (filter: IPropertyFilter) => void;
  propertyFilter: IPropertyFilter;
  searchResult: MapFeatureData;
}

interface PropertyProjection<T> {
  isStrataLot: boolean;
  pid: string | null;
  pin: string | null;
  plan: string | null;
  feature: Feature<Geometry, T> | null;
}

export const SearchView: React.FC<ISearchViewProps> = props => {
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

  return (
    <>
      <StyledWrapper>
        <Section>
          <PropertyFilter
            defaultFilter={{ ...defaultPropertyFilter }}
            propertyFilter={props.propertyFilter}
            onChange={props.onFilterChange}
            useGeocoder
          />
        </Section>
        <Section header="Results (PMBC)" isCollapsable initiallyExpanded>
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
          <Scrollable className="pb-4">
            {pimsPropertyProjections.map((property, index) => (
              <StyledRow key={`feature-${property.pid}-${index}`} index={index}>
                {property.isStrataLot && <Col>Common Property ({property.plan})</Col>}
                {property.pid && <Col>PID: {property.pid} </Col>}
                {property.pin && <Col>PIN: {property.pin} </Col>}
              </StyledRow>
            ))}
          </Scrollable>
        </Section>
      </StyledWrapper>
      <PropertyQuickInfoContainer />
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
