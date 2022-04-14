import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { AssociatedPlan, LtsaOrders } from 'interfaces/ltsaModels';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { HeaderField } from './tabs/HeaderField';

interface IMotiInventoryHeaderProps {
  ltsaData?: LtsaOrders;
  property?: IPropertyApiModel;
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
      <HeaderWrapper>
        <LoadingBackdrop show={isLoading} parentScreen={true} />
        <Row>
          <Col xs="8">
            <HeaderField label={'Civic Address'}>-</HeaderField>
          </Col>
          <Col>
            <HeaderField label={'PID:'} className="justify-content-end">
              {pid}
            </HeaderField>
          </Col>
        </Row>
        <Row>
          <Col xs="8">
            <HeaderField label={'Plan(s) #'}>
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
