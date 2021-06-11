import './AppNavBar.scss';

import { Claims } from 'constants/claims';
import { HelpContainer } from 'features/help/containers/HelpContainer';
import { SidebarContextType } from 'features/mapSideBar/hooks/useQueryParamSideBar';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import queryString from 'query-string';
import React from 'react';
import { Nav, Navbar, NavDropdown } from 'react-bootstrap';
import { FaHome } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';
import { FeatureHidden, FeatureVisible } from 'tenants';

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
          <FeatureHidden tenant="MOTI">
            <SubmitProperty />
          </FeatureHidden>
          <FeatureVisible tenant="MOTI">
            <AddProperty />
          </FeatureVisible>
          <ViewInventory />
          <FeatureHidden tenant="MOTI">
            <DisposeProjectsDropdown />
          </FeatureHidden>
          <ReportsDropdown />
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
 * Submit Property navigation dropdown item.
 */
function SubmitProperty() {
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
      Submit Property
    </Nav.Link>
  ) : null;
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

/**
 * Disposal Projects navigation dropdown menu.
 */
function DisposeProjectsDropdown() {
  const history = useHistory();
  const keycloak = useKeycloakWrapper();
  return keycloak.hasClaim(Claims.PROJECT_VIEW) ||
    keycloak.hasClaim(Claims.DISPOSE_APPROVE) ||
    keycloak.hasClaim(Claims.ADMIN_PROJECTS) ? (
    <NavDropdown
      className={
        history.location.pathname.includes('dispose') ||
        history.location.pathname.includes('projects')
          ? 'active'
          : 'idle'
      }
      title="Disposal Projects"
      id="dispose"
    >
      {(keycloak.hasClaim(Claims.PROJECT_ADD) || keycloak.hasClaim(Claims.ADMIN_PROJECTS)) && (
        <NavDropdown.Item onClick={() => history.push('/dispose')}>
          Create Disposal Project
        </NavDropdown.Item>
      )}
      {(keycloak.hasClaim(Claims.PROJECT_VIEW) || keycloak.hasClaim(Claims.ADMIN_PROJECTS)) && (
        <NavDropdown.Item onClick={() => history.push('/projects/list')}>
          View Projects
        </NavDropdown.Item>
      )}
      {keycloak.hasClaim(Claims.DISPOSE_APPROVE) && (
        <NavDropdown.Item onClick={() => history.push('/projects/approval/requests')}>
          Approval Requests
        </NavDropdown.Item>
      )}
    </NavDropdown>
  ) : null;
}

/**
 * Reports navigation dropdown menu.
 */
function ReportsDropdown() {
  const history = useHistory();
  const keycloak = useKeycloakWrapper();
  return keycloak.hasClaim(Claims.REPORTS_VIEW) ? (
    <NavDropdown
      className={history.location.pathname.includes('reports') ? 'active' : 'idle'}
      title="Reports"
      id="reports"
    >
      {keycloak.hasClaim(Claims.REPORTS_SPL) && (
        <FeatureHidden tenant="MOTI">
          <NavDropdown.Item onClick={() => history.push('/reports/spl')}>
            SPL Report
          </NavDropdown.Item>
        </FeatureHidden>
      )}
    </NavDropdown>
  ) : null;
}

export default AppNavBar;
