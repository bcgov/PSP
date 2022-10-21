import { NoPaddingRow } from 'components/common/styles';
import TooltipIcon from 'components/common/TooltipIcon';
import * as React from 'react';
import { Col } from 'react-bootstrap';
import styled from 'styled-components';

interface ISelectedPropertyHeaderRowProps {}

export const SelectedPropertyHeaderRow: React.FunctionComponent<
  ISelectedPropertyHeaderRowProps
> = props => {
  return (
    <HeaderRow>
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

const HeaderRow = styled(NoPaddingRow)`
  font-size: 1.6rem;
  color: ${props => props.theme.css.lightVariantColor};
  border-bottom: 0.2rem solid ${props => props.theme.css.lightVariantColor};
  margin-bottom: 0.9rem;
  padding-bottom: 0.25rem;
  font-family: 'BcSans-Bold';
`;

export default SelectedPropertyHeaderRow;
