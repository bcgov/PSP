import React from 'react';
import { act, render } from '@testing-library/react';
import { TenantProvider, ITenantConfig, defaultTenant, config, TenantConsumer } from 'tenants';
import { useTenant } from 'tenants/useTenant';
import { Header } from './Header';
import { ThemeProvider } from 'styled-components';

jest.mock('tenants/useTenant');
const mockUseTenant = useTenant as jest.Mock<ITenantConfig>;
global.fetch = jest.fn(
  () =>
    Promise.resolve({ json: () => Promise.resolve(JSON.stringify(defaultTenant)) }) as Promise<
      Response
    >,
);

const testRender = () =>
  render(
    <TenantProvider>
      <TenantConsumer>
        {({ tenant }) => (
          <ThemeProvider theme={{ tenant, css: {} }}>
            <Header />
          </ThemeProvider>
        )}
      </TenantConsumer>
    </TenantProvider>,
  );

describe('Tenant Header', () => {
  const OLD_ENV = process.env;

  beforeEach(() => {
    delete process.env.REACT_APP_TENANT;
  });

  afterEach(() => {
    jest.clearAllMocks();
    process.env = OLD_ENV;
  });

  it('Header default background', async () => {
    mockUseTenant.mockImplementation(() => defaultTenant);

    await act(async () => {
      const { container } = testRender();
      expect(container).toMatchSnapshot();
    });
  });

  it('Header black background', async () => {
    mockUseTenant.mockImplementation(() => ({ ...defaultTenant, colour: 'black' }));

    await act(async () => {
      const { container } = testRender();
      expect(container).toMatchSnapshot();
    });
  });

  it('Header MOTI background', async () => {
    mockUseTenant.mockImplementation(() => ({ ...defaultTenant, ...config['MOTI'] }));

    await act(async () => {
      const { container } = testRender();
      expect(container).toMatchSnapshot();
    });
  });

  it('Header CITZ background', async () => {
    mockUseTenant.mockImplementation(() => ({ ...defaultTenant, ...config['CITZ'] }));

    await act(async () => {
      const { container } = testRender();
      expect(container).toMatchSnapshot();
    });
  });
});
