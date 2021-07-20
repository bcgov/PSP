import './AppNavBar.scss';

import { Claims } from 'constants/claims';
import { HelpContainer } from 'features/help/containers/HelpContainer';
import { SidebarContextType } from 'features/mapSideBar/hooks/useQueryParamSideBar';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import queryString from 'query-string';
import React from 'react';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';
import { FaHome } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';

/**
 * Nav bar with role-based functionality.
 */
function AppNavBar() {
  return (
    <Navbar variant="dark" className="map-nav" expand="lg">
      <Navbar.Toggle aria-controls="collapse" className="navbar-dark mr-auto" />
      <Navbar.Collapse className="links mr-auto">
        <Nav>
          <HomeButton />
          <AdminDropdown />
          <AddProperty />
          <ViewInventory />
        </Nav>
      </Navbar.Collapse>
      <HelpContainer />
    </Navbar>
  );
}

function HomeButton() {
  const history = useHistory();
  const keycloak = useKeycloakWrapper();
  // We don't want to show the Home button to new users (who get redirected to Access Request page)
  if (!keycloak?.roles?.length) return null;

  return (
    <Nav.Item aria-label="Home" className="home-button" onClick={() => history.push('/mapview')}>
      <FaHome size={20} />
    </Nav.Item>
  );
}

/**
 * Add a property dropdown item.
 */
const AddProperty = () => {
  const keycloak = useKeycloakWrapper();
  const history = useHistory();
  return keycloak.hasClaim(Claims.PROPERTY_ADD) ? (
    <Nav.Link
      className={
        history.location.pathname.includes('mapview') &&
        queryString.parse(history.location.search).sidebar === 'true'
          ? 'active'
          : 'idle'
      }
      onClick={() =>
        history.push({
          pathname: '/mapview',
          search: queryString.stringify({
            ...queryString.parse(history.location.search),
            sidebar: true,
            disabled: false,
            loadDraft: false,
            parcelId: undefined,
            buildingId: undefined,
            new: true,
            sidebarContext: SidebarContextType.ADD_PROPERTY_TYPE_SELECTOR,
            sidebarSize: 'narrow',
          }),
        })
      }
    >
      Add a Property
    </Nav.Link>
  ) : null;
};

/**
 * View Inventory navigation item.
 */
function ViewInventory() {
  const keycloak = useKeycloakWrapper();
  const history = useHistory();
  return keycloak.hasClaim(Claims.PROPERTY_VIEW) ? (
    <Nav.Link
      className={history.location.pathname.includes('properties/list') ? 'active' : 'idle'}
      onClick={() => history.push('/properties/list')}
    >
      View Property Inventory
    </Nav.Link>
  ) : null;
}

/**
 * Administration dropdown menu.
 */
function AdminDropdown() {
  const keycloak = useKeycloakWrapper();
  const history = useHistory();
  return keycloak.isAdmin ? (
    <NavDropdown
      className={history.location.pathname.includes('admin') ? 'active' : 'idle'}
      title="Administration"
      id="administration"
    >
      <NavDropdown.Item onClick={() => history.push('/admin/users')}>Users</NavDropdown.Item>
      <NavDropdown.Item onClick={() => history.push('/admin/access/requests')}>
        Access Requests
      </NavDropdown.Item>
      <NavDropdown.Item onClick={() => history.push('/admin/agencies')}>Agencies</NavDropdown.Item>
    </NavDropdown>
  ) : null;
}

export default AppNavBar;
