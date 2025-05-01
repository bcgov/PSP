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
  isNavActive: boolean;
  showText: boolean;
  onClick: () => void;
  roles?: Roles[];
  claims?: Claims[];
}

/**
 * Component that creates a nav, with an icon, and optional text.
 * @param {INavIconProps} param0
 */
export const NavIcon = ({
  icon,
  text,
  isNavActive,
  showText,
  onClick,
  roles,
  claims,
}: INavIconProps) => {
  const { hasRole, hasClaim } = useKeycloakWrapper();

  const displayIcon =
    find(roles, (role: Roles) => hasRole(role)) || find(claims, (claim: Claims) => hasClaim(claim));

  return (!roles?.length && !claims?.length) || displayIcon ? (
    <StyledNav
      onClick={onClick}
      data-testid={`nav-tooltip-${text.replaceAll(' ', '').toLowerCase()}`}
    >
      <StyledLink className={clsx({ active: isNavActive })}>
        <TooltipWrapper tooltipId={`nav-tooltip-${text}`} tooltip={text}>
          {icon}
        </TooltipWrapper>
        {showText && (
          <StyledLabel className={clsx({ show: showText, active: isNavActive })}>
            {text}
          </StyledLabel>
        )}
      </StyledLink>
    </StyledNav>
  ) : null;
};

const StyledNav = styled(Nav.Item)`
  width: 100%;
  margin-bottom: 2rem;
  fill: ${({ theme }) => theme.css.pimsWhite};

  &:hover {
    label {
      color: ${({ theme }) => theme.css.hoverActionColor};
    }

    svg {
      fill: ${({ theme }) => theme.css.hoverActionColor};
    }
  }
`;

const StyledLink = styled(Nav.Link)`
  display: flex;
  flex-direction: row;
  align-items: center;

  svg {
    fill: white;
    min-width: max-content;
  }

  &.active {
    svg,
    svg g,
    svg g path {
      fill: ${({ theme }) => theme.css.focusNavbarActionColor};
    }
  }
`;

const StyledLabel = styled.label`
  margin-left: 1rem;
  margin-bottom: 0;
  color: white;
  font-size: 1.2rem;
  word-break: break-word;
  white-space: break-spaces;
  transition: width 0.25s;
  width: 0;
  overflow: hidden;
  text-align: left;
  cursor: pointer;
  &.show {
    width: 100%;
  }
  &.active {
    color: ${({ theme }) => theme.css.focusNavbarActionColor};
  }
`;

export default NavIcon;
