import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

interface ISectionFieldProps {
  label: string;
  content: string[];
  isMultiLine?: boolean;
}

export const SectionField: React.FunctionComponent<ISectionFieldProps> = props => {
  return (
    <Row className="pb-2">
      <Col xs="4">
        <StyledFieldLabel>{props.label}:</StyledFieldLabel>
      </Col>
      <Col>
        {props.content.map((item, index) =>
          props.isMultiLine === true ? (
            <Row key={`label ${props.label} ${index}`}>
              <Col>
                <span>{item}</span>
              </Col>
            </Row>
          ) : (
            <span key={`label ${props.label} ${index}`} className="mr-2">
              {item}
            </span>
          ),
        )}
      </Col>
    </Row>
  );
};

const StyledFieldLabel = styled.div`
  font-weight: bold;
`;
