import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { LeaseFormModel } from '@/features/leases/models';
import { mockApiOrganization, mockApiPerson } from '@/mocks/filterData.mock';
import { getEmptyLeaseTenant, getMockApiLease } from '@/mocks/lease.mock';
import { defaultApiLease } from '@/models/defaultInitializers';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { FormTenant } from './models';
import PrimaryContactWarningModal from './PrimaryContactWarningModal';

const history = createMemoryHistory();

describe('PrimaryContactWarningModal component', () => {
  const setup = (
    renderOptions: RenderOptions & { tenants?: FormTenant[]; saveCallback?: () => void } = {},
  ) => {
    // render component under test
    const component = render(
      <PrimaryContactWarningModal
        selectedTenants={renderOptions.tenants ?? []}
        saveCallback={renderOptions.saveCallback}
      />,
      {
        ...renderOptions,
        history,
      },
    );

    return {
      component,
    };
  };
  it('renders as expected', () => {
    const { component } = setup({
      tenants: LeaseFormModel.fromApi({
        ...defaultApiLease(),
        tenants: [
          { ...getEmptyLeaseTenant(), leaseId: 1, person: mockApiPerson },
          { ...getEmptyLeaseTenant(), leaseId: 1, organization: mockApiOrganization },
        ],
      }).tenants,
    });
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('calls saveCallback on save', async () => {
    const saveCallback = vi.fn();
    const { component } = setup({
      saveCallback: saveCallback,
      tenants: LeaseFormModel.fromApi({
        ...defaultApiLease(),
        tenants: [
          { ...getEmptyLeaseTenant(), leaseId: 1, person: mockApiPerson },
          { ...getEmptyLeaseTenant(), leaseId: 1, person: mockApiPerson },
        ],
      }).tenants,
    });
    const { getByText } = component;
    const save = getByText('Save');
    await act(async () => userEvent.click(save));

    expect(saveCallback).toHaveBeenCalled();
  });

  it('displays all organization tenants that have multiple persons and no primary contact', () => {
    setup({
      tenants: LeaseFormModel.fromApi({
        ...getMockApiLease(),
        tenants: [
          {
            ...getMockApiLease().tenants![0],
            primaryContactId: null,
            primaryContact: null,
          },
        ],
      }).tenants,
      saveCallback: noop,
    });
    const tenantText = screen.getByText(content =>
      content.includes('French Mouse Property Management'),
    );

    expect(tenantText).toBeVisible();
  });
});
