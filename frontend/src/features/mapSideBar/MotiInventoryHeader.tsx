import { IconButton } from 'components/common/buttons';
import TooltipWrapper from 'components/common/TooltipWrapper';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { AssociatedPlan, LtsaOrders } from 'interfaces/ltsaModels';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { MdZoomIn } from 'react-icons/md';
import styled from 'styled-components';

import { HeaderField } from './tabs/HeaderField';

interface IMotiInventoryHeaderProps {
  ltsaData?: LtsaOrders;
  property?: IPropertyApiModel;
  onZoom?: (apiProperty?: IPropertyApiModel | undefined) => void;
}

export const MotiInventoryHeader: React.FunctionComponent<IMotiInventoryHeaderProps> = props => {
  const pid = props.ltsaData?.parcelInfo.orderedProduct.fieldedData.parcelIdentifier;
  const planNumbers =
    props.ltsaData === undefined
      ? []
      : (props.ltsaData?.parcelInfo.orderedProduct.fieldedData
          .associatedPlans as AssociatedPlan[]).map(x => x.planNumber);

  const isLoading = props.ltsaData === undefined || props.property === undefined;
  return (
    <>
      <LoadingBackdrop show={isLoading} parentScreen={true} />
      <HeaderWrapper>
        <Row>
          <Col xs="11">
            <Row>
              <Col xs="8">
                <HeaderField label={'Civic Address'}>-</HeaderField>
              </Col>
              <Col>
                <HeaderField className="justify-content-end" label={'PID:'}>
                  {pid}
                </HeaderField>
              </Col>
            </Row>
            <Row>
              <Col xs="8">
                <HeaderField label={'Plan #'}>
                  {planNumbers.map((planNumber: string, index: number) => (
                    <span key={'plannumber-' + index} className="pr-3">
                      {planNumber}
                    </span>
                  ))}
                </HeaderField>
              </Col>
              <Col>
                <HeaderField label={'Land parcel type:'} className="justify-content-end">
                  {props.property?.propertyType?.description}
                </HeaderField>
              </Col>
            </Row>
          </Col>
          <Col xs="1" className="d-flex p-0 align-items-center justify-content-end">
            <IconButton
              variant="info"
              className="float-right"
              disabled={!props.onZoom}
              title="Zoom Map"
              onClick={() => props?.onZoom && props?.onZoom(props.property)}
            >
              <TooltipWrapper toolTipId="property-zoom-map" toolTip="Zoom Map">
                <MdZoomIn size={22} />
              </TooltipWrapper>
            </IconButton>
          </Col>
        </Row>
      </HeaderWrapper>
      <StyledDivider />
    </>
  );
};

const HeaderWrapper = styled.div`
  margin-left: 3rem;
  margin-right: 3rem;
  margin-top: 1rem;
  position: relative;
`;
const StyledDivider = styled.div`
  margin-top: 0.5rem;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
`;
