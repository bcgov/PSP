import FilterBackdrop from 'components/maps/leaflet/FilterBackdrop';
import { Claims } from 'constants/claims';
import { IENotSupportedPage } from 'features/account/IENotSupportedPage';
import { LogoutPage } from 'features/account/Logout';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import AuthLayout from 'layouts/AuthLayout';
import PublicLayout from 'layouts/PublicLayout';
import { NotFoundPage } from 'pages/404/NotFoundPage';
import Test from 'pages/Test.ignore';
import React, { lazy, Suspense, useLayoutEffect } from 'react';
import Col from 'react-bootstrap/Col';
import { Redirect, Switch, useLocation } from 'react-router-dom';
import AppRoute from 'utils/AppRoute';

import Login from './features/account/Login';
import AccessDenied from './pages/401/AccessDenied';

const MapView = lazy(() => import('./features/properties/map/MapView'));
const AccessRequestPage = lazy(() => import('./features/admin/access-request/AccessRequestPage'));
const EditUserPage = lazy(() => import('./features/admin/edit-user/EditUserPage'));
const ManageAccessRequests = lazy(() => import('features/admin/access/ManageAccessRequests'));
const ManageUsers = lazy(() => import('features/admin/users/ManageUsers'));
const PropertyListView = lazy(() => import('features/properties/list/PropertyListView'));

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
            <FilterBackdrop show={true}></FilterBackdrop>
          </AuthLayout>
        ) : (
          <PublicLayout>
            <Col>
              <FilterBackdrop show={true}></FilterBackdrop>
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
          path="/admin/user/:key?"
          component={EditUserPage}
          layout={AuthLayout}
          claim={Claims.ADMIN_USERS}
          title={getTitle('Edit User')}
        />
        <AppRoute title="*" path="*" component={() => <Redirect to="/page-not-found" />} />
      </Switch>
    </Suspense>
  );
};

export default AppRouter;
