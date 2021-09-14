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
    <Nav.Item onClick={onClick} data-testid={`nav-tooltip-${text}`}>
      <StyledLink>
        <TooltipWrapper toolTipId={`nav-tooltip-${text}`} toolTip={text}>
          {icon}
        </TooltipWrapper>
        {showText && <p>{text}</p>}
      </StyledLink>
    </Nav.Item>
  ) : null;
};

const StyledLink = styled(Nav.Link)`
  display: flex;
  flex-direction: row;
  align-items: center;
  p {
    margin-left: 0.25rem;
    margin-bottom: 0;
    font-size: 12px;
    color: white;
    white-space: nowrap;
  }
`;

export default NavIcon;
