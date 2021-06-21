import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { MemoryHistory } from 'history';
import { IRole } from 'interfaces';
import { IAgency } from 'interfaces/agency';
import React from 'react';
import { ToastContainer } from 'react-toastify';
import { TenantProvider } from 'tenants';

import TestProviderWrapper from './TestProviderWrapper';
import TestRouterWrapper from './TestRouterWrapper';

jest.mock('@react-keycloak/web');

interface TestProviderWrapperParams {
  children: React.ReactNode;
  store?: any;
  agencies?: IAgency[];
  roles?: IRole[];
  history?: MemoryHistory;
}

/**
 * The purpose of this wrapper is to provide mock context provider functionality for common functionality within the project, such as redux, router, etc.
 * Reduces the amount of boilerplate required for a given test, and allows each test file to focus on test-specific logic.
 */
const TestCommonWrapper: React.FunctionComponent<TestProviderWrapperParams> = ({
  children,
  store,
  roles,
  agencies,
  history,
}) => {
  (useKeycloak as jest.Mock).mockReturnValue({
    keycloak: {
      userInfo: {
        agencies: agencies ?? [1],
        roles: roles ?? [],
      },
      subject: 'test',
    },
  });
  const mockAxios = new MockAdapter(axios);
  mockAxios.onAny().reply(200);
  return (
    <TenantProvider>
      <TestProviderWrapper store={store}>
        <TestRouterWrapper history={history}>
          <ToastContainer
            autoClose={5000}
            hideProgressBar
            newestOnTop={false}
            closeOnClick={false}
            rtl={false}
            pauseOnFocusLoss={false}
          />
          {children}
        </TestRouterWrapper>
      </TestProviderWrapper>
    </TenantProvider>
  );
};

export default TestCommonWrapper;
