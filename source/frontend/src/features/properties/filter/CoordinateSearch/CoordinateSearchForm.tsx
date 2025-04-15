import clsx from 'classnames';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { NumberInput, Select } from '@/components/common/form';
import { withNameSpace } from '@/utils/formUtils';

import { DmsDirection } from './models';

export interface ICoordinateSearchProps {
  field: string;
  innerClassName?: string;
}

export const CoordinateSearchForm: React.FunctionComponent<ICoordinateSearchProps> = ({
  field,
  innerClassName,
}) => {
  return (
    <StyledOuterRow
      noGutters
      className={clsx('d-flex', 'align-items-center', 'colum-gap-2', innerClassName)}
    >
      <Col xs={6}>
        <StyledInnerRow>
          <Col xs="auto" className="lat-lng-label">
            Lat:
          </Col>
          <Col>
            <StyledNumberInput
              className="dms-degree-input"
              field={withNameSpace(field, 'latitude.degrees')}
              onFocus={e => e.target.select()}
              displayErrorTooltips
            ></StyledNumberInput>
          </Col>
          °
          <Col>
            <StyledNumberInput
              className="dms-input"
              field={withNameSpace(field, 'latitude.minutes')}
              onFocus={e => e.target.select()}
              displayErrorTooltips
            ></StyledNumberInput>
          </Col>
          ’
          <Col>
            <StyledNumberInput
              className="dms-input"
              field={withNameSpace(field, 'latitude.seconds')}
              onFocus={e => e.target.select()}
              displayErrorTooltips
            ></StyledNumberInput>
          </Col>
          ”
          <Col xs="auto">
            <StyledSelect
              field={withNameSpace(field, 'latitude.direction')}
              options={[
                { label: 'N', value: DmsDirection.N },
                { label: 'S', value: DmsDirection.S },
              ]}
            ></StyledSelect>
          </Col>
        </StyledInnerRow>
      </Col>
      <Col xs={6}>
        <StyledInnerRow>
          <Col xs="auto" className="lat-lng-label">
            Long:
          </Col>
          <Col>
            <StyledNumberInput
              className="dms-degree-input"
              field={withNameSpace(field, 'longitude.degrees')}
              onFocus={e => e.target.select()}
              displayErrorTooltips
            ></StyledNumberInput>
          </Col>
          °
          <Col>
            <StyledNumberInput
              className="dms-input"
              field={withNameSpace(field, 'longitude.minutes')}
              onFocus={e => e.target.select()}
              displayErrorTooltips
            ></StyledNumberInput>
          </Col>
          ’
          <Col>
            <StyledNumberInput
              className="dms-input"
              field={withNameSpace(field, 'longitude.seconds')}
              onFocus={e => e.target.select()}
              displayErrorTooltips
            ></StyledNumberInput>
          </Col>
          ”
          <Col xs="auto">
            <StyledSelect
              field={withNameSpace(field, 'longitude.direction')}
              options={[
                { label: 'W', value: DmsDirection.W },
                { label: 'E', value: DmsDirection.E },
              ]}
            ></StyledSelect>
          </Col>
        </StyledInnerRow>
      </Col>
    </StyledOuterRow>
  );
};

const StyledSelect = styled(Select)`
  && .form-select {
    min-width: unset;
    border-radius: 0.4rem;
  }
`;

const StyledNumberInput = styled(NumberInput)`
  && .form-control {
    border-radius: 0.4rem;
    &.is-invalid {
      background-image: none;
      padding-right: 1.2rem !important;
    }
  }
`;

const StyledOuterRow = styled(Row)`
  padding: 0;
  .dms-input {
    min-width: 4rem;
    max-width: 4rem;
  }
  .dms-degree-input {
    min-width: 5rem;
    max-width: 5rem;
  }
  .lat-lng-label {
    min-width: 6rem;
    max-width: 6rem;
  }
`;

const StyledInnerRow = styled(Row)`
  margin-right: 0;
  margin-left: 0;
  flex-wrap: nowrap;

  > .col,
  > [class*='col-'] {
    padding-right: 0.5rem;
    padding-left: 0.5rem;
  }
`;
