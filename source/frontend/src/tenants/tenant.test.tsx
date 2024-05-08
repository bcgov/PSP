import { act, render } from '@testing-library/react';
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
  const OLD_ENV = import.meta.env.VITE_TENANT;

  beforeEach(() => {
    vi.resetModules();
    mockAxios.onAny().reply(200);
    import.meta.env.VITE_TENANT = undefined;
  });

  afterAll(() => {
    import.meta.env.VITE_TENANT = OLD_ENV;
    mockAxios.reset();
    vi.restoreAllMocks();
  });

  it('Tenant returns correct default configuration', async () => {
    const { findByText, asFragment } = render(renderTenant());
    await act(async () => {});
    expect(await findByText(/Default Tenant Name/)).toBeVisible();
    expect(asFragment()).toMatchSnapshot();
  });

  it('Tenant returns correct non-existing configuration', async () => {
    import.meta.env.VITE_TENANT = 'FAKE';
    const { findByText, asFragment } = render(renderTenant());
    await act(async () => {});
    expect(await findByText(/Default Tenant Name/)).toBeVisible();
    expect(asFragment()).toMatchSnapshot();
  });

  it('Tenant returns correct MOTI configuration', async () => {
    import.meta.env.VITE_TENANT = 'MOTI';
    const { findByText, asFragment } = render(renderTenant());
    expect(await findByText(/Property Information Management System/)).toBeVisible();
    expect(asFragment()).toMatchSnapshot();
  });
});
