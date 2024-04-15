import { FC, lazy, PropsWithChildren, Suspense, useLayoutEffect } from 'react';
import Col from 'react-bootstrap/Col';
import { Redirect, Switch, useLocation } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { MapStateMachineProvider } from '@/components/common/mapFSM/MapStateMachineContext';
import { Claims } from '@/constants/claims';
import { Roles } from '@/constants/roles';
import { IENotSupportedPage } from '@/features/account/IENotSupportedPage';
import { LogoutPage } from '@/features/account/Logout';
import { AdminAccessRequestPage } from '@/features/admin/access-request/AdminAccessRequestPage';
import { AddFinancialCodeContainer } from '@/features/admin/financial-codes/add/AddFinancialCodeContainer';
import { AddFinancialCodeForm } from '@/features/admin/financial-codes/add/AddFinancialCodeForm';
import { UpdateFinancialCodeContainer } from '@/features/admin/financial-codes/update/UpdateFinancialCodeContainer';
import { UpdateFinancialCodeForm } from '@/features/admin/financial-codes/update/UpdateFinancialCodeForm';
import { ContactListPage } from '@/features/contacts';
import CreateContactContainer from '@/features/contacts/contact/create/CreateContactContainer';
import ContactViewContainer from '@/features/contacts/contact/detail/Container';
import UpdateContactContainer from '@/features/contacts/contact/edit/UpdateContactContainer';
import ProjectListView from '@/features/projects/list/ProjectListView';
import { ResearchListView } from '@/features/research/list/ResearchListView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import AuthLayout from '@/layouts/AuthLayout';
import PublicLayout from '@/layouts/PublicLayout';
import { NotFoundPage } from '@/pages/404/NotFoundPage';
import AppRoute from '@/utils/AppRoute';
import componentLoader from '@/utils/utils';

import Login from './features/account/Login';
import AccessDenied from './pages/401/AccessDenied';

const MapView = lazy(() => componentLoader(import('@/features/properties/map/MapContainer'), 2));
const AccessRequestPage = lazy(() =>
  componentLoader(import('@/features/admin/access-request/AccessRequestPage'), 2),
);
const EditUserPage = lazy(() =>
  componentLoader(import('@/features/admin/edit-user/EditUserPage'), 2),
);
const ManageAccessRequests = lazy(() =>
  componentLoader(import('@/features/admin/access/ManageAccessRequestsPage'), 2),
);
const ManageUsers = lazy(() =>
  componentLoader(import('@/features/admin/users/ManageUsersPage'), 2),
);

const ManageDocumentTemplate = lazy(() =>
  componentLoader(import('@/features/admin/document-template/DocumentTemplateManagementPage'), 2),
);

const PropertyListView = lazy(() =>
  componentLoader(import('@/features/properties/list/PropertyListView'), 2),
);
const LeaseAndLicenseListView = lazy(() =>
  componentLoader(import('@/features/leases/list/LeaseListView'), 2),
);
const AcquisitionListView = lazy(() =>
  componentLoader(import('@/features/acquisition/list/AcquisitionListView'), 2),
);
const FinancialCodesListView = lazy(() =>
  componentLoader(import('@/features/admin/financial-codes/list/FinancialCodeListView'), 2),
);

const DispositionListView = lazy(() =>
  componentLoader(import('@/features/disposition/list/DispositionListView'), 2),
);

