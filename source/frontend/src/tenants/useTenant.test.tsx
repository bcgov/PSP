import { render } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { config, TenantProvider, useTenant } from '.';
import defaultTenant from './config/defaultTenant';

const origEnv = process.env;

const mockAxios = new MockAdapter(axios);

const TestTenant = () => {
  const tenant = useTenant();
  return <div data-testid="tenant">{JSON.stringify(tenant)}</div>;
};

const testRender = () =>
  render(
    <TenantProvider>
      <TestTenant></TestTenant>
    </TenantProvider>,
  );

describe('useTenant hook', () => {
  beforeEach(() => {
    mockAxios.onAny().reply(200);
    jest.resetModules();
    process.env = { ...origEnv };
  });

  afterAll(() => {
    process.env = origEnv;
    mockAxios.reset();
    jest.restoreAllMocks();
  });

  it('returns default configuration when REACT_APP_TENANT is not set', async () => {
    process.env.REACT_APP_TENANT = undefined;
    const { findByTestId } = testRender();
    const title = await findByTestId('tenant');
    const tenant = title.innerHTML.replace(/&amp;/g, '&');
    expect({ ...JSON.parse(tenant), propertiesUrl: undefined }).toStrictEqual({
      ...defaultTenant,
      propertiesUrl: undefined,
    });
  });

  it('returns default configuration when REACT_APP_TENANT is invalid', async () => {
    process.env.REACT_APP_TENANT = 'FAKE_I_DONT_EXIST';
    const { findByTestId } = testRender();
    const title = await findByTestId('tenant');
    const tenant = title.innerHTML.replace(/&amp;/g, '&');
    expect({ ...JSON.parse(tenant), propertiesUrl: undefined }).toStrictEqual({
      ...defaultTenant,
      propertiesUrl: undefined,
    });
  });

  it('returns correct MOTI tenant configuration', async () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    const { findByTestId } = testRender();
    const title = await findByTestId('tenant');
    const tenant = title.innerHTML.replace(/&amp;/g, '&');
    expect({ ...JSON.parse(tenant), propertiesUrl: undefined }).toStrictEqual({
      ...defaultTenant,
      ...config['MOTI'],
      propertiesUrl: undefined,
    });
  });
});
