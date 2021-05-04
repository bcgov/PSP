import { tenant, defaultTenant } from '.';
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
  });

  afterAll(() => {
    process.env = OLD_ENV;
  });

  test('Tenant returns correct default configuration', () => {
    expect(tenant()).toBe(defaultTenant);
  });

  test('Tenant returns correct non-existing configuration', () => {
    process.env.REACT_APP_TENANT = 'FAKE';
    expect(tenant()).toBe(defaultTenant);
  });

  test('Tenant returns correct MOTI configuration', () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    expect(tenant()).toBe(MOTI);
  });

  test('Tenant returns correct CITZ configuration', () => {
    process.env.REACT_APP_TENANT = 'CITZ';
    expect(tenant()).toBe(CITZ);
  });
});
