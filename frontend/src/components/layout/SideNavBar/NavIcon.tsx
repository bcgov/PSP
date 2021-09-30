import clsx from 'classnames';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { Roles } from 'constants/index';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import _ from 'lodash';
import { Nav } from 'react-bootstrap';
import styled from 'styled-components';

interface INavIconProps {
  icon: React.ReactElement;
  text: string;
  showText: boolean;
  onClick: () => void;
  roles?: Roles[];
}

/**
 * Component that creates a nav, with an icon, and optional text.
 * @param {INavIconProps} param0
 */
export const NavIcon = ({ icon, text, showText, onClick, roles }: INavIconProps) => {
  const { hasRole } = useKeycloakWrapper();
  const displayIcon = _.find(roles, (role: Roles) => hasRole(role));

  return !roles?.length || displayIcon ? (
    <StyledNav onClick={onClick} data-testid={`nav-tooltip-${text.replace(' ', '').toLowerCase()}`}>
      <StyledLink>
        <TooltipWrapper toolTipId={`nav-tooltip-${text}`} toolTip={text}>
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
