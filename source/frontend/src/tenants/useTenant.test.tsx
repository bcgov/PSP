import { act, render } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { config, TenantProvider, useTenant } from '.';
import defaultTenant from './config/defaultTenant';

const origEnv = import.meta.env.VITE_TENANT;

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
    vi.resetModules();
    import.meta.env.VITE_TENANT = { ...origEnv };
  });

  afterAll(() => {
    import.meta.env.VITE_TENANT = origEnv;
    mockAxios.reset();
    vi.restoreAllMocks();
  });

  it('returns default configuration when VITE_TENANT is not set', async () => {
    import.meta.env.VITE_TENANT = undefined;
    const { findByTestId } = testRender();
    await act(async () => {});
    const title = await findByTestId('tenant');
    const tenant = title.innerHTML.replace(/&amp;/g, '&');
    expect({ ...JSON.parse(tenant), propertiesUrl: undefined }).toStrictEqual({
      ...defaultTenant,
      propertiesUrl: undefined,
    });
  });

  it('returns default configuration when VITE_TENANT is invalid', async () => {
    import.meta.env.VITE_TENANT = 'FAKE_I_DONT_EXIST';
    const { findByTestId } = testRender();
    const title = await findByTestId('tenant');
    const tenant = title.innerHTML.replace(/&amp;/g, '&');
    expect({ ...JSON.parse(tenant), propertiesUrl: undefined }).toStrictEqual({
      ...defaultTenant,
      propertiesUrl: undefined,
    });
  });

  it('returns correct MOTI tenant configuration', async () => {
    import.meta.env.VITE_TENANT = 'MOTI';
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
