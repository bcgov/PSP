import { setTenant, tenant, defaultTenant } from '.';
import { Config as CITZ } from './CITZ/config';
import { Config as MOTI } from './MOTI/config';

describe('Tenant configuration', () => {
  const OLD_ENV = process.env;

  beforeEach(() => {
    jest.resetModules();
    process.env = {
      ...OLD_ENV,
      REACT_APP_TENANT: undefined,
    };
    setTenant();
  });

  afterAll(() => {
    process.env = OLD_ENV;
  });

  it('Tenant returns correct default configuration', () => {
    expect(tenant).toBe(defaultTenant);
  });

  it('Tenant returns correct non-existing configuration', () => {
    process.env.REACT_APP_TENANT = 'FAKE';
    setTenant();
    expect(tenant).toBe(defaultTenant);
  });

  it('Tenant returns correct MOTI configuration', () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    setTenant();
    expect(tenant).toBe(MOTI);
  });

  it('Tenant returns correct CITZ configuration', () => {
    process.env.REACT_APP_TENANT = 'CITZ';
    setTenant();
    expect(tenant).toBe(CITZ);
  });
});
