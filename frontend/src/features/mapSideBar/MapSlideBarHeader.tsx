import { AssociatedPlan, LtsaOrders } from 'interfaces/ltsaModels';
import { Col, Row } from 'react-bootstrap';

import { SectionField } from './tabs/SectionField';

interface IMapSlideBarHeaderProps {
  ltsaData?: LtsaOrders;
  propertyDetails: any;
}

export const MapSlideBarHeader: React.FunctionComponent<IMapSlideBarHeaderProps> = props => {
  const pid = props.ltsaData?.parcelInfo.orderedProduct.fieldedData.parcelIdentifier;
  const planNumbers = (props.ltsaData?.parcelInfo.orderedProduct.fieldedData
    .associatedPlans as AssociatedPlan[]).map(x => x.planNumber);
  console.log(props.ltsaData?.titleOrders[0].orderedProduct.fieldedData);
  return (
    <>
      <Row>
        <Col>
          <SectionField label={'Civic Address'} isCompact>
            Todo
          </SectionField>
        </Col>
        <Col>
          <SectionField label={'PID'} isCompact>
            {pid}
          </SectionField>
        </Col>
      </Row>
      <Row>
        <Col>
          <SectionField label={'Plan(s) #'} isCompact>
            {planNumbers.map((planNumber: string, index: number) => (
              <span key={'plannumber-' + index} className="pr-3">
                {planNumber}
              </span>
            ))}
          </SectionField>
        </Col>
        <Col>
          <SectionField label={'Land parcel type'} isCompact>
            {props.propertyDetails.propertyType}
          </SectionField>
        </Col>
      </Row>
    </>
  );
};
