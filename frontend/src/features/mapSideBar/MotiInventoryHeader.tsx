import { IconButton } from 'components/common/buttons';
import ProtectedComponent from 'components/common/ProtectedComponent';
import TooltipWrapper from 'components/common/TooltipWrapper';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Claims } from 'constants/index';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { AssociatedPlan, LtsaOrders } from 'interfaces/ltsaModels';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaEdit, FaSearchPlus } from 'react-icons/fa';
import styled from 'styled-components';

import { HeaderField } from './tabs/HeaderField';

interface IMotiInventoryHeaderProps {
  ltsaData?: LtsaOrders;
  ltsaLoading: boolean;
  propertyLoading: boolean;
  property?: IPropertyApiModel;
  showEditButton?: boolean;
  onZoom?: (apiProperty?: IPropertyApiModel | undefined) => void;
}

export const MotiInventoryHeader: React.FunctionComponent<IMotiInventoryHeaderProps> = props => {
  const pid = props.ltsaData?.parcelInfo.orderedProduct.fieldedData.parcelIdentifier;
  const planNumbers =
    props.ltsaData === undefined
      ? []
      : (props.ltsaData?.parcelInfo.orderedProduct.fieldedData
          .associatedPlans as AssociatedPlan[]).map(x => x.planNumber);

  const isLoading = props.ltsaLoading || props.propertyLoading;
  return (
    <>
      <LoadingBackdrop show={isLoading} parentScreen={true} />
      <HeaderWrapper>
        <Row>
          <Col xs="10">
            <Row>
              <Col xs="8">
                <HeaderField label="Civic Address:">-</HeaderField>
              </Col>
              <Col>
                <HeaderField className="justify-content-end" label="PID:">
                  {pid}
                </HeaderField>
              </Col>
            </Row>
            <Row>
              <Col xs="8">
                <HeaderField label="Plan #:">
                  {planNumbers.map((planNumber: string, index: number) => (
                    <span key={'plannumber-' + index} className="pr-3">
                      {planNumber}
                    </span>
                  ))}
                </HeaderField>
              </Col>
              <Col>
                <HeaderField label="Land parcel type:" className="justify-content-end">
                  {props.property?.propertyType?.description}
                </HeaderField>
              </Col>
            </Row>
          </Col>
          <Col xs="2" className="d-flex p-0 align-items-center justify-content-end">
            <TooltipWrapper toolTipId="property-zoom-map" toolTip="Zoom Map">
              <IconButton
                variant="info"
                disabled={!props.onZoom}
                title="Zoom Map"
                onClick={() => props?.onZoom && props?.onZoom(props.property)}
              >
                <FaSearchPlus size={22} />
              </IconButton>
            </TooltipWrapper>
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
