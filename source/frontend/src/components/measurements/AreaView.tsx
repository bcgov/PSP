import { Col, Row } from 'react-bootstrap';

import { ApiGen_CodeTypes_AreaUnitTypes } from '@/models/api/generated/ApiGen_CodeTypes_AreaUnitTypes';
import { convertArea, formatNumber } from '@/utils';

import { StyledGreenCol, StyledGreenGrey } from './styles';

interface IAreaViewProps {
  landArea?: number;
  unitCode?: string;
}

const AreaView: React.FunctionComponent<IAreaViewProps> = props => {
  const meters = convertArea(
    props.landArea || 0,
    props.unitCode || '',
    ApiGen_CodeTypes_AreaUnitTypes.M2,
  );
  const feet = convertArea(
    props.landArea || 0,
    props.unitCode || '',
    ApiGen_CodeTypes_AreaUnitTypes.FEET2,
  );
  const hectares = convertArea(
    props.landArea || 0,
    props.unitCode || '',
    ApiGen_CodeTypes_AreaUnitTypes.HA,
  );
  const acres = convertArea(
    props.landArea || 0,
    props.unitCode || '',
    ApiGen_CodeTypes_AreaUnitTypes.ACRE,
  );
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
