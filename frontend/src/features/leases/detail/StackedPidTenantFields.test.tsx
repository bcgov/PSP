import { createMemoryHistory } from 'history';
import { defaultLease } from 'interfaces';
import { render, RenderOptions } from 'utils/test-utils';

import StackedPidTenantFields, { IStackedPidTenantFieldsProps } from './StackedPidTenantFields';

const history = createMemoryHistory();
const onClickManagement = jest.fn();

describe('StackedPidTenantFields component', () => {
  const setup = (
    renderOptions: RenderOptions & IStackedPidTenantFieldsProps = { lease: { ...defaultLease } },
  ) => {
    // render component under test
    const component = render(<StackedPidTenantFields lease={renderOptions.lease} />, {
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
      lease: {
        ...defaultLease,
        properties: [{ pid: '111-111-111' } as any],
        persons: [{ firstName: 'First', surname: 'Last' }],
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders pids appropriately', () => {
    const {
      component: { getByText },
    } = setup({
      lease: {
        ...defaultLease,
        properties: [{ pid: '111-111-111' } as any, { pid: '222-222-222' }, { pid: '333-333-333' }],
      },
    });
    expect(getByText('111-111-111, 222-222-222, 333-333-333')).toBeVisible();
  });

  it('renders tenant appropriately', () => {
    const {
      component: { getByText },
    } = setup({
      lease: { ...defaultLease, persons: [{ firstName: 'tenantFirst', surname: 'tenantSurname' }] },
    });
    expect(getByText('tenantFirst tenantSurname')).toBeVisible();
  });
});
