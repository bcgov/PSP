import { act, render } from '@testing-library/react';
import { config, defaultTenant, ITenantConfig } from 'tenants';
import { useTenant } from 'tenants/useTenant';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import { Header } from './Header';

jest.mock('tenants/useTenant');
const mockUseTenant = useTenant as jest.Mock<ITenantConfig>;

const testRender = () =>
  render(
    <TestCommonWrapper>
      <Header />
    </TestCommonWrapper>,
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
