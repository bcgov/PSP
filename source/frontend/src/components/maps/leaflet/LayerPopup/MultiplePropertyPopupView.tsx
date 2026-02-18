import { Feature, Geometry } from 'geojson';
import { chain } from 'lodash';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaWindowClose } from 'react-icons/fa';
import styled from 'styled-components';

import AddAllWorklistIcon from '@/assets/images/add-all-wl-icon.svg?react';
import AddToWorklistIcon from '@/assets/images/add-to-wl-icon.svg?react';
import { LinkButton, StyledIconButton } from '@/components/common/buttons';
import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { StyledScrollable } from '@/features/documents/commonStyles';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { exists } from '@/utils';
import { isStrataCommonProperty, pidFormatter } from '@/utils/propertyUtils';

export interface IMultiplePropertyPopupView {
  featureDataset: LocationFeatureDataset | null;
  onSelectProperty: (feature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties>) => void;
  onAddPropertyToWorklist: (
    feature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties>,
    featureDataset: LocationFeatureDataset,
  ) => void;
  onAddAllToWorklist: (featureDataset: LocationFeatureDataset) => void;
  onClose: (event: React.MouseEvent<HTMLElement, MouseEvent>) => void;
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
> = ({
  featureDataset,
  onSelectProperty,
  onAddPropertyToWorklist,
  onAddAllToWorklist,
  onClose,
}) => {
  const handlePropertySelect = (index: number) => {
    const selectedProperty = propertyProjections[index];
    if (exists(selectedProperty?.feature)) {
      onSelectProperty(selectedProperty.feature);
    }
  };

  const handlePropertyAddToWorklist = (index: number) => {
    const selectedProperty = propertyProjections[index];
    if (exists(selectedProperty?.feature)) {
      onAddPropertyToWorklist(selectedProperty.feature, featureDataset);
    }
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
        <StyledCloseButton title="close" onClick={onClose}>
          <CloseIcon />
        </StyledCloseButton>
      </TooltipWrapper>
      <StyledTitle>Multiple properties found</StyledTitle>
      <StyledDivider />
      <StyledScrollable className="pb-4">
        {propertyProjections.map((property, index) => (
          <StyledRow key={`feature-${property.pid}-${index}`}>
            <StyledPropIdentifierCol
              xs={10}
              onClick={(e: React.MouseEvent<HTMLElement, MouseEvent>) => {
                e.stopPropagation();
                handlePropertySelect(index);
              }}
            >
              {property.isStrataLot && <>Common Property ({property.plan})</>}
              {property.pid && <>PID: {pidFormatter(property.pid)} </>}
              {property.pin && <>PIN: {property.pin} </>}
            </StyledPropIdentifierCol>
            <StyledButtonCol xs={2}>
              <ButtonContainer>
                <StyledIconButton
                  title="Add to working list"
                  onClick={(e: React.MouseEvent<HTMLElement, MouseEvent>) => {
                    e.stopPropagation();
                    handlePropertyAddToWorklist(index);
                  }}
                >
                  <AddToWorklistIcon width="1.5rem" height="1.5rem" fill="currentColor" />
                </StyledIconButton>
              </ButtonContainer>
            </StyledButtonCol>
          </StyledRow>
        ))}
      </StyledScrollable>
      <StyledLinkButton
        onClick={() => onAddAllToWorklist(featureDataset)}
        disabled={propertyProjections.length === 0}
      >
        <AddAllWorklistIcon width={24} height={24} />
        <span>Add all to working list</span>
      </StyledLinkButton>
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
  padding-bottom: 1rem;
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

const StyledRow = styled(Row)`
  display: flex;
  align-items: center;
  flex-wrap: nowrap;
  font-weight: bold;
  padding-top: 0;
  padding-bottom: 0;
  background-color: none;
  min-height: 3.6rem;

  &:hover {
    background-color: ${props => props.theme.css.pimsBlue10 + '38'};
  }
`;

const StyledPropIdentifierCol = styled(Col)`
  display: flex;
  justify-content: flex-start;
  padding-right: 0;
  color: ${props => props.theme.css.pimsBlue200};
  font-weight: bold;
  cursor: pointer;
`;

const StyledButtonCol = styled(Col)`
  flex: 0 0 10rem;
  display: flex;
  justify-content: flex-end;
  gap: 0.5rem;
  padding-right: 2rem;
`;

const ButtonContainer = styled.div`
  display: none;
  gap: 0.5rem;
  align-items: center;

  ${StyledRow}:hover & {
    display: flex;
    flex-wrap: nowrap;
    gap: 0.5rem;
    justify-content: flex-end;
  }
`;

const StyledLinkButton = styled(LinkButton)`
  &&.btn {
    text-decoration: none;

    .Button__value {
      display: flex;
      align-items: center;
      gap: 0.8rem;
      font-weight: bold;
      font-size: 1.4rem;
    }
  }
`;
