import * as React from 'react';
import styled from 'styled-components';

interface IRolesToolTipProps {}

const RolesToolTip: React.FunctionComponent<
  React.PropsWithChildren<IRolesToolTipProps>
> = props => {
  return (
    <StyledTooltip>
      <StyledTooltipHeader>Role Descriptions</StyledTooltipHeader>
      <br />
      <br />
      <b>System Administrator</b>
      <p>System Administrator of the PIMS solution.</p>
      <br />
      <b>Finance</b>
      <p>Finance team members.</p>
      <br />
      <b>Functional</b>
      <p>PLMB staff (includes team members from HQ, regions and districts).</p>
      <br />
      <b>Functional (Restricted)</b>
      <p>Contractors, Internal ministry staff.</p>
      <br />
      <b>Read Only</b>
      <p>Other ministries (e.g. Attorney General).</p>
    </StyledTooltip>
  );
};

const StyledTooltip = styled.div`
  color: black;
  font-size: 1.6rem;
  p {
    margin: 0;
  }
`;

const StyledTooltipHeader = styled.b`
  color: ${({ theme }) => theme.css.primaryColor};
  font-size: 2.6rem;
`;

export default RolesToolTip;
