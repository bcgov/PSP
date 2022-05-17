import { Col, Row } from 'react-bootstrap';

interface IHeaderFieldProps {
  label: string;
  className?: string;
  labelWidth?: '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' | '10' | '11' | '12' | 'auto';
}

export const HeaderField: React.FunctionComponent<IHeaderFieldProps> = props => {
  return (
    <Row className={props.className}>
      <Col xs={props.labelWidth ?? 'auto'} className="pr-0 text-left">
        <label>{props.label}</label>
      </Col>
      <Col xs="auto">
        <strong>{props.children}</strong>
      </Col>
    </Row>
  );
};
