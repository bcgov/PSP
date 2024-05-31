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
  const hectares = convertArea(props.landArea || 0, props.unitCode || '', AreaUnitTypes.Hectares);
  const acres = convertArea(props.landArea || 0, props.unitCode || '', AreaUnitTypes.Acres);
  return (
    <Row>
      <Col>
        <StyledGreenCol>
          <Row>
            <Col className="text-right">{meters === 0 ? 0 : formatNumber(meters, 4, 4)}</Col>
            <Col>sq. metres</Col>
          </Row>
          <Row>
            <Col className="text-right">{hectares === 0 ? 0 : formatNumber(hectares, 4, 4)}</Col>
            <Col>hectares</Col>
          </Row>
        </StyledGreenCol>
      </Col>
      <Col>
        <StyledGreenGrey>
          <Row>
            <Col className="text-right">{feet === 0 ? 0 : formatNumber(feet, 4, 4)}</Col>
            <Col>sq. feet</Col>
          </Row>
          <Row>
            <Col className="text-right">{acres === 0 ? 0 : formatNumber(acres, 4, 4)}</Col>
            <Col>acres</Col>
          </Row>
        </StyledGreenGrey>
      </Col>
    </Row>
  );
};

export default AreaView;
