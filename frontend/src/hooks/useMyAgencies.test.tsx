import { useKeycloak } from '@react-keycloak/web';
import { cleanup, render } from '@testing-library/react';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';
import React from 'react';
import { Provider } from 'react-redux';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { lookupCodesSlice } from 'store/slices/lookupCodes';

import { useMyAgencies } from './useMyAgencies';

jest.mock('@react-keycloak/web');
Enzyme.configure({ adapter: new Adapter() });

const mockStore = configureMockStore([thunk]);

const store = mockStore({
  [lookupCodesSlice.name]: {
    lookupCodes: [
      { type: 'Agency', code: 'BCA', parentId: 8, id: 41, name: 'BC Assessment' },
      { type: 'Agency', code: 'BAC', id: 8, name: 'B Assessment C' },
      { type: 'Agency', code: 'ABC', id: 1, name: ' Assessment BC' },
    ],
  },
});

const MyAgencies = () => {
  const agencies = useMyAgencies();

  return (
    <>
      {agencies.map(agency => (
        <h6 key={agency.value} data-testid={`agency-${agency.value}`}>
          {agency.label}
        </h6>
      ))}
    </>
  );
};

describe('UseMyAgencies', () => {
  afterEach(() => {
    cleanup();
  });

  it('Belongs to Sub Agency, should return one agencies', () => {
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: {
        userInfo: {
          agencies: ['41'],
          roles: [],
        },
        subject: 'test',
      },
    });

    const { getByTestId } = render(
      <Provider store={store}>
        <MyAgencies />
      </Provider>,
    );

    expect(getByTestId('agency-41')).toBeDefined();
  });

  it('Belongs to Parent Agency, should return parent agency and child agency', () => {
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: {
        userInfo: {
          agencies: ['8'],
          roles: [],
        },
        subject: 'test',
      },
    });

    const { getByTestId } = render(
      <Provider store={store}>
        <MyAgencies />
      </Provider>,
    );

    expect(getByTestId('agency-8')).toBeDefined();
    expect(getByTestId('agency-41')).toBeDefined();
  });
});
