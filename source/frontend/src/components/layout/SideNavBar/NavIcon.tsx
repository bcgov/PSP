import clsx from 'classnames';
import find from 'lodash/find';
import { Nav } from 'react-bootstrap';
import styled from 'styled-components';

import TooltipWrapper from '@/components/common/TooltipWrapper';
import { Claims, Roles } from '@/constants/index';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

interface INavIconProps {
  icon: React.ReactElement;
  text: string;
  showText: boolean;
  onClick: () => void;
  roles?: Roles[];
  claims?: Claims[];
}

/**
 * Component that creates a nav, with an icon, and optional text.
 * @param {INavIconProps} param0
 */
export const NavIcon = ({ icon, text, showText, onClick, roles, claims }: INavIconProps) => {
  const { hasRole, hasClaim } = useKeycloakWrapper();
  const displayIcon =
    find(roles, (role: Roles) => hasRole(role)) || find(claims, (claim: Claims) => hasClaim(claim));

  return (!roles?.length && !claims?.length) || displayIcon ? (
    <StyledNav
      onClick={onClick}
      data-testid={`nav-tooltip-${text.replaceAll(' ', '').toLowerCase()}`}
    >
      <StyledLink>
        <TooltipWrapper tooltipId={`nav-tooltip-${text}`} tooltip={text}>
          {icon}
        </TooltipWrapper>
        <StyledLabel className={clsx({ show: showText })}>{text}</StyledLabel>
      </StyledLink>
    </StyledNav>
  ) : null;
};

const StyledNav = styled(Nav.Item)`
  width: 100%;
`;

const StyledLink = styled(Nav.Link)`
  display: flex;
  flex-direction: row;
  align-items: center;
  svg {
    min-width: max-content;
  }
`;

const StyledLabel = styled.label`
  margin-left: 0.4rem;
  margin-bottom: 0;
  font-size: 1.2rem;
  color: white;
  white-space: nowrap;
  transition: width 0.25s;
  width: 0;
  overflow: hidden;
  text-align: left;
  cursor: pointer;
  &.show {
    width: 100%;
  }
`;

export default NavIcon;
