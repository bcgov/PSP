import { AreaUnitTypes } from 'constants/index';
import { TableCaption } from 'features/mapSideBar/tabs/SectionStyles';
import Api_TypeCode from 'models/api/TypeCode';
import React, { useEffect, useMemo, useState } from 'react';
import styled from 'styled-components';
import { formatNumber } from 'utils';
import { convertArea } from 'utils/convertUtils';

import { createColumns, generateTableData, roundToTwoDecimals } from './helpers';

export interface IUpdateLandMeasurementTableProps {
  landArea: number;
  areaUnit: Api_TypeCode<string>;
  onChange?: (landArea: number, areaUnit: Api_TypeCode<string>) => void;
}

export const LandMeasurementTable: React.FC<IUpdateLandMeasurementTableProps> = props => {
  const { landArea, areaUnit, onChange } = props;

  // populate data array from input props
  const data = useMemo(() => generateTableData(landArea, areaUnit), [areaUnit, landArea]);

  // derive our internal state from values in the data table above
  const initialState = data.reduce((dictionary, next) => {
    dictionary[next.unit] = next.value;
    return dictionary;
  }, {} as Record<string, number>);

  // keep track of which input is receiving user input
  const [focus, setFocus] = useState('');
  const [state, setState] = useState(initialState);

  const sqMeters = roundToTwoDecimals(state[AreaUnitTypes.SquareMeters]);
  const sqFeet = roundToTwoDecimals(state[AreaUnitTypes.SquareFeet]);
  const ha = roundToTwoDecimals(state[AreaUnitTypes.Hectares]);
  const acres = roundToTwoDecimals(state[AreaUnitTypes.Acres]);

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
                  onChange={e => {
                    setFocus(AreaUnitTypes.SquareMeters);
                    setState(prevState => ({
                      ...prevState,
                      [AreaUnitTypes.SquareMeters]: e.target.valueAsNumber,
                    }));
                  }}
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
                  onChange={e => {
                    setFocus(AreaUnitTypes.SquareFeet);
                    setState(prevState => ({
                      ...prevState,
                      [AreaUnitTypes.SquareFeet]: e.target.valueAsNumber,
                    }));
                  }}
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
                  onChange={e => {
                    setFocus(AreaUnitTypes.Hectares);
                    setState(prevState => ({
                      ...prevState,
                      [AreaUnitTypes.Hectares]: e.target.valueAsNumber,
                    }));
                  }}
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
                  onChange={e => {
                    setFocus(AreaUnitTypes.Acres);
                    setState(prevState => ({
                      ...prevState,
                      [AreaUnitTypes.Acres]: e.target.valueAsNumber,
                    }));
                  }}
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

const StyledTable = styled.div`
  &.table {
    .no-rows-message {
      align-items: flex-start;
    }
    .tbody {
      .tr {
        display: flex;
        flex: 1 0 auto;
        min-width: 60px;

        .td {
          display: flex;
          flex: 100 0 auto;
          box-sizing: border-box;
          min-height: 3.6rem;
          min-width: 30px;
          width: 100px;
          flex-wrap: wrap;
          align-items: center;

          &.right {
            justify-content: right;
            text-align: right;
          }
          &.left {
            justify-content: left;
            text-align: left;
          }

          input {
            max-width: 12.5rem;
          }
        }
      }
    }
  }
`;
