// import { waitFor } from '@testing-library/react';
// import AppRouter from '@/AppRouter';
// import axios from 'axios';
// import MockAdapter from 'axios-mock-adapter';
// import { Footer, Header } from '@/components/layout';
// import Map from '@/components/maps/leaflet/Map';
// import { Claims } from '@/constants/index';
// import { AuthStateContextProvider } from '@/contexts/authStateContext';
// import { IENotSupportedPage } from '@/features/account/IENotSupportedPage';
// import Login from '@/features/account/Login';
// import ManageAccessRequestsPage from '@/features/admin/access/ManageAccessRequestsPage';
// import AccessRequestPage from '@/features/admin/access-request/AccessRequestPage';
// import EditUserPage from '@/features/admin/edit-user/EditUserPage';
// import ManageUsers from '@/features/admin/users/ManageUsersPage';
// import { PropertyListView } from '@/features/properties/list';
// import { Formik } from 'formik';
// import { createMemoryHistory } from 'history';
// import { enableFetchMocks } from 'jest-fetch-mock';
// import { noop } from 'lodash';
// import AccessDenied from '@/pages/401/AccessDenied';
// import { NotFoundPage } from '@/pages/404/NotFoundPage';
// import Test from '@/pages/Test.ignore';
// import { act } from 'react-dom/test-utils';
// import { flushPromises, mockKeycloak } from '@/utils/test-utils';
// import TestCommonWrapper from '@/utils/TestCommonWrapper';

// const mockAxios = new MockAdapter(axios);

// enableFetchMocks();
// jest.mock('@react-keycloak/web');
// const history = createMemoryHistory();

describe('PSP routing - Enzyme Tests - NEEDS REFACTORING', () => {
  it('should be implemented', async () => {});

  // beforeEach(() => {
  //   fetchMock.mockResponse(JSON.stringify({ status: 200, body: {} }));
  // });
  // const getRouter = (url: string) => {
  //   history.push(url);
  //   return (
  //     <TestCommonWrapper
  //       history={history}
  //       store={{
  //         network: {},
  //         keycloakReady: true,
  //         loadingBar: {},
  //         lookupCode: { lookupCodes: [] },
  //         tenants: { config: { settings: {} } },
  //         users: { pagedUsers: { items: [] }, userDetail: {} },
  //         organizations: { pagedOrganizations: { items: [] }, organizationDetail: {} },
  //         accessRequests: { pagedAccessRequests: { items: [] } },
  //       }}
  //     >
  //       <AuthStateContextProvider>
  //         <Formik initialValues={{}} onSubmit={noop}>
  //           <AppRouter />
  //         </Formik>
  //       </AuthStateContextProvider>
  //     </TestCommonWrapper>
  //   );
  // };
  // describe('unauth routes', () => {
  //   let wrapper: any;
  //   beforeEach(() => {
  //     mockKeycloak({ authenticated: false });
  //     mockAxios.onAny().reply(200, {});
  //   });
  //   afterEach(() => {
  //     wrapper.unmount();
  //   });
  //   it('valid path should redirect unauthenticated user to the login page', async () => {
  //     wrapper = mount(getRouter('/'));
  //     await act(async () => {
  //       await flushPromises();
  //     });
  //     expect(wrapper.find(Login)).toHaveLength(1);
  //   });
  //   it('unauthenticated users should see the Header', async () => {
  //     wrapper = mount(getRouter('/'));
  //     await act(async () => {
  //       await flushPromises();
  //     });
  //     expect(wrapper.find(Header)).toHaveLength(1);
  //   });
  //   it('unauthenticated users should see the footer', async () => {
  //     wrapper = mount(getRouter('/'));
  //     await act(async () => {
  //       await flushPromises();
  //     });
  //     expect(wrapper.find(Footer)).toHaveLength(1);
  //   });
  //   it('displays the ie warning page at the expected route', async () => {
  //     wrapper = mount(getRouter('/ienotsupported'));
  //     await act(async () => {
  //       await flushPromises();
  //     });
  //     expect(wrapper.find(IENotSupportedPage)).toHaveLength(1);
  //   });
  //   it('displays the forbidden page at the expected route', async () => {
  //     wrapper = mount(getRouter('/forbidden'));
  //     await act(async () => {
  //       await flushPromises();
  //     });
  //     expect(wrapper.find(AccessDenied)).toHaveLength(1);
  //   });
  //   it('displays not found page at the expected route', async () => {
  //     wrapper = mount(getRouter('/page-not-found'));
  //     await act(async () => {
  //       await flushPromises();
  //     });
  //     expect(wrapper.find(NotFoundPage)).toHaveLength(1);
  //   });
  //   it('displays not found page at an unknown route', async () => {
  //     wrapper = mount(getRouter('/fake'));
  //     await act(async () => {
  //       await flushPromises();
  //     });
  //     expect(wrapper.find(NotFoundPage)).toHaveLength(1);
  //   });
  //   it('displays the test page at the expected route', async () => {
  //     wrapper = mount(getRouter('/test'));
  //     await act(async () => {
  //       await flushPromises();
  //     });
  //     expect(wrapper.find(Test)).toHaveLength(1);
  //   });
  // });
  // describe('auth routes', () => {
  //   beforeEach(() => {
  //     mockKeycloak({
  //       claims: [Claims.PROPERTY_VIEW, Claims.ADMIN_USERS],
  //       roles: [Claims.PROPERTY_VIEW],
  //       authenticated: true,
  //     });
  //     mockAxios.onAny().reply(200, {});
  //     delete (window as any).ResizeObserver;
  //     window.ResizeObserver = jest.fn().mockImplementation(() => ({
  //       observe: jest.fn(),
  //       unobserve: jest.fn(),
  //       disconnect: jest.fn(),
  //     }));
  //   });
  //   it('displays the mapview on the home page', async () => {
  //     const wrapper = mount(getRouter('/mapView'));
  //     await waitFor(async () => {
  //       wrapper.update();
  //       expect(wrapper.find(Map)).toHaveLength(1);
  //     });
  //   });
  //   it('displays the admin users page at the expected route', async () => {
  //     const wrapper = mount(getRouter('/admin/users'));
  //     await waitFor(async () => {
  //       wrapper.update();
  //       expect(wrapper.find(ManageUsers)).toHaveLength(1);
  //     });
  //   });
  //   it('displays the admin access requests page at the expected route', async () => {
  //     const wrapper = mount(getRouter('/admin/access/requests'));
  //     await waitFor(async () => {
  //       wrapper.update();
  //       expect(wrapper.find(ManageAccessRequestsPage)).toHaveLength(1);
  //     });
  //   });
  //   it('displays the access request page at the expected route', async () => {
  //     const wrapper = mount(getRouter('/access/request'));
  //     await waitFor(async () => {
  //       wrapper.update();
  //       expect(wrapper.find(AccessRequestPage)).toHaveLength(1);
  //     });
  //   });
  //   it('displays the property list view at the expected route', async () => {
  //     const wrapper = mount(getRouter('/properties/list'));
  //     await waitFor(async () => {
  //       wrapper.update();
  //       expect(wrapper.find(PropertyListView)).toHaveLength(1);
  //     });
  //   });
  //   it('displays the edit user page at the expected route', async () => {
  //     const wrapper = mount(getRouter('/admin/user/1'));
  //     await waitFor(async () => {
  //       wrapper.update();
  //       expect(wrapper.find(EditUserPage)).toHaveLength(1);
  //     });
  //   });
  // });
});

// TODO: Remove this line when unit tests above are fixed
export {};
