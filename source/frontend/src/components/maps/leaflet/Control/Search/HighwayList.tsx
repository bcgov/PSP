import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { MapFeatureData } from '@/components/common/mapFSM/models';
import OverflowTip from '@/components/common/OverflowTip';
import { Section } from '@/components/common/Section/Section';
import { SimpleSectionHeader } from '@/components/common/SimpleSectionHeader';
import { ZoomIconType, ZoomToLocation } from '@/components/maps/ZoomToLocation';

export interface IHighwayListViewProps {
  searchResult: MapFeatureData;
}

export const HighwayListView: React.FC<IHighwayListViewProps> = props => {
  return (
    <Section
      header={<SimpleSectionHeader title="Highway Results" />}
      isCollapsable
      initiallyExpanded
    >
      {props.searchResult?.highwayPlanFeatures?.features?.map(feature => (
        <StyledRow key={feature.properties.UNIQUE_ID}>
          <StyledNameCol>
            <StyledOverflowTip fullText={`Plan#: ${feature.properties.SURVEY_PLAN}`} />
          </StyledNameCol>
          <StyledButtonCol>
            <ButtonContainer>
              <ZoomToLocation icon={ZoomIconType.single} featureCollection={[feature]} />
            </ButtonContainer>
          </StyledButtonCol>
        </StyledRow>
      ))}
    </Section>
  );
};

const StyledRow = styled(Row)`
  display: flex;
  align-items: center;
  margin-left: 0;
  margin-right: 0;
  min-height: 4.5rem;

  &:hover {
    // Adding a 38% opacity to the background color (to match the mockups)
    background-color: ${props => props.theme.css.pimsBlue10 + '38'};
  }
`;

const StyledOverflowTip = styled(OverflowTip)`
  font-size: 1.4rem;
  font-weight: 700;
  color: ${props => props.theme.css.pimsBlue200};
`;

const StyledNameCol = styled(Col)`
  display: flex;
  justify-content: flex-start;
  padding-left: 3rem;
  padding-right: 0;
`;

const StyledButtonCol = styled(Col)`
  width: 10rem;
  flex: 0 0 10rem; /* Prevents shrinking/growing */
  display: flex;
  justify-content: flex-end;
`;

const ButtonContainer = styled.div`
  display: none;
  gap: 0.5rem;
  align-items: center;

  ${StyledRow}:hover & {
    display: flex;
  }
`;
