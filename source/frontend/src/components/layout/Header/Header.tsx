import Navbar from 'react-bootstrap/Navbar';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import { BCGovLogo } from '@/components/common/BCGovLogo';
import { VerticalBar } from '@/components/common/VerticalBar';
import HelpContainer from '@/features/help/containers/HelpContainer';
import NotificationBellContainer from '@/features/notifications/notificationsPopover/NotificationBellContainer';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Logo, useTenant } from '@/tenants';

import { HeaderStyled } from './HeaderStyled';
import { UserProfile } from './UserProfile';

/**
 * A header component that includes the navigation bar.
 * @returns Header component.
 */
export const Header = () => {
  const keycloak = useKeycloakWrapper();
  const tenant = useTenant();
  const isAuthenticated = !!keycloak.obj?.authenticated;

  return (
    <HeaderStyled expand className="App-header">
      <Navbar.Brand className="brand-box">
        <StyledContainer>
          <a
            target="_blank"
            rel="noopener noreferrer"
            href="https://www2.gov.bc.ca/gov/content/home"
          >
            <BCGovLogo />
          </a>
          <div>
            <VerticalBar />
          </div>
          <Link to="/mapview">
            <Logo height={50} />
          </Link>
          <div className="title">
            <label className="longAppName">{tenant.title}</label>
            <label className="shortAppName">{tenant.shortName}</label>
          </div>
        </StyledContainer>
      </Navbar.Brand>

      <HelpContainer />
      <div>
        <VerticalBar />
      </div>
      {isAuthenticated && <NotificationBellContainer />}
      {isAuthenticated && <UserProfile />}
      <div className="other"></div>
    </HeaderStyled>
  );
};

const StyledContainer = styled.div`
  display: flex;
  align-items: center;
  flex-direction: row;
  margin-left: 36px;
`;

export default Header;
