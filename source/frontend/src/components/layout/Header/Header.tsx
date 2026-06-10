import { useEffect } from 'react';
import Navbar from 'react-bootstrap/Navbar';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import { BCGovLogo } from '@/components/common/BCGovLogo';
import { VerticalBar } from '@/components/common/VerticalBar';
import HelpContainer from '@/features/help/containers/HelpContainer';
import NotificationsBell from '@/features/notifications/notificationsPopover/NotificationsBell';
import { useNotificationInboxRepository } from '@/hooks/repositories/useNotificationInboxRepository';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Logo, useTenant } from '@/tenants';
import { exists } from '@/utils';

import { HeaderStyled } from './HeaderStyled';
import { UserProfile } from './UserProfile';

/**
 * A header component that includes the navigation bar.
 * @returns Header component.
 */
export const Header = () => {
  const keycloak = useKeycloakWrapper();
  const tenant = useTenant();
  const isAuthenticated = exists(keycloak.obj?.authenticated);

  const {
    getUnreadCount: { execute: getUnreadCount, response: unreadCount },
  } = useNotificationInboxRepository();

  // Keep the badge count fresh on mount; refresh again whenever the popover opens
  // so stale counts don't linger if multiple tabs are open.
  useEffect(() => {
    getUnreadCount();
  }, [getUnreadCount]);

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
      {isAuthenticated && <NotificationsBell unreadCount={unreadCount} />}
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
