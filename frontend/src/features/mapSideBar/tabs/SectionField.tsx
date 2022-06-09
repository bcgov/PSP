import clsx from 'classnames';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

interface ISectionFieldProps {
  label: string;
  className?: string;
  wideScreen?: boolean;
  required?: boolean;
}

export const SectionField: React.FunctionComponent<ISectionFieldProps> = props => {
  return (
    <Row className={props.className ?? 'pb-2'}>
      <Col xs={props.wideScreen ? 2 : 4}>
        <StyledFieldLabel>{props.label}:</StyledFieldLabel>
      </Col>
      <StyledCol className={clsx({ required: props.required })}>{props.children}</StyledCol>
    </Row>
  );
};
export const StyledCol = styled(Col)`
  &.required::before {
    content: '*';
    position: absolute;
    top: 0.75rem;
    left: 0rem;
  }
`;

export const StyledFieldLabel = styled.label`
  font-weight: bold;
`;
