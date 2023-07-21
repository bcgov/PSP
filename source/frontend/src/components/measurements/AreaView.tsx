import { Col, Row } from 'react-bootstrap';

import { AreaUnitTypes } from '@/constants/areaUnitTypes';
import { convertArea, formatNumber } from '@/utils';

import { StyledGreenCol, StyledGreenGrey } from './styles';

interface IAreaViewProps {
  landArea?: number;
  unitCode?: string;
}

const AreaView: React.FunctionComponent<IAreaViewProps> = props => {
  const meters = convertArea(props.landArea || 0, props.unitCode || '', AreaUnitTypes.SquareMeters);
  const feet = convertArea(props.landArea || 0, props.unitCode || '', AreaUnitTypes.SquareFeet);
  const hectars = convertArea(props.landArea || 0, props.unitCode || '', AreaUnitTypes.Hectares);
  const acres = convertArea(props.landArea || 0, props.unitCode || '', AreaUnitTypes.Acres);
  return (
    <>
      <Row>
        <Col>
          <StyledGreenCol>
            <Row>
              <Col className="text-right">{formatNumber(meters, 0, 2)}</Col>
              <Col>sq. metres</Col>
            </Row>
            <Row>
              <Col className="text-right">{formatNumber(hectars, 0, 2)}</Col>
              <Col>hectares</Col>
            </Row>
          </StyledGreenCol>
        </Col>
        <Col>
          <StyledGreenGrey>
            <Row>
              <Col className="text-right">{formatNumber(feet, 0, 2)}</Col>
              <Col>sq. feet</Col>
            </Row>
            <Row>
              <Col className="text-right">{formatNumber(acres, 0, 2)}</Col>
              <Col>acres</Col>
            </Row>
          </StyledGreenGrey>
        </Col>
      </Row>
    </>
  );
};

export default AreaView;
