import React, { useCallback, useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';

import { VolumeUnitTypes } from '@/constants/index';
import { convertVolume, round } from '@/utils';

import { StyledGreenBlue, StyledInput } from './styles';

export interface IVolumeFormProps {
  volume?: number;
  volumeUnitTypeCode?: string;
  onChange: (volume: number, volumeUnitTypeCode: string) => void;
}

export const VolumeForm: React.FC<IVolumeFormProps> = ({
  volume = 0,
  volumeUnitTypeCode = VolumeUnitTypes.CubicMeters,
  onChange,
}) => {
  // derive our internal state from props
  const initialState: Record<string, number> = {
    [VolumeUnitTypes.CubicMeters]: convertVolume(
      volume,
      volumeUnitTypeCode,
      VolumeUnitTypes.CubicMeters,
    ),
    [VolumeUnitTypes.CubicFeet]: convertVolume(
      volume,
      volumeUnitTypeCode,
      VolumeUnitTypes.CubicFeet,
    ),
  };

  // keep track of which input is receiving user input
  const [focus, setFocus] = useState('');
  const [state, setState] = useState(initialState);

  const cubicMeters = round(state[VolumeUnitTypes.CubicMeters], 2);
  const cubicFeet = round(state[VolumeUnitTypes.CubicFeet], 2);

  // update dependent fields based on user input
  useEffect(() => {
    if (focus === VolumeUnitTypes.CubicMeters) {
      setState(prevState => {
        const value = prevState[VolumeUnitTypes.CubicMeters];
        const newCubicFeet = convertVolume(
          value,
          VolumeUnitTypes.CubicMeters,
          VolumeUnitTypes.CubicFeet,
        );

        return {
          [VolumeUnitTypes.CubicMeters]: value,
          [VolumeUnitTypes.CubicFeet]: newCubicFeet,
        };
      });
    }
  }, [focus, cubicMeters]);

  useEffect(() => {
    if (focus === VolumeUnitTypes.CubicFeet) {
      setState(prevState => {
        const value = prevState[VolumeUnitTypes.CubicFeet];
        const newCubicMeters = convertVolume(
          value,
          VolumeUnitTypes.CubicFeet,
          VolumeUnitTypes.CubicMeters,
        );

        return {
          [VolumeUnitTypes.CubicMeters]: newCubicMeters,
          [VolumeUnitTypes.CubicFeet]: value,
        };
      });
    }
  }, [focus, cubicFeet]);

  const handleInputChange = useCallback(
    (newValue: number, volumeUnitTypeCode: string) => {
      setFocus(volumeUnitTypeCode);
      setState(prevState => ({ ...prevState, [volumeUnitTypeCode]: newValue }));

      onChange(newValue, volumeUnitTypeCode);
    },
    [onChange],
  );

  return (
    <>
      <Row>
        <Col>
          <StyledGreenBlue>
            <Row className="pb-2">
              <Col className="text-right">
                <StyledInput
                  name="volume-cubic-meters"
                  aria-label="cubic metres"
                  type="number"
                  step=".01"
                  value={cubicMeters}
                  onChange={e =>
                    handleInputChange(e.target.valueAsNumber, VolumeUnitTypes.CubicMeters)
                  }
                />
              </Col>
              <Col>
                <span>
                  metres<sup>3</sup>
                </span>
              </Col>
            </Row>
            <Row>
              <Col className="text-right">
                <StyledInput
                  name="volume-cubic-feet"
                  aria-label="cubic feet"
                  type="number"
                  step=".01"
                  value={cubicFeet}
                  onChange={e =>
                    handleInputChange(e.target.valueAsNumber, VolumeUnitTypes.CubicFeet)
                  }
                />
              </Col>
              <Col>
                <span>
                  feet<sup>3</sup>
                </span>
              </Col>
            </Row>
          </StyledGreenBlue>
        </Col>
      </Row>
    </>
  );
};
