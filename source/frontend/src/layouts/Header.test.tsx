import { ITenantConfig2 } from '@/hooks/pims-api/interfaces/ITenantConfig';
import { config } from '@/tenants';
import defaultTenant from '@/tenants/config/defaultTenant';
import { useTenant } from '@/tenants/useTenant';
import { render } from '@/utils/test-utils';

import { Header } from './Header';

jest.mock('@/tenants/useTenant');
const mockUseTenant = useTenant as jest.Mock<ITenantConfig2>;

const testRender = () => render(<Header />);

describe('Tenant Header', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Header default background', async () => {
    mockUseTenant.mockReturnValue(defaultTenant);
    const { asFragment } = testRender();
    expect(asFragment()).toMatchSnapshot();
  });

  it('Header black background', async () => {
    mockUseTenant.mockReturnValue({ ...defaultTenant, colour: 'black' });
    const { asFragment } = testRender();
    expect(asFragment()).toMatchSnapshot();
  });

  it('Header MOTI background', async () => {
    mockUseTenant.mockReturnValue({ ...defaultTenant, ...config['MOTI'] });
    const { asFragment } = testRender();
    expect(asFragment()).toMatchSnapshot();
  });
});
