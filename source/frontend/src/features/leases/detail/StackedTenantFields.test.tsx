import { createMemoryHistory } from 'history';

import { render, RenderOptions } from '@/utils/test-utils';

import StackedPidTenantFields, { IStackedTenantFieldsProps } from './StackedTenantFields';

const history = createMemoryHistory();
const onClickManagement = jest.fn();

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
          leaseId: 1,
          person: { firstName: 'First', surname: 'Last' },
        },
      ],
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders tenant appropriately', () => {
    const {
      component: { getByText },
    } = setup({
      tenants: [{ leaseId: 1, person: { firstName: 'tenantFirst', surname: 'tenantSurname' } }],
    });
    expect(getByText('tenantFirst tenantSurname')).toBeVisible();
  });
});
