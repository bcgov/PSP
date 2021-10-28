import { useKeycloak } from '@react-keycloak/web';
import { MemoryHistory } from 'history';
import { IOrganization } from 'interfaces';
import React from 'react';
import { ToastContainer } from 'react-toastify';
import { ThemeProvider } from 'styled-components';
import { TenantConsumer, TenantProvider } from 'tenants';

import TestProviderWrapper from './TestProviderWrapper';
import TestRouterWrapper from './TestRouterWrapper';

jest.mock('@react-keycloak/web');

interface TestProviderWrapperParams {
  store?: any;
  organizations?: IOrganization[];
  claims?: string[];
  roles?: string[];
  history?: MemoryHistory;
}

/**
 * The purpose of this wrapper is to provide mock context provider functionality for common functionality within the project, such as redux, router, etc.
 * Reduces the amount of boilerplate required for a given test, and allows each test file to focus on test-specific logic.
 */
const TestCommonWrapper: React.FunctionComponent<TestProviderWrapperParams> = ({
  children,
  store,
  claims,
  roles,
  organizations,
  history,
}) => {
  if (!!roles || !!claims || !!organizations) {
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: {
        userInfo: {
          organizations: organizations ?? [1],
          roles: claims ?? [],
          groups: roles ?? [],
          email: 'test@test.com',
          name: 'Chester Tester',
        },
        subject: 'test',
        authenticated: true,
      },
    });
  }

  return (
    <TenantProvider>
      <TenantConsumer>
        {({ tenant }) => (
          <TestProviderWrapper store={store}>
            <TestRouterWrapper history={history}>
              <ThemeProvider theme={{ tenant, css: {} }}>
                <ToastContainer
                  autoClose={5000}
                  hideProgressBar
                  newestOnTop={false}
                  closeOnClick={false}
                  rtl={false}
                  pauseOnFocusLoss={false}
                />
                {children}
              </ThemeProvider>
            </TestRouterWrapper>
          </TestProviderWrapper>
        )}
      </TenantConsumer>
    </TenantProvider>
  );
};

export default TestCommonWrapper;
