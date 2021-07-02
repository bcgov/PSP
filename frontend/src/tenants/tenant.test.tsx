import { render } from '@testing-library/react';

import { defaultTenant, TenantConsumer, TenantProvider } from '.';

const unmockedFetch = global.fetch;
const renderTenant = () => {
  return (
    <TenantProvider>
      <TenantConsumer>{({ tenant }) => <div>{JSON.stringify(tenant)}</div>}</TenantConsumer>
    </TenantProvider>
  );
};

describe('Tenant configuration', () => {
  const OLD_ENV = process.env;
  const mockFetch = () =>
    Promise.resolve({ json: () => Promise.resolve(JSON.stringify(defaultTenant)) }) as Promise<
      Response
    >;

  beforeEach(() => {
    jest.resetModules();
    process.env = {
      ...OLD_ENV,
      REACT_APP_TENANT: undefined,
    };
    global.fetch = mockFetch as any;
  });

  afterAll(() => {
    process.env = OLD_ENV;
    global.fetch = unmockedFetch;
    jest.restoreAllMocks();
  });

  it('Tenant returns correct default configuration', async () => {
    const { container, findByText } = render(renderTenant());
    await findByText(/Default Tenant Name/);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('Tenant returns correct non-existing configuration', async () => {
    process.env.REACT_APP_TENANT = 'FAKE';
    const { container, findByText } = render(renderTenant());
    await findByText(/Default Tenant Name/);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('Tenant returns correct MOTI configuration', async () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    const { container, findByText } = render(renderTenant());
    await findByText(/Property Information Management System/);
    expect(container.firstChild).toMatchSnapshot();
  });
});
