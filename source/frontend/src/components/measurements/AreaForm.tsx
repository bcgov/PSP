import React, { useCallback, useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';

import { ApiGen_CodeTypes_AreaUnitTypes } from '@/models/api/generated/ApiGen_CodeTypes_AreaUnitTypes';
import { convertArea, round } from '@/utils';

import { StyledGreenCol, StyledGreenGrey, StyledInput } from './styles';

export interface IAreaFormProps {
  area: number | undefined;
  areaUnitTypeCode: string | undefined;
  onChange: (landArea: number, areaUnitTypeCode: string) => void;
}

export const AreaForm: React.FC<IAreaFormProps> = ({
  area = 0,
  areaUnitTypeCode = ApiGen_CodeTypes_AreaUnitTypes.HA,
  onChange,
}) => {
  // keep track of which input is receiving user input
  const [focus, setFocus] = useState('');
  const [state, setState] = useState<Record<string, number>>({});

  // derive our internal state from props
  useEffect(() => {
    const initialState: Record<string, number> = {
      [ApiGen_CodeTypes_AreaUnitTypes.M2]: convertArea(
        area,
        areaUnitTypeCode,
        ApiGen_CodeTypes_AreaUnitTypes.M2,
      ),
      [ApiGen_CodeTypes_AreaUnitTypes.FEET2]: convertArea(
        area,
        areaUnitTypeCode,
        ApiGen_CodeTypes_AreaUnitTypes.FEET2,
      ),
      [ApiGen_CodeTypes_AreaUnitTypes.HA]: convertArea(
        area,
        areaUnitTypeCode,
        ApiGen_CodeTypes_AreaUnitTypes.HA,
      ),
      [ApiGen_CodeTypes_AreaUnitTypes.ACRE]: convertArea(
        area,
        areaUnitTypeCode,
        ApiGen_CodeTypes_AreaUnitTypes.ACRE,
      ),
    };
    setState(initialState);
  }, [area, areaUnitTypeCode]);

  const sqMeters = round(state[ApiGen_CodeTypes_AreaUnitTypes.M2], 4);
  const sqFeet = round(state[ApiGen_CodeTypes_AreaUnitTypes.FEET2], 4);
  const ha = round(state[ApiGen_CodeTypes_AreaUnitTypes.HA], 4);
  const acres = round(state[ApiGen_CodeTypes_AreaUnitTypes.ACRE], 4);

  // update dependent fields based on user input
  useEffect(() => {
    if (focus === ApiGen_CodeTypes_AreaUnitTypes.M2) {
      setState(prevState => {
        const value = prevState[ApiGen_CodeTypes_AreaUnitTypes.M2];
        const newSqFeet = convertArea(
          value,
          ApiGen_CodeTypes_AreaUnitTypes.M2,
          ApiGen_CodeTypes_AreaUnitTypes.FEET2,
        );
        const newHa = convertArea(
          value,
          ApiGen_CodeTypes_AreaUnitTypes.M2,
          ApiGen_CodeTypes_AreaUnitTypes.HA,
        );
        const newAcres = convertArea(
          value,
          ApiGen_CodeTypes_AreaUnitTypes.M2,
          ApiGen_CodeTypes_AreaUnitTypes.ACRE,
        );

        return {
          [ApiGen_CodeTypes_AreaUnitTypes.M2]: value,
          [ApiGen_CodeTypes_AreaUnitTypes.FEET2]: newSqFeet,
          [ApiGen_CodeTypes_AreaUnitTypes.HA]: newHa,
          [ApiGen_CodeTypes_AreaUnitTypes.ACRE]: newAcres,
        };
      });
    }
  }, [focus, sqMeters]);

  useEffect(() => {
    if (focus === ApiGen_CodeTypes_AreaUnitTypes.FEET2) {
      setState(prevState => {
        const value = prevState[ApiGen_CodeTypes_AreaUnitTypes.FEET2];
        const newSqMeters = convertArea(
          value,
          ApiGen_CodeTypes_AreaUnitTypes.FEET2,
          ApiGen_CodeTypes_AreaUnitTypes.M2,
        );
        const newHa = convertArea(
          value,
          ApiGen_CodeTypes_AreaUnitTypes.FEET2,
          ApiGen_CodeTypes_AreaUnitTypes.HA,
        );
        const newAcres = convertArea(
          value,
          ApiGen_CodeTypes_AreaUnitTypes.FEET2,
          ApiGen_CodeTypes_AreaUnitTypes.ACRE,
        );

        return {
          [ApiGen_CodeTypes_AreaUnitTypes.M2]: newSqMeters,
          [ApiGen_CodeTypes_AreaUnitTypes.FEET2]: value,
          [ApiGen_CodeTypes_AreaUnitTypes.HA]: newHa,
          [ApiGen_CodeTypes_AreaUnitTypes.ACRE]: newAcres,
        };
      });
    }
  }, [focus, sqFeet]);

  useEffect(() => {
    if (focus === ApiGen_CodeTypes_AreaUnitTypes.HA) {
      setState(prevState => {
        const value = prevState[ApiGen_CodeTypes_AreaUnitTypes.HA];
        const newSqMeters = convertArea(
          value,
          ApiGen_CodeTypes_AreaUnitTypes.HA,
          ApiGen_CodeTypes_AreaUnitTypes.M2,
        );
        const newSqFeet = convertArea(
          value,
          ApiGen_CodeTypes_AreaUnitTypes.HA,
          ApiGen_CodeTypes_AreaUnitTypes.FEET2,
        );
        const newAcres = convertArea(
          value,
          ApiGen_CodeTypes_AreaUnitTypes.HA,
          ApiGen_CodeTypes_AreaUnitTypes.ACRE,
        );

        return {
          [ApiGen_CodeTypes_AreaUnitTypes.M2]: newSqMeters,
          [ApiGen_CodeTypes_AreaUnitTypes.FEET2]: newSqFeet,
          [ApiGen_CodeTypes_AreaUnitTypes.HA]: value,
          [ApiGen_CodeTypes_AreaUnitTypes.ACRE]: newAcres,
        };
      });
    }
  }, [focus, ha]);

  useEffect(() => {
    if (focus === ApiGen_CodeTypes_AreaUnitTypes.ACRE) {
      setState(prevState => {
        const value = prevState[ApiGen_CodeTypes_AreaUnitTypes.ACRE];
        const newSqMeters = convertArea(
          value,
          ApiGen_CodeTypes_AreaUnitTypes.ACRE,
          ApiGen_CodeTypes_AreaUnitTypes.M2,
        );
        const newSqFeet = convertArea(
          value,
          ApiGen_CodeTypes_AreaUnitTypes.ACRE,
          ApiGen_CodeTypes_AreaUnitTypes.FEET2,
        );
        const newHa = convertArea(
          value,
          ApiGen_CodeTypes_AreaUnitTypes.ACRE,
          ApiGen_CodeTypes_AreaUnitTypes.HA,
        );

        return {
          [ApiGen_CodeTypes_AreaUnitTypes.M2]: newSqMeters,
          [ApiGen_CodeTypes_AreaUnitTypes.FEET2]: newSqFeet,
          [ApiGen_CodeTypes_AreaUnitTypes.HA]: newHa,
          [ApiGen_CodeTypes_AreaUnitTypes.ACRE]: value,
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
                    handleInputChange(e.target.valueAsNumber, ApiGen_CodeTypes_AreaUnitTypes.M2)
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
                  onChange={e =>
                    handleInputChange(e.target.valueAsNumber, ApiGen_CodeTypes_AreaUnitTypes.HA)
                  }
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
                    handleInputChange(e.target.valueAsNumber, ApiGen_CodeTypes_AreaUnitTypes.FEET2)
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
                  onChange={e =>
                    handleInputChange(e.target.valueAsNumber, ApiGen_CodeTypes_AreaUnitTypes.ACRE)
                  }
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
