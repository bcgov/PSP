import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

interface ISectionFieldProps {
  label: string;
  className?: string;
}

export const SectionField: React.FunctionComponent<ISectionFieldProps> = props => {
  return (
    <Row className={props.className ?? 'pb-2'}>
      <Col xs="4">
        <StyledFieldLabel>{props.label}:</StyledFieldLabel>
      </Col>
      <Col>{props.children}</Col>
    </Row>
  );
};

export const StyledFieldLabel = styled.label`
  font-weight: bold;
`;
