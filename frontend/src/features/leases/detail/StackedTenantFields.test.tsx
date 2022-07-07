import { createMemoryHistory } from 'history';
import { defaultLease } from 'interfaces';
import { render, RenderOptions } from 'utils/test-utils';

import StackedPidTenantFields, { IStackedTenantFieldsProps } from './StackedTenantFields';

const history = createMemoryHistory();
const onClickManagement = jest.fn();

describe('StackedPidTenantFields component', () => {
  const setup = (
    renderOptions: RenderOptions & IStackedTenantFieldsProps = { lease: { ...defaultLease } },
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

  it('renders tenant appropriately', () => {
    const {
      component: { getByText },
    } = setup({
      lease: { ...defaultLease, persons: [{ firstName: 'tenantFirst', surname: 'tenantSurname' }] },
    });
    expect(getByText('tenantFirst tenantSurname')).toBeVisible();
  });
});
