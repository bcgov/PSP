import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaSearchPlus } from 'react-icons/fa';
import styled from 'styled-components';

import { StyledIconButton } from '@/components/common/buttons';
import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
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
