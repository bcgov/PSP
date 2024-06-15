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
            <Col className="text-right">{meters === 0 ? 0 : formatNumber(meters, 4, 4)}</Col>
            <Col>
              <span>
                metres<sup>3</sup>
              </span>
            </Col>
          </Row>
          <Row>
            <Col className="text-right">{feet === 0 ? 0 : formatNumber(feet, 4, 4)}</Col>
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
