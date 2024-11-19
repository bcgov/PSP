import * as bcTokens from '@bcgov/design-tokens/js/variables.js';
import { useKeycloak } from '@react-keycloak/web';
import { MemoryHistory } from 'history';
import { KeycloakInstance } from 'keycloak-js';
import React from 'react';
import { ToastContainer } from 'react-toastify';
import { ThemeProvider } from 'styled-components';
import { vi } from 'vitest';

import css from '@/assets/scss/_variables.module.scss';
import ModalContainer from '@/components/common/ModalContainer';
import { ModalContextProvider } from '@/contexts/modalContext';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { TenantConsumer, TenantProvider } from '@/tenants';

import TestProviderWrapper from './TestProviderWrapper';
import TestRouterWrapper from './TestRouterWrapper';

interface TestProviderWrapperParams {
  store?: any;
  organizations?: ApiGen_Concepts_Organization[];
  claims?: string[];
  roles?: string[];
  history?: MemoryHistory;
}

/**
 * The purpose of this wrapper is to provide mock context provider functionality for common functionality within the project, such as redux, router, etc.
 * Reduces the amount of boilerplate required for a given test, and allows each test file to focus on test-specific logic.
 */
const TestCommonWrapper: React.FunctionComponent<
  React.PropsWithChildren<TestProviderWrapperParams>
> = ({ children, store, claims, roles, organizations, history }) => {
  if (!!roles || !!claims || !!organizations) {
    vi.mocked(useKeycloak).mockReturnValue({
      keycloak: {
        userInfo: {
          organizations: organizations ?? [1],
          client_roles: [...(claims ?? []), ...(roles ?? [])] ?? [],
          email: 'test@test.com',
          name: 'Chester Tester',
        },
        subject: 'test',
        authenticated: true,
      } as unknown as KeycloakInstance,
      initialized: true,
    });
  }

  return (
    <TenantProvider>
      <TenantConsumer>
        {({ tenant }) => (
          <TestProviderWrapper store={store}>
            <TestRouterWrapper history={history}>
              <ThemeProvider theme={{ tenant, css, bcTokens }}>
                <ModalContextProvider>
                  <ToastContainer
                    autoClose={5000}
                    hideProgressBar
                    newestOnTop={false}
                    closeOnClick={false}
                    rtl={false}
                    pauseOnFocusLoss={false}
                  />
                  <ModalContainer />
                  {children}
                </ModalContextProvider>
              </ThemeProvider>
            </TestRouterWrapper>
          </TestProviderWrapper>
        )}
      </TenantConsumer>
    </TenantProvider>
  );
};

export default TestCommonWrapper;
