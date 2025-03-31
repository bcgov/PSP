import clsx from 'classnames';
import React from 'react';
import { Col, ColProps, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { renderTooltip } from '@/utils/formUtils';

export interface IHeaderLabelColProps {
  label: string | null;
  /** It accepts either a string or a custom React tooltip component  */
  tooltip?: React.ReactNode;
  labelWidth?: ColProps;
  className?: string;
}

export interface IHeaderContentColProps {
  contentWidth?: ColProps;
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
  <Col
    {...(props.labelWidth ?? { xs: 'auto' })}
    className={clsx('pr-0', 'text-left', props.className)}
  >
    {props.label && (
      <StyledHeaderLabel>
        {props.label}
        {props.tooltip && <span>{renderTooltip(props.tooltip)}</span>}
      </StyledHeaderLabel>
    )}
  </Col>
);

export const HeaderContentCol: React.FC<
  React.PropsWithChildren<IHeaderContentColProps>
> = props => {
  return (
    <Col {...props.contentWidth} className="pl-1 text-left" data-testid={props.valueTestId}>
      {props.children}
    </Col>
  );
};

const StyledHeaderLabel = styled.label`
  font-weight: bold;
  white-space: nowrap;
`;
