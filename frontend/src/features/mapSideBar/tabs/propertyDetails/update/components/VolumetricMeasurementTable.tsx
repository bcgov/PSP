import { VolumeUnitTypes } from 'constants/index';
import { TableCaption } from 'features/mapSideBar/tabs/SectionStyles';
import React, { useCallback, useEffect, useState } from 'react';
import { convertVolume, round } from 'utils';

import { StyledTable } from '../styles';

export interface IVolumetricMeasurementTableProps {
  volume?: number;
  volumeUnitTypeCode?: string;
  onChange?: (volume: number, volumeUnitTypeCode: string) => void;
}

export const VolumetricMeasurementTable: React.FC<IVolumetricMeasurementTableProps> = ({
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
      if (typeof onChange === 'function') {
        onChange(newValue, volumeUnitTypeCode);
      }
    },
    [onChange],
  );

  return (
    <>
      <TableCaption>Volumetric measurement</TableCaption>
      <StyledTable role="table" className="table">
        <div role="rowgroup" className="tbody">
          <div className="tr-wrapper">
            <div role="row" className="tr">
              <div role="cell" title="" className="td right">
                <input
                  type="number"
                  step=".01"
                  value={cubicMeters}
                  onChange={e =>
                    handleInputChange(e.target.valueAsNumber, VolumeUnitTypes.CubicMeters)
                  }
                />
              </div>
              <div role="cell" title="" className="td left">
                <span>
                  metres<sup>3</sup>
                </span>
              </div>
            </div>
          </div>
          <div className="tr-wrapper">
            <div role="row" className="tr">
              <div role="cell" title="" className="td right">
                <input
                  type="number"
                  step=".01"
                  value={cubicFeet}
                  onChange={e =>
                    handleInputChange(e.target.valueAsNumber, VolumeUnitTypes.CubicFeet)
                  }
                />
              </div>
              <div role="cell" title="" className="td left">
                <span>
                  feet<sup>3</sup>
                </span>
              </div>
            </div>
          </div>
        </div>
      </StyledTable>
    </>
  );
};
