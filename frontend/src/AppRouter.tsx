import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Claims } from 'constants/claims';
import { IENotSupportedPage } from 'features/account/IENotSupportedPage';
import { LogoutPage } from 'features/account/Logout';
import { ContactListPage } from 'features/contacts';
import CreateContactContainer from 'features/contacts/contact/create/CreateContactContainer';
import ContactViewContainer from 'features/contacts/contact/detail/Container';
import UpdateContactContainer from 'features/contacts/contact/edit/UpdateContactContainer';
import { AddLeaseContainer } from 'features/leases';
import { ResearchListView } from 'features/research/list/ResearchListView';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import AuthLayout from 'layouts/AuthLayout';
import PublicLayout from 'layouts/PublicLayout';
import { NotFoundPage } from 'pages/404/NotFoundPage';
import Test from 'pages/Test.ignore';
import { TestFileManagement } from 'pages/TestFileManagement';
import React, { lazy, Suspense, useLayoutEffect } from 'react';
import Col from 'react-bootstrap/Col';
import { Redirect, Switch, useLocation } from 'react-router-dom';
import AppRoute from 'utils/AppRoute';
import componentLoader from 'utils/utils';

import Login from './features/account/Login';
import AccessDenied from './pages/401/AccessDenied';

const MapView = lazy(() => componentLoader(import('./features/properties/map/MapView'), 2));
const AccessRequestPage = lazy(() =>
  componentLoader(import('./features/admin/access-request/AccessRequestPage'), 2),
);
const EditUserPage = lazy(() =>
  componentLoader(import('./features/admin/edit-user/EditUserPage'), 2),
);
const ManageAccessRequests = lazy(() =>
  componentLoader(import('features/admin/access/ManageAccessRequests'), 2),
);
const ManageUsers = lazy(() => componentLoader(import('features/admin/users/ManageUsers'), 2));
const PropertyListView = lazy(() =>
  componentLoader(import('features/properties/list/PropertyListView'), 2),
);
const LeaseAndLicenseListView = lazy(() =>
  componentLoader(import('features/leases/list/LeaseListView'), 2),
);
const LeaseContainerWrapper = lazy(() =>
  componentLoader(import('features/leases/detail/LeaseContainerWrapper'), 2),
);

const AppRouter: React.FC = () => {
  const location = useLocation();
  useLayoutEffect(() => {
    window.scrollTo(0, 0);
  }, [location.pathname]);
  const getTitle = (page: string) => {
    return `PIMS${' - ' + page}`;
  };
  const keycloak = useKeycloakWrapper();
  return (
    <Suspense
      fallback={
        keycloak.obj.authenticated ? (
          <AuthLayout>
            <LoadingBackdrop show={true}></LoadingBackdrop>
          </AuthLayout>
        ) : (
          <PublicLayout>
            <Col>
              <LoadingBackdrop show={true}></LoadingBackdrop>
            </Col>
          </PublicLayout>
        )
      }
    >
      <Switch>
        <Redirect exact from="/" to="/login" />
        <AppRoute
          path="/login"
          title={getTitle('Login')}
          component={Login}
          layout={PublicLayout}
        ></AppRoute>
        <AppRoute
          path="/ienotsupported"
          title={getTitle('IE Not Supported')}
          component={IENotSupportedPage}
          layout={PublicLayout}
        ></AppRoute>
        <AppRoute path="/logout" title={getTitle('Logout')} component={LogoutPage}></AppRoute>
        <AppRoute
          path="/forbidden"
          title={getTitle('Forbidden')}
          component={AccessDenied}
          layout={PublicLayout}
        ></AppRoute>
        <AppRoute
          path="/page-not-found"
          title={getTitle('Page Not Found')}
          component={NotFoundPage}
          layout={PublicLayout}
        ></AppRoute>
        <AppRoute
          path="/test"
          title={getTitle('Test')}
          component={Test}
          layout={PublicLayout}
        ></AppRoute>
        <AppRoute
          protected
          path="/admin/users"
          component={ManageUsers}
          layout={AuthLayout}
          claim={Claims.ADMIN_USERS}
          title={getTitle('Users Management')}
        ></AppRoute>
        <AppRoute
          protected
          path="/admin/access/requests"
          component={ManageAccessRequests}
          layout={AuthLayout}
          claim={Claims.ADMIN_USERS}
          title={getTitle('Access Requests')}
        ></AppRoute>
        <AppRoute
          protected
          path="/access/request"
          component={AccessRequestPage}
          layout={AuthLayout}
          title={getTitle('Request Access')}
        ></AppRoute>
        <AppRoute
          protected
          path="/mapView/:id?"
          component={MapView}
          layout={AuthLayout}
          claim={Claims.PROPERTY_VIEW}
          title={getTitle('Map View')}
        />
        <AppRoute
          protected
          path="/properties/list"
          component={PropertyListView}
          layout={AuthLayout}
          claim={Claims.PROPERTY_VIEW}
          title={getTitle('View Inventory')}
        />
        <AppRoute
          protected
          exact
          path="/lease/list"
          component={LeaseAndLicenseListView}
          layout={AuthLayout}
          claim={Claims.PROPERTY_VIEW}
          title={getTitle('View Lease & Licenses')}
        />
        <AppRoute
          protected
          path="/lease/new"
          exact
          component={AddLeaseContainer}
          layout={AuthLayout}
          claim={Claims.LEASE_ADD}
          title={getTitle('Create/Edit Lease & Licenses')}
        />
        <AppRoute
          protected
          path="/lease/:leaseId"
          component={LeaseContainerWrapper}
          layout={AuthLayout}
          claim={Claims.PROPERTY_VIEW}
          title={getTitle('Create/Edit Lease & Licenses')}
        />
        <AppRoute
          protected
          path="/contact/list"
          component={ContactListPage}
          layout={AuthLayout}
          claim={Claims.CONTACT_VIEW}
          title={getTitle('View Contacts')}
        />
        <AppRoute
          protected
          path="/contact/new"
          component={CreateContactContainer}
          layout={AuthLayout}
          claim={[Claims.CONTACT_ADD]}
          title={getTitle('Create Contact')}
        />
        <AppRoute
          protected
          path="/contact/:id?/edit"
          component={UpdateContactContainer}
          layout={AuthLayout}
          claim={[Claims.CONTACT_EDIT]}
          title={getTitle('Edit Contact')}
        />
        <AppRoute
          protected
          path="/contact/:id?"
          component={ContactViewContainer}
          layout={AuthLayout}
          claim={[Claims.CONTACT_VIEW]}
          title={getTitle('View Contact')}
        />
        <AppRoute
          protected
          exact
          path="/research/list"
          component={ResearchListView}
          layout={AuthLayout}
          claim={Claims.RESEARCH_VIEW}
          title={getTitle('View Research Files')}
        />
        <AppRoute
          protected
          path="/admin/user/:key?"
          component={EditUserPage}
          layout={AuthLayout}
          claim={Claims.ADMIN_USERS}
          title={getTitle('Edit User')}
        />
        <AppRoute
          path="/testFileManagement"
          title={getTitle('Test')}
          component={TestFileManagement}
          layout={AuthLayout}
        />
        <AppRoute title="*" path="*" component={() => <Redirect to="/page-not-found" />} />
      </Switch>
    </Suspense>
  );
};

export default AppRouter;
