import { Feature, Geometry } from 'geojson';
import { chain } from 'lodash';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaWindowClose } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton, StyledIconButton } from '@/components/common/buttons';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import {
  LocationFeatureDataset,
  WorklistLocationFeatureDataset,
} from '@/components/common/mapFSM/useLocationFeatureLoader';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { StyledScrollable } from '@/features/documents/commonStyles';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { exists, firstOrNull } from '@/utils';
import { isStrataCommonProperty, pidFormatter } from '@/utils/propertyUtils';

export interface IMultiplePropertyPopupView {
  featureDataset: LocationFeatureDataset | null;
  onSelectProperty: (feature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties>) => void;
}

interface PropertyProjection {
  isStrataLot: boolean;
  pid: string | null;
  pin: string | null;
  plan: string | null;
  feature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null;
}

export const MultiplePropertyPopupView: React.FC<
  React.PropsWithChildren<IMultiplePropertyPopupView>
> = ({ featureDataset, onSelectProperty }) => {
  const mapMachine = useMapStateMachine();

  const onCloseButtonPressed = (event: React.MouseEvent<HTMLElement, MouseEvent>) => {
    event.stopPropagation();
    mapMachine.closePopup();
  };

  const handlePropertySelect = (index: number) => {
    const selectedProperty = propertyProjections[index];
    if (exists(selectedProperty?.feature)) {
      onSelectProperty(selectedProperty.feature);
    }
  };

  const onAddAllToWorklist = () => {
    debugger;
    const worklistDataSet: WorklistLocationFeatureDataset = {
      fullyAttributedFeatures: {
        features: featureDataset.parcelFeatures,
        type: 'FeatureCollection',
      },
      pimsFeature: firstOrNull(featureDataset.pimsFeatures),
      regionFeature: featureDataset.regionFeature,
      districtFeature: featureDataset.districtFeature,
      location: featureDataset.location,
    };
    mapMachine.worklistAdd(worklistDataSet);
  };

  const groupedFeatures = chain(featureDataset?.parcelFeatures)
    .groupBy(feature => feature?.properties?.PLAN_NUMBER)
    .map(
      planGroup =>
        planGroup
          ?.map<PropertyProjection>(x => ({
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

  return (
    <StyledContainer>
      <TooltipWrapper tooltipId="close-sidebar-tooltip" tooltip="Close Form">
        <StyledCloseButton title="close" onClick={onCloseButtonPressed}>
          <CloseIcon />
        </StyledCloseButton>
      </TooltipWrapper>
      <StyledTitle>Multiple properties found</StyledTitle>
      <StyledDivider />
      <StyledScrollable className="pb-4">
        {propertyProjections.map((property, index) => (
          <StyledRow
            key={`feature-${property.pid}-${index}`}
            onClick={(e: Event) => {
              e.stopPropagation();
              handlePropertySelect(index);
            }}
            index={index}
          >
            {property.isStrataLot && <Col>Common Property ({property.plan})</Col>}
            {property.pid && <Col>PID: {pidFormatter(property.pid)} </Col>}
            {property.pin && <Col>PIN: {property.pin} </Col>}
          </StyledRow>
        ))}
      </StyledScrollable>
      <LinkButton onClick={onAddAllToWorklist}>+ Add all to worklist</LinkButton>
    </StyledContainer>
  );
};

const CloseIcon = styled(FaWindowClose)`
  color: ${props => props.theme.css.textColor};
  font-size: 2rem;
  cursor: pointer;
`;

const StyledCloseButton = styled(StyledIconButton)`
  position: fixed;
  top: 0rem;
  right: 1rem;
`;

const StyledContainer = styled.div`
  position: relative;
  min-width: 30rem;
  padding-left: 1.6rem;
  padding-right: 1.6rem;
  margin: 0rem;
`;

const StyledTitle = styled.div`
  font-size: 1.6rem;
  padding-top: 1rem;
  font-weight: bold;
  height: 3.6rem;
`;

const StyledDivider = styled.div`
  margin-bottom: 1rem;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
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