const AppRouter: FC<PropsWithChildren<unknown>> = () => {
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
      <MapStateMachineProvider>
        <Switch>
          <Redirect exact from="/" to="/login" />
          <AppRoute
            path="/login"
            title={getTitle('Login')}
            customComponent={Login}
            layout={PublicLayout}
          ></AppRoute>
          <AppRoute
            path="/ienotsupported"
            title={getTitle('IE Not Supported')}
            customComponent={IENotSupportedPage}
            layout={PublicLayout}
          ></AppRoute>
          <AppRoute
            path="/logout"
            title={getTitle('Logout')}
            customComponent={LogoutPage}
          ></AppRoute>
          <AppRoute
            path="/forbidden"
            title={getTitle('Forbidden')}
            customComponent={AccessDenied}
            layout={PublicLayout}
          ></AppRoute>
          <AppRoute
            path="/page-not-found"
            title={getTitle('Page Not Found')}
            customComponent={NotFoundPage}
            layout={PublicLayout}
          ></AppRoute>
          <AppRoute
            protected
            path="/admin/users"
            customComponent={ManageUsers}
            layout={AuthLayout}
            claim={Claims.ADMIN_USERS}
            title={getTitle('Users Management')}
          ></AppRoute>
          <AppRoute
            protected
            path="/admin/access/requests"
            customComponent={ManageAccessRequests}
            layout={AuthLayout}
            claim={Claims.ADMIN_USERS}
            title={getTitle('Access Requests')}
          ></AppRoute>
          <AppRoute
            protected
            path="/admin/document_generation"
            customComponent={ManageDocumentTemplate}
            layout={AuthLayout}
            claim={Claims.DOCUMENT_ADMIN}
            title={getTitle('Document Template')}
          ></AppRoute>
          <AppRoute
            protected
            exact
            path="/admin/financial-code/list"
            customComponent={FinancialCodesListView}
            layout={AuthLayout}
            role={Roles.SYSTEM_ADMINISTRATOR}
            title={getTitle('Financial Codes')}
          />
          <AppRoute
            protected
            exact
            path="/admin/financial-code/new"
            customRender={() => (
              <AuthLayout>
                <AddFinancialCodeContainer View={AddFinancialCodeForm} />
              </AuthLayout>
            )}
            role={Roles.SYSTEM_ADMINISTRATOR}
            title={getTitle('Create Financial Code')}
          />
          <AppRoute
            protected
            path="/admin/financial-code/:type/:id"
            customRender={({ match }) => (
              <AuthLayout>
                <UpdateFinancialCodeContainer
                  type={match.params.type}
                  id={Number(match.params.id)}
                  View={UpdateFinancialCodeForm}
                />
              </AuthLayout>
            )}
            role={Roles.SYSTEM_ADMINISTRATOR}
            title={getTitle('Update Financial Code')}
          />
          <AppRoute
            protected
            path="/access/request"
            customComponent={AccessRequestPage}
            layout={AuthLayout}
            title={getTitle('Request Access')}
          ></AppRoute>
          <AppRoute
            protected
            path="/admin/access/request/:id"
            customComponent={AdminAccessRequestPage}
            layout={AuthLayout}
            claim={Claims.ADMIN_USERS}
            title={getTitle('Request Access')}
          ></AppRoute>
          <AppRoute
            protected
            path="/mapView/:id?"
            customComponent={MapView}
            layout={AuthLayout}
            claim={Claims.PROPERTY_VIEW}
            title={getTitle('Map View')}
          />
          <AppRoute
            protected
            path="/properties/list"
            customComponent={PropertyListView}
            layout={AuthLayout}
            claim={Claims.PROPERTY_VIEW}
            title={getTitle('View Inventory')}
          />
          <AppRoute
            protected
            exact
            path="/lease/list"
            customComponent={LeaseAndLicenseListView}
            layout={AuthLayout}
            claim={Claims.LEASE_VIEW}
            title={getTitle('View Lease & Licenses')}
          />
          <AppRoute
            protected
            path="/contact/list"
            customComponent={ContactListPage}
            layout={AuthLayout}
            claim={Claims.CONTACT_VIEW}
            title={getTitle('View Contacts')}
          />
          <AppRoute
            protected
            path="/contact/new"
            customComponent={CreateContactContainer}
            layout={AuthLayout}
            claim={[Claims.CONTACT_ADD]}
            title={getTitle('Create Contact')}
          />
          <AppRoute
            protected
            path="/contact/:id?/edit"
            customComponent={UpdateContactContainer}
            layout={AuthLayout}
            claim={[Claims.CONTACT_EDIT]}
            title={getTitle('Edit Contact')}
          />
          <AppRoute
            protected
            path="/contact/:id?"
            customComponent={ContactViewContainer}
            layout={AuthLayout}
            claim={[Claims.CONTACT_VIEW]}
            title={getTitle('View Contact')}
          />
          <AppRoute
            protected
            exact
            path="/research/list"
            customComponent={ResearchListView}
            layout={AuthLayout}
            claim={Claims.RESEARCH_VIEW}
            title={getTitle('View Research Files')}
          />
          <AppRoute
            protected
            exact
            path="/acquisition/list"
            customComponent={AcquisitionListView}
            layout={AuthLayout}
            claim={Claims.ACQUISITION_VIEW}
            title={getTitle('View Acquisition Files')}
          />
          <AppRoute
            protected
            path="/project/list"
            customComponent={ProjectListView}
            layout={AuthLayout}
            claim={Claims.PROJECT_VIEW}
            title={getTitle('View Projects')}
          />
          <AppRoute
            protected
            exact
            path="/disposition/list"
            customComponent={DispositionListView}
            layout={AuthLayout}
            claim={Claims.DISPOSITION_VIEW}
            title={getTitle('View Disposition Files')}
          />
          <AppRoute
            protected
            path="/admin/user/:key?"
            customComponent={EditUserPage}
            layout={AuthLayout}
            claim={Claims.ADMIN_USERS}
            title={getTitle('Edit User')}
          />
          <AppRoute title="*" path="*" customComponent={() => <Redirect to="/page-not-found" />} />
        </Switch>
      </MapStateMachineProvider>
    </Suspense>
  );
};

export default AppRouter;
