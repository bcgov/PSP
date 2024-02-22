import React, { useCallback, useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';

import { AreaUnitTypes } from '@/constants/index';
import { convertArea, round } from '@/utils';

import { StyledGreenCol, StyledGreenGrey, StyledInput } from './styles';

export interface IAreaFormProps {
  area: number | undefined;
  areaUnitTypeCode: string | undefined;
  onChange: (landArea: number, areaUnitTypeCode: string) => void;
}

export const AreaForm: React.FC<IAreaFormProps> = ({
  area = 0,
  areaUnitTypeCode = AreaUnitTypes.Hectares,
  onChange,
}) => {
  // keep track of which input is receiving user input
  const [focus, setFocus] = useState('');
  const [state, setState] = useState<Record<string, number>>({});

  // derive our internal state from props
  useEffect(() => {
    const initialState: Record<string, number> = {
      [AreaUnitTypes.SquareMeters]: convertArea(area, areaUnitTypeCode, AreaUnitTypes.SquareMeters),
      [AreaUnitTypes.SquareFeet]: convertArea(area, areaUnitTypeCode, AreaUnitTypes.SquareFeet),
      [AreaUnitTypes.Hectares]: convertArea(area, areaUnitTypeCode, AreaUnitTypes.Hectares),
      [AreaUnitTypes.Acres]: convertArea(area, areaUnitTypeCode, AreaUnitTypes.Acres),
    };
    setState(initialState);
  }, [area, areaUnitTypeCode]);

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
    (newValue: number, areaUnitTypeCode: string) => {
      setFocus(areaUnitTypeCode);
      setState(prevState => ({ ...prevState, [areaUnitTypeCode]: newValue }));
      onChange(newValue, areaUnitTypeCode);
    },
    [onChange],
  );

  return (
    <>
      <Row>
        <Col>
          <StyledGreenCol>
            <Row className="pb-2">
              <Col className="text-right">
                <StyledInput
                  name="area-sq-meters"
                  aria-label="square metres"
                  type="number"
                  step=".01"
                  value={isNaN(sqMeters) ? 0 : sqMeters}
                  onChange={e =>
                    handleInputChange(e.target.valueAsNumber, AreaUnitTypes.SquareMeters)
                  }
                />
              </Col>
              <Col>sq. metres</Col>
            </Row>
            <Row>
              <Col className="text-right">
                <StyledInput
                  name="area-hectares"
                  aria-label="hectares"
                  type="number"
                  step=".01"
                  value={isNaN(ha) ? 0 : ha}
                  onChange={e => handleInputChange(e.target.valueAsNumber, AreaUnitTypes.Hectares)}
                />
              </Col>
              <Col>hectares</Col>
            </Row>
          </StyledGreenCol>
        </Col>
        <Col>
          <StyledGreenGrey>
            <Row className="pb-2">
              <Col className="text-right">
                <StyledInput
                  name="area-sq-feet"
                  aria-label="square feet"
                  type="number"
                  step=".01"
                  value={isNaN(sqFeet) ? 0 : sqFeet}
                  onChange={e =>
                    handleInputChange(e.target.valueAsNumber, AreaUnitTypes.SquareFeet)
                  }
                />
              </Col>
              <Col>sq. feet</Col>
            </Row>
            <Row>
              <Col className="text-right">
                <StyledInput
                  name="area-acres"
                  aria-label="acres"
                  type="number"
                  step=".01"
                  value={isNaN(acres) ? 0 : acres}
                  onChange={e => handleInputChange(e.target.valueAsNumber, AreaUnitTypes.Acres)}
                />
              </Col>
              <Col>acres</Col>
            </Row>
          </StyledGreenGrey>
        </Col>
      </Row>
    </>
  );
};
