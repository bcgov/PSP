import styled from 'styled-components';

const RolesToolTip: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  return (
    <StyledTooltip>
      <StyledTooltipHeader>Role Descriptions</StyledTooltipHeader>
      <br />
      <br />
      <b>System Administrator</b>
      <p>
        System administrator of the PIMS solution with view, create, update and delete access
        through out the system including elevated access to override some system based restrictions.
      </p>
      <br />
      <b>Functional</b>
      <p>
        Ability to view, create, update and delete entities corresponding to the respective business
        process, generally granted to PLMB staff and equivalent contractors from HQ, regions and
        district.
      </p>
      <br />
      <b>Read Only</b>
      <p>Ability to view entities corresponding to the respective business process.</p>
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
