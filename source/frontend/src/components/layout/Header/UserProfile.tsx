import React from 'react';
import Image from 'react-bootstrap/Image';
import NavDropdown from 'react-bootstrap/NavDropdown';
import { FaSignOutAlt } from 'react-icons/fa';
import styled from 'styled-components';

import profileUrl from '@/assets/images/profile.svg';
import variables from '@/assets/scss/_variables.module.scss';
import { useConfiguration } from '@/hooks/useConfiguration';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

/** the styling for the dropdown menu that appears after clicking the user's name */
const StyleDropDown = styled(NavDropdown)`
  font-size: 1.4rem;
  .dropdown-menu {
    width: 30rem;
    padding: 0rem;
  }
  .nav-link {
    color: #fff;
    padding: 0.1rem;
  }
  .dropdown-item {
    background-color: ${variables.primaryLightColor};
    border-top: 0.2rem solid ${variables.accentColor};
  }
`;

/** shaded box the users system roles will be displayed in */
const RolesBox = styled.div`
  background-color: ${variables.filterBackgroundColor};
  margin: 0.5rem;
`;

/** the text contained in the logout footer  */
const LogoutText = styled.p`
  color: #fff;
  margin-top: 0.8rem;
  margin-left: 12rem;
`;

/** the styling for the avatar next to user's name */
const ProfileAvatar = styled(Image)`
  height: 3rem;
  width: 3rem;
`;

/** the styling for the logout icon in the logout footer */
const LogoutButton = styled(FaSignOutAlt)`
  margin-bottom: 0.2rem;
  margin-left: 0.5rem;
`;

/** Component that allows the user to logout, and gives information on current user's organization/roles */
export const UserProfile: React.FC<React.PropsWithChildren<unknown>> = () => {
  const keycloak = useKeycloakWrapper();
  const displayName =
    keycloak.displayName ??
    (!!keycloak.firstName && !!keycloak.surname
      ? `${keycloak.firstName} ${keycloak.surname}`
      : 'default');
  const configuration = useConfiguration();
  const roles = keycloak.roles.join(', ');

  return (
    <>
      <ProfileAvatar src={profileUrl} rounded />
      <StyleDropDown className="px-0" title={displayName} id="user-dropdown" alignRight>
        {!!keycloak.roles.length && (
          <RolesBox>
            <p style={{ margin: 5 }}>
              <b>
                System Role(s):
                <br />
              </b>
              {roles}
            </p>
          </RolesBox>
        )}
        <NavDropdown.Item
          onClick={() => {
            keycloak.obj.logout({ redirectUri: `${configuration.baseUrl}/logout` });
          }}
        >
          <LogoutText>
            Log out of PIMS <LogoutButton />
          </LogoutText>
        </NavDropdown.Item>
      </StyleDropDown>
    </>
  );
};
