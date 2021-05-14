import { TenantProvider, defaultTenant, useTenant, config } from '.';
import { render, getByTestId, act } from '@testing-library/react';

const unmockedFetch = global.fetch;
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
    global.fetch = mockFetch;
  });

  afterAll(() => {
    process.env = OLD_ENV;
    global.fetch = unmockedFetch;
  });

  it('Tenant returns correct default configuration', () => {
    act(() => {
      const { container } = testRender();
      const title = getByTestId(container, 'tenant');
      expect(title).toContainHTML(JSON.stringify(defaultTenant));
    });
  });

  it('Tenant returns correct non-existing configuration', () => {
    act(() => {
      process.env.REACT_APP_TENANT = 'FAKE';
      const { container } = testRender();
      const title = getByTestId(container, 'tenant');
      expect(title).toContainHTML(JSON.stringify(defaultTenant));
    });
  });

  it('Tenant returns correct MOTI configuration', () => {
    act(() => {
      process.env.REACT_APP_TENANT = 'MOTI';
      const { container } = testRender();
      const title = getByTestId(container, 'tenant');
      expect(title).toContainHTML(JSON.stringify({ ...defaultTenant, ...config['MOTI'] }));
    });
  });

  it('Tenant returns correct CITZ configuration', () => {
    act(() => {
      process.env.REACT_APP_TENANT = 'CITZ';
      const { container } = testRender();
      const title = getByTestId(container, 'tenant');
      expect(title).toContainHTML(JSON.stringify({ ...defaultTenant, ...config['CITZ'] }));
    });
  });
});
