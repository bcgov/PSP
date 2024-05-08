import { createMemoryHistory } from 'history';

import { getEmptyPerson } from '@/mocks/contacts.mock';
import { getEmptyLeaseTenant } from '@/mocks/lease.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import StackedPidTenantFields, { IStackedTenantFieldsProps } from './StackedTenantFields';

const history = createMemoryHistory();
const onClickManagement = vi.fn();

describe('StackedPidTenantFields component', () => {
  const setup = (renderOptions: RenderOptions & IStackedTenantFieldsProps = { tenants: [] }) => {
    // render component under test
    const component = render(<StackedPidTenantFields tenants={renderOptions.tenants} />, {
      ...renderOptions,
      history,
    });

    return {
      component,
    };
  };
  beforeEach(() => {
    onClickManagement.mockReset();
  });
  it('renders as expected when provided no data', () => {
    const { component } = setup();
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('renders as expected when provided data', () => {
    const { component } = setup({
      tenants: [
        {
          ...getEmptyLeaseTenant(),
          leaseId: 1,
          personId: 1,
          person: { ...getEmptyPerson(), id: 1, firstName: 'First', surname: 'Last' },
        },
      ],
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders tenant appropriately', () => {
    const {
      component: { getByText },
    } = setup({
      tenants: [
        {
          ...getEmptyLeaseTenant(),
          leaseId: 1,
          personId: 1,
          person: {
            ...getEmptyPerson(),
            id: 1,
            firstName: 'tenantFirst',
            surname: 'tenantSurname',
          },
        },
      ],
    });
    expect(getByText('tenantFirst tenantSurname')).toBeVisible();
  });
});
