import { Col, Row } from 'react-bootstrap';

import { VolumeUnitTypes } from '@/constants/volumeUnitTypes';
import { convertVolume, formatNumber } from '@/utils';

import { StyledGreenBlue } from './styles';

interface IVolumeViewProps {
  volume?: number;
  unitCode?: string;
}

const VolumeView: React.FunctionComponent<IVolumeViewProps> = ({ volume, unitCode }) => {
  const meters = convertVolume(volume || 0, unitCode || '', VolumeUnitTypes.CubicMeters);
  const feet = convertVolume(volume || 0, unitCode || '', VolumeUnitTypes.CubicFeet);

  return (
    <Row>
      <Col>
        <StyledGreenBlue>
          <Row>
            <Col className="text-right">{formatNumber(meters)}</Col>
            <Col>
              <span>
                metres<sup>3</sup>
              </span>
            </Col>
          </Row>
          <Row>
            <Col className="text-right">{formatNumber(feet)}</Col>
            <Col>
              <span>
                feet<sup>3</sup>
              </span>
            </Col>
          </Row>
        </StyledGreenBlue>
      </Col>
    </Row>
  );
};

export default VolumeView;
