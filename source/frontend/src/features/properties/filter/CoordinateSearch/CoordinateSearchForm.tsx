import { Col, Container, Form } from 'react-bootstrap';
import styled from 'styled-components';

import { Input, Select } from '@/components/common/form';
import { withNameSpace } from '@/utils/formUtils';

import { DmsDirection } from './models';

export interface ICoordinateSearchProps {
  field: string;
}

export const CoordinateSearchForm: React.FunctionComponent<ICoordinateSearchProps> = ({
  field,
}) => {
  return (
    <Container fluid>
      <Form.Row className="d-flex align-items-center">
        <Col xs="auto">Lat:</Col>
        <StyledCol9Rem>
          <StyledInput
            field={withNameSpace(field, 'latitude.degrees')}
            displayErrorTooltips
          ></StyledInput>
        </StyledCol9Rem>
        &thinsp;°&thinsp;
        <StyledCol9Rem>
          <StyledInput
            field={withNameSpace(field, 'latitude.minutes')}
            displayErrorTooltips
          ></StyledInput>
        </StyledCol9Rem>
        &thinsp;’&thinsp;
        <StyledCol10Rem>
          <StyledInput
            field={withNameSpace(field, 'latitude.seconds')}
            displayErrorTooltips
          ></StyledInput>
        </StyledCol10Rem>
        &thinsp;”&thinsp;
        <Col xs="auto">
          <StyledSelect
            field={withNameSpace(field, 'latitude.direction')}
            options={[
              { label: 'N', value: DmsDirection.N },
              { label: 'S', value: DmsDirection.S },
            ]}
          ></StyledSelect>
        </Col>
        <Col xs="auto">Long:</Col>
        <StyledCol9Rem>
          <StyledInput
            field={withNameSpace(field, 'longitude.degrees')}
            displayErrorTooltips
          ></StyledInput>
        </StyledCol9Rem>
        &thinsp;°&thinsp;
        <StyledCol9Rem>
          <StyledInput
            field={withNameSpace(field, 'longitude.minutes')}
            displayErrorTooltips
          ></StyledInput>
        </StyledCol9Rem>
        &thinsp;’&thinsp;
        <StyledCol10Rem>
          <StyledInput
            field={withNameSpace(field, 'longitude.seconds')}
            displayErrorTooltips
          ></StyledInput>
        </StyledCol10Rem>
        &thinsp;”&thinsp;
        <Col xs="auto">
          <StyledSelect
            field={withNameSpace(field, 'longitude.direction')}
            options={[
              { label: 'W', value: DmsDirection.W },
              { label: 'E', value: DmsDirection.E },
            ]}
          ></StyledSelect>
        </Col>
      </Form.Row>
    </Container>
  );
};

const StyledSelect = styled(Select)`
  && .form-select {
    min-width: unset;
    border-radius: 0.4rem;
  }
`;

const StyledInput = styled(Input)`
  && .form-control {
    border-radius: 0.4rem;
  }
`;

const StyledCol9Rem = styled(Col)`
  max-width: 9rem;
`;

const StyledCol10Rem = styled(Col)`
  max-width: 10rem;
`;
