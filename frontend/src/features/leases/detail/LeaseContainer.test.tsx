import { createMemoryHistory } from 'history';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { ILease } from 'interfaces';
import { render, RenderOptions, waitFor } from 'utils/test-utils';

import { ILeaseAndLicenseContainerProps, LeaseContainer } from '..';

const history = createMemoryHistory();

const mockLease: ILease = {
  id: 1,
  address: '123 fake st',
  expiryDate: '2000-01-01',
  pidOrPin: '123-456-789',
  lFileNo: '111-222-333',
  properties: [{ pid: '987-654-321' } as any],
  tenantName: 'tenant name',
};

const getLease = jest.fn();
jest.mock('hooks/pims-api/useApiLeases');
((useApiLeases as unknown) as jest.Mock<Partial<typeof useApiLeases>>).mockReturnValue({
  getLease,
});

describe('LeaseContainer component', () => {
  const setup = (renderOptions?: RenderOptions & ILeaseAndLicenseContainerProps) => {
    // render component under test
    const component = render(
      <LeaseContainer match={renderOptions?.match ?? { params: { leaseId: 1 } }} />,
      {
        ...renderOptions,
        history,
      },
    );

    return {
      component,
    };
  };
  beforeEach(() => {
    getLease.mockReset();
    history.push('/lease/1?leasePageName=details');
  });
  it('renders as expected', () => {
    const { component } = setup();
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('loads a lease by lease id', async () => {
    getLease.mockResolvedValue({ data: mockLease });
    setup();
    await waitFor(async () => {
      expect(getLease).toHaveBeenCalled();
    });
  });

  it('throws an error if no lease id is provided', async () => {
    const {
      component: { findByText },
    } = setup({ match: {} });
    expect(
      await findByText(
        'No valid lease id provided, go back to the lease and license list and select a valid lease.',
      ),
    ).toBeVisible();
  });

  it('throws an error if the lease fails to load', async () => {
    getLease.mockRejectedValue({});
    const {
      component: { findByText },
    } = setup();
    expect(await findByText('Failed to load lease, reload this page to try again.')).toBeVisible();
  });
});
