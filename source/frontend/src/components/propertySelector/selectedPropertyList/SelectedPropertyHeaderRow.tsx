import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import TooltipIcon from '@/components/common/TooltipIcon';

interface ISelectedPropertyHeaderRowProps {}

export const SelectedPropertyHeaderRow: React.FunctionComponent<
  React.PropsWithChildren<ISelectedPropertyHeaderRowProps>
> = props => {
  return (
    <HeaderRow className="no-gutters">
      <Col md={3}>Identifier</Col>
      <Col md={9}>
        Provide a descriptive name for this land
        <TooltipIcon
          toolTipId="property-selector-tooltip"
          toolTip="Optionally - provide a user friendly description to identify the property, such as Highway 1"
        />
      </Col>
    </HeaderRow>
  );
};

const HeaderRow = styled(Row)`
  font-size: 1.6rem;
  color: ${props => props.theme.css.lightVariantColor};
  border-bottom: 0.2rem solid ${props => props.theme.css.lightVariantColor};
  margin-bottom: 0.9rem;
  padding-bottom: 0.25rem;
  font-family: 'BcSans-Bold';
`;

export default SelectedPropertyHeaderRow;
