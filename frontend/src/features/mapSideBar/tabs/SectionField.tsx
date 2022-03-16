import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

interface ISectionFieldProps {
  label: string;
  isCompact?: boolean;
}

export const SectionField: React.FunctionComponent<ISectionFieldProps> = props => {
  return (
    <Row className="pb-2">
      <Col xs={props.isCompact ? 'auto' : '4'}>
        <StyledFieldLabel>{props.label}:</StyledFieldLabel>
      </Col>
      <Col xs={props.isCompact ? 'auto' : false}>{props.children}</Col>
    </Row>
  );
};

export const StyledFieldLabel = styled.label`
  font-weight: bold;
`;
