import { createMemoryHistory } from 'history';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { defaultLease, ILease } from 'interfaces';
import { mockOrganization } from 'mocks/filterDataMock';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from 'utils/test-utils';

import { ILeaseAndLicenseContainerProps, LeaseContainer } from '..';

const history = createMemoryHistory();

const mockLease: ILease = {
  ...defaultLease,
  id: 1,
  expiryDate: '2000-01-01',
  lFileNo: '111-222-333',
  properties: [{ pid: '987-654-321' } as any],
  persons: [{ fullName: 'First Last' }],
  organizations: [mockOrganization],
};

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: [] },
};

const getLease = jest.fn();
jest.mock('hooks/pims-api/useApiLeases');
((useApiLeases as unknown) as jest.Mock<Partial<typeof useApiLeases>>).mockReturnValue({
  getLease,
});
jest.mock('@react-keycloak/web');

describe('LeaseContainer component', () => {
  const setup = (renderOptions?: RenderOptions & ILeaseAndLicenseContainerProps) => {
    // render component under test
    const component = render(
      <LeaseContainer match={renderOptions?.match ?? { params: { leaseId: 1 } }} />,
      {
        ...renderOptions,
        useMockAuthentication: true,
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
    const { component } = setup({ store: storeState });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('loads a lease by lease id', async () => {
    getLease.mockResolvedValue({ data: mockLease });
    setup({ store: storeState });
    await waitFor(async () => {
      expect(getLease).toHaveBeenCalled();
    });
  });

  it('throws an error if the lease fails to load', async () => {
    getLease.mockRejectedValue({});
    const {
      component: { findByText },
    } = setup({ store: storeState });
    expect(await findByText('Failed to load lease, reload this page to try again.')).toBeVisible();
  });
});
