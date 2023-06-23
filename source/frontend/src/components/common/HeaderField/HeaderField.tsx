import React from 'react';
import { Col, Row } from 'react-bootstrap';

export interface IHeaderLabelColProps {
  label: string;
  labelWidth?: '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' | '10' | '11' | '12' | 'auto';
}

export interface IHeaderContentColProps {
  contentWidth?: '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' | '10' | '11' | '12' | 'auto';
  valueTestId?: string | null;
}

export interface IHeaderFieldProps extends IHeaderLabelColProps, IHeaderContentColProps {
  className?: string;
  valueTestId?: string | null;
}

export const HeaderField: React.FC<React.PropsWithChildren<IHeaderFieldProps>> = props => {
  return (
    <Row className={props.className}>
      <HeaderLabelCol label={props.label} labelWidth={props.labelWidth} />
      <HeaderContentCol contentWidth={props.contentWidth} valueTestId={props.valueTestId}>
        {props.children}
      </HeaderContentCol>
    </Row>
  );
};

export const HeaderLabelCol: React.FC<IHeaderLabelColProps> = props => (
  <Col xs={props.labelWidth ?? 'auto'} className="pr-0 text-left">
    <label>{props.label}</label>
  </Col>
);

export const HeaderContentCol: React.FC<
  React.PropsWithChildren<IHeaderContentColProps>
> = props => {
  return (
    <Col
      xs={props.contentWidth ?? 'auto'}
      className="pl-1 text-left"
      data-testid={props.valueTestId}
    >
      <strong>{props.children}</strong>
    </Col>
  );
};
