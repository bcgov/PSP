import { createMemoryHistory } from 'history';
import { render, RenderOptions } from 'utils/test-utils';

import StackedPidTenantFields, { IStackedPidTenantFieldsProps } from './StackedPidTenantFields';

const history = createMemoryHistory();
const onClickManagement = jest.fn();

describe('StackedPidTenantFields component', () => {
  const setup = (renderOptions: RenderOptions & IStackedPidTenantFieldsProps = { lease: {} }) => {
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
        properties: [{ pid: '111-111-111' } as any],
        tenant: { name: 'tenant' },
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders pids appropriately', () => {
    const {
      component: { getByText },
    } = setup({
      lease: {
        properties: [{ pid: '111-111-111' } as any, { pid: '222-222-222' }, { pid: '333-333-333' }],
      },
    });
    expect(getByText('111-111-111, 222-222-222, 333-333-333')).toBeVisible();
  });

  it('renders tenant appropriately', () => {
    const {
      component: { getByText },
    } = setup({ lease: { tenant: { name: 'tenant' } } });
    expect(getByText('tenant')).toBeVisible();
  });
});
