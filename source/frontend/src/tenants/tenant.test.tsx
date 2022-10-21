import { render } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { TenantConsumer, TenantProvider } from '.';

const mockAxios = new MockAdapter(axios);

const renderTenant = () => {
  return (
    <TenantProvider>
      <TenantConsumer>{({ tenant }) => <div>{JSON.stringify(tenant)}</div>}</TenantConsumer>
    </TenantProvider>
  );
};

describe('Tenant configuration', () => {
  const OLD_ENV = process.env;

  beforeEach(() => {
    jest.resetModules();
    mockAxios.onAny().reply(200);
    process.env = {
      ...OLD_ENV,
      REACT_APP_TENANT: undefined,
    };
  });

  afterAll(() => {
    process.env = OLD_ENV;
    mockAxios.reset();
    jest.restoreAllMocks();
  });

  it('Tenant returns correct default configuration', async () => {
    const { findByText, asFragment } = render(renderTenant());
    expect(await findByText(/Default Tenant Name/)).toBeVisible();
    expect(asFragment()).toMatchSnapshot();
  });

  it('Tenant returns correct non-existing configuration', async () => {
    process.env.REACT_APP_TENANT = 'FAKE';
    const { findByText, asFragment } = render(renderTenant());
    expect(await findByText(/Default Tenant Name/)).toBeVisible();
    expect(asFragment()).toMatchSnapshot();
  });

  it('Tenant returns correct MOTI configuration', async () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    const { findByText, asFragment } = render(renderTenant());
    expect(await findByText(/Property Information Management System/)).toBeVisible();
    expect(asFragment()).toMatchSnapshot();
  });
});
