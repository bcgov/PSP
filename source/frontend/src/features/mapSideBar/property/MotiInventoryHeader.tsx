import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import { FaSearchPlus } from 'react-icons/fa';
import styled from 'styled-components';

import { StyledIconButton } from '@/components/common/buttons';
import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { InlineFlexDiv } from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { IMapProperty } from '@/components/propertySelector/models';
import { ComposedProperty } from '@/features/mapSideBar/property/ComposedProperty';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { exists, formatApiAddress, pidFormatter } from '@/utils';
import { mapFeatureToProperty } from '@/utils/mapPropertyUtils';

export interface IMotiInventoryHeaderProps {
  isLoading: boolean;
  composedProperty: ComposedProperty;
  onZoom?: (apiProperty?: ApiGen_Concepts_Property | undefined) => void;
}

export const MotiInventoryHeader: React.FunctionComponent<IMotiInventoryHeaderProps> = props => {
  const pid = pidFormatter(props.composedProperty.pid);
  const parcelMapData = props.composedProperty.parcelMapFeatureCollection;
  const apiProperty = props.composedProperty.pimsProperty;
  let property: IMapProperty | null = null;

  if (parcelMapData?.features[0]) {
    property = mapFeatureToProperty(parcelMapData?.features[0]);
  }

  const isLoading = props.isLoading;

  const isRetired = React.useMemo(() => {
    if (exists(apiProperty) && apiProperty.isRetired) {
      return true;
    }
    return false;
  }, [apiProperty]);

  return (
    <>
      <LoadingBackdrop show={isLoading} parentScreen={true} />
      <Row className="no-gutters">
        <Col>
          <Row className="no-gutters">
            <Col xs="8">
              <HeaderField label="Civic Address:" labelWidth={'3'} contentWidth="9">
                {exists(apiProperty?.address) ? formatApiAddress(apiProperty!.address) : '-'}
              </HeaderField>
            </Col>
            <Col>
              <HeaderField className="justify-content-end" label="PID:" contentWidth="5">
                {pid}
              </HeaderField>
            </Col>
          </Row>
          <Row className="no-gutters">
            <Col xs="8">
              <HeaderField label="Plan #:" labelWidth={'3'} contentWidth="9">
                {property?.planNumber}
              </HeaderField>
            </Col>
            <Col>
              <HeaderField
                label="Land parcel type:"
                contentWidth="5"
                className="justify-content-end"
              >
                {apiProperty?.propertyType?.description}
              </HeaderField>
            </Col>
          </Row>
          {isRetired && (
            <Row className="no-gutters">
              <Col xs="8"></Col>
              <Col className="d-flex justify-content-end pr-4">
                <RetiredWarning>
                  <AiOutlineExclamationCircle size={16} />
                  RETIRED
                </RetiredWarning>
              </Col>
            </Row>
          )}
        </Col>
        <Col xs="auto" className="d-flex p-0 align-items-center justify-content-end">
          <TooltipWrapper tooltipId="property-zoom-map" tooltip="Zoom Map">
            <StyledIconButton
              variant="info"
              disabled={!props.onZoom}
              title="Zoom Map"
              onClick={() => props?.onZoom && props?.onZoom(apiProperty)}
            >
              <FaSearchPlus size={22} />
            </StyledIconButton>
          </TooltipWrapper>
        </Col>
      </Row>
      <StyledDivider />
    </>
  );
};

const StyledDivider = styled.div`
  margin-top: 0.5rem;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
`;

export const RetiredWarning = styled(InlineFlexDiv)`
  text-transform: uppercase;
  color: ${props => props.theme.css.fontWarningColor};
  background-color: ${props => props.theme.css.warningBackgroundColor};
  border-radius: 0.4rem;
  letter-spacing: 0.1rem;
  padding: 0.2rem 0.5rem;
  gap: 0.5rem;
  font-family: 'BCSans-Bold';
  font-size: 1.4rem;
  align-items: center;
  width: fit-content;
`;
