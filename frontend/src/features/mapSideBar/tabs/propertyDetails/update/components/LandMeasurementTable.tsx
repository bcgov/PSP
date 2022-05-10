import { AreaUnitTypes } from 'constants/index';
import { TableCaption } from 'features/mapSideBar/tabs/SectionStyles';
import Api_TypeCode from 'models/api/TypeCode';
import React, { useCallback, useEffect, useState } from 'react';
import { convertArea, round } from 'utils';

import { StyledTable } from '../styles';

export interface IUpdateLandMeasurementTableProps {
  area: number;
  areaUnit: Api_TypeCode<string>;
  onChange?: (landArea: number, areaUnit: Api_TypeCode<string>) => void;
}

export const LandMeasurementTable: React.FC<IUpdateLandMeasurementTableProps> = ({
  area,
  areaUnit,
  onChange,
}) => {
  const areaUnitId = areaUnit.id || AreaUnitTypes.Hectares;

  // derive our internal state from props
  const initialState: Record<string, number> = {
    [AreaUnitTypes.SquareMeters]: convertArea(area, areaUnitId, AreaUnitTypes.SquareMeters),
    [AreaUnitTypes.SquareFeet]: convertArea(area, areaUnitId, AreaUnitTypes.SquareFeet),
    [AreaUnitTypes.Hectares]: convertArea(area, areaUnitId, AreaUnitTypes.Hectares),
    [AreaUnitTypes.Acres]: convertArea(area, areaUnitId, AreaUnitTypes.Acres),
  };

  // keep track of which input is receiving user input
  const [focus, setFocus] = useState('');
  const [state, setState] = useState(initialState);

  const sqMeters = round(state[AreaUnitTypes.SquareMeters], 2);
  const sqFeet = round(state[AreaUnitTypes.SquareFeet], 2);
  const ha = round(state[AreaUnitTypes.Hectares], 2);
  const acres = round(state[AreaUnitTypes.Acres], 2);

  // update dependent fields based on user input
  useEffect(() => {
    if (focus === AreaUnitTypes.SquareMeters) {
      setState(prevState => {
        const value = prevState[AreaUnitTypes.SquareMeters];
        const newSqFeet = convertArea(value, AreaUnitTypes.SquareMeters, AreaUnitTypes.SquareFeet);
        const newHa = convertArea(value, AreaUnitTypes.SquareMeters, AreaUnitTypes.Hectares);
        const newAcres = convertArea(value, AreaUnitTypes.SquareMeters, AreaUnitTypes.Acres);

        return {
          [AreaUnitTypes.SquareMeters]: value,
          [AreaUnitTypes.SquareFeet]: newSqFeet,
          [AreaUnitTypes.Hectares]: newHa,
          [AreaUnitTypes.Acres]: newAcres,
        };
      });
    }
  }, [focus, sqMeters]);

  useEffect(() => {
    if (focus === AreaUnitTypes.SquareFeet) {
      setState(prevState => {
        const value = prevState[AreaUnitTypes.SquareFeet];
        const newSqMeters = convertArea(
          value,
          AreaUnitTypes.SquareFeet,
          AreaUnitTypes.SquareMeters,
        );
        const newHa = convertArea(value, AreaUnitTypes.SquareFeet, AreaUnitTypes.Hectares);
        const newAcres = convertArea(value, AreaUnitTypes.SquareFeet, AreaUnitTypes.Acres);

        return {
          [AreaUnitTypes.SquareMeters]: newSqMeters,
          [AreaUnitTypes.SquareFeet]: value,
          [AreaUnitTypes.Hectares]: newHa,
          [AreaUnitTypes.Acres]: newAcres,
        };
      });
    }
  }, [focus, sqFeet]);

  useEffect(() => {
    if (focus === AreaUnitTypes.Hectares) {
      setState(prevState => {
        const value = prevState[AreaUnitTypes.Hectares];
        const newSqMeters = convertArea(value, AreaUnitTypes.Hectares, AreaUnitTypes.SquareMeters);
        const newSqFeet = convertArea(value, AreaUnitTypes.Hectares, AreaUnitTypes.SquareFeet);
        const newAcres = convertArea(value, AreaUnitTypes.Hectares, AreaUnitTypes.Acres);

        return {
          [AreaUnitTypes.SquareMeters]: newSqMeters,
          [AreaUnitTypes.SquareFeet]: newSqFeet,
          [AreaUnitTypes.Hectares]: value,
          [AreaUnitTypes.Acres]: newAcres,
        };
      });
    }
  }, [focus, ha]);

  useEffect(() => {
    if (focus === AreaUnitTypes.Acres) {
      setState(prevState => {
        const value = prevState[AreaUnitTypes.Acres];
        const newSqMeters = convertArea(value, AreaUnitTypes.Acres, AreaUnitTypes.SquareMeters);
        const newSqFeet = convertArea(value, AreaUnitTypes.Acres, AreaUnitTypes.SquareFeet);
        const newHa = convertArea(value, AreaUnitTypes.Acres, AreaUnitTypes.Hectares);

        return {
          [AreaUnitTypes.SquareMeters]: newSqMeters,
          [AreaUnitTypes.SquareFeet]: newSqFeet,
          [AreaUnitTypes.Hectares]: newHa,
          [AreaUnitTypes.Acres]: value,
        };
      });
    }
  }, [focus, acres]);

  const handleInputChange = useCallback(
    (newValue: number, areaUnitId: string) => {
      setFocus(areaUnitId);
      setState(prevState => ({ ...prevState, [areaUnitId]: newValue }));
      if (typeof onChange === 'function') {
        onChange(newValue, { id: areaUnitId });
      }
    },
    [onChange],
  );

  return (
    <>
      <TableCaption>Land measurement</TableCaption>
      <StyledTable role="table" className="table">
        <div role="rowgroup" className="tbody">
          <div className="tr-wrapper">
            <div role="row" className="tr">
              <div role="cell" title="" className="td right">
                <input
                  type="number"
                  step=".01"
                  value={sqMeters}
                  onChange={e =>
                    handleInputChange(e.target.valueAsNumber, AreaUnitTypes.SquareMeters)
                  }
                />
              </div>
              <div role="cell" title="" className="td left">
                sq. metres
              </div>
            </div>
          </div>
          <div className="tr-wrapper">
            <div role="row" className="tr">
              <div role="cell" title="" className="td right">
                <input
                  type="number"
                  step=".01"
                  value={sqFeet}
                  onChange={e =>
                    handleInputChange(e.target.valueAsNumber, AreaUnitTypes.SquareFeet)
                  }
                />
              </div>
              <div role="cell" title="" className="td left">
                sq. feet
              </div>
            </div>
          </div>
          <div className="tr-wrapper">
            <div role="row" className="tr">
              <div role="cell" title="" className="td right">
                <input
                  type="number"
                  step=".01"
                  value={ha}
                  onChange={e => handleInputChange(e.target.valueAsNumber, AreaUnitTypes.Hectares)}
                />
              </div>
              <div role="cell" title="" className="td left">
                hectares
              </div>
            </div>
          </div>
          <div className="tr-wrapper">
            <div role="row" className="tr">
              <div role="cell" title="" className="td right">
                <input
                  type="number"
                  step=".01"
                  value={acres}
                  onChange={e => handleInputChange(e.target.valueAsNumber, AreaUnitTypes.Acres)}
                />
              </div>
              <div role="cell" title="" className="td left">
                acres
              </div>
            </div>
          </div>
        </div>
      </StyledTable>
    </>
  );
};
