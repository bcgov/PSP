import { Col, Row } from 'react-bootstrap';

interface IHeaderFieldProps {
  label: string;
  className?: string;
}

export const HeaderField: React.FunctionComponent<IHeaderFieldProps> = props => {
  return (
    <Row className={props.className}>
      <Col xs="auto" className="pr-0">
        <label>{props.label}</label>
      </Col>
      <Col xs="auto">
        <strong>{props.children}</strong>
      </Col>
    </Row>
  );
};
