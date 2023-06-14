import { cleanup } from '@testing-library/react';
import { createMemoryHistory } from 'history';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import renderer from 'react-test-renderer';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { ADD_ACTIVATE_USER } from '@/constants/actionTypes';
import { networkSlice } from '@/store/slices/network/networkSlice';
import { TenantProvider } from '@/tenants';

import { IENotSupportedPage } from './IENotSupportedPage';

jest.mock('axios');
jest.mock('@react-keycloak/web');
const mockStore = configureMockStore([thunk]);

const store = mockStore({
  [networkSlice.name]: {
    [ADD_ACTIVATE_USER]: {},
  },
});

describe('login error page', () => {
  afterEach(() => {
    cleanup();
  });
  it('login error page renders correctly', () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    const history = createMemoryHistory();
    const tree = renderer
      .create(
        <TenantProvider>
          <Provider store={store}>
            <Router history={history}>
              <IENotSupportedPage></IENotSupportedPage>
            </Router>
          </Provider>
        </TenantProvider>,
      )
      .toJSON();
    expect(tree).toMatchSnapshot();
  });
});
