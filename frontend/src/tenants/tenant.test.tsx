import renderer, { act } from 'react-test-renderer';

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
    global.fetch = mockFetch;
  });

  afterAll(() => {
    process.env = OLD_ENV;
    global.fetch = unmockedFetch;
  });

  it('Tenant returns correct default configuration', () => {
    act(() => {
      const json = renderer.create(renderTenant()).toJSON();
      expect(json).toMatchSnapshot();
    });
  });

  it('Tenant returns correct non-existing configuration', () => {
    act(() => {
      process.env.REACT_APP_TENANT = 'FAKE';
      const json = renderer.create(renderTenant()).toJSON();
      expect(json).toMatchSnapshot();
    });
  });

  it('Tenant returns correct MOTI configuration', () => {
    act(() => {
      process.env.REACT_APP_TENANT = 'MOTI';
      const json = renderer.create(renderTenant()).toJSON();
      expect(json).toMatchSnapshot();
    });
  });

  it('Tenant returns correct CITZ configuration', () => {
    act(() => {
      process.env.REACT_APP_TENANT = 'CITZ';
      const json = renderer.create(renderTenant()).toJSON();
      expect(json).toMatchSnapshot();
    });
  });
});
