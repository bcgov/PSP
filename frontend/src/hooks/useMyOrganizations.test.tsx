import { useKeycloak } from '@react-keycloak/web';
import { cleanup, render } from '@testing-library/react';
import React from 'react';
import { Provider } from 'react-redux';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { lookupCodesSlice } from 'store/slices/lookupCodes';

import { useMyOrganizations } from './useMyOrganizations';

jest.mock('@react-keycloak/web');

const mockStore = configureMockStore([thunk]);

const store = mockStore({
  [lookupCodesSlice.name]: {
    lookupCodes: [
      { type: 'Organization', code: 'BCA', parentId: 8, id: 41, name: 'BC Assessment' },
      { type: 'Organization', code: 'BAC', id: 8, name: 'B Assessment C' },
      { type: 'Organization', code: 'ABC', id: 1, name: ' Assessment BC' },
    ],
  },
});

const MyOrganizations = () => {
  const organizations = useMyOrganizations();

  return (
    <>
      {organizations.map(organization => (
        <h6 key={organization.value} data-testid={`organization-${organization.value}`}>
          {organization.label}
        </h6>
      ))}
    </>
  );
};

describe('UseMyOrganizations', () => {
  afterEach(() => {
    cleanup();
  });

  it('Belongs to Sub Organization, should return one organizations', () => {
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: {
        userInfo: {
          organizations: ['41'],
          roles: [],
        },
        subject: 'test',
      },
    });

    const { getByTestId } = render(
      <Provider store={store}>
        <MyOrganizations />
      </Provider>,
    );

    expect(getByTestId('organization-41')).toBeDefined();
  });

  it('Belongs to Parent Organization, should return porganizationorganization and organizationorganization', () => {
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: {
        userInfo: {
          organizations: ['8'],
          roles: [],
        },
        subject: 'test',
      },
    });

    const { getByTestId } = render(
      <Provider store={store}>
        <MyOrganizations />
      </Provider>,
    );

    expect(getByTestId('organization-8')).toBeDefined();
    expect(getByTestId('organization-41')).toBeDefined();
  });
});
