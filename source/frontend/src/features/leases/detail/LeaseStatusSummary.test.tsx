import { createMemoryHistory } from 'history';
import { defaultLease } from 'interfaces';
import moment from 'moment';
import { render, RenderOptions } from 'utils/test-utils';

import { ILeaseStatusSummaryProps, LeaseStatusSummary } from './LeaseStatusSummary';

const history = createMemoryHistory();
const onClickManagement = jest.fn();

describe('LeaseStatusSummary component', () => {
  const setup = (renderOptions: RenderOptions & ILeaseStatusSummaryProps) => {
    // render component under test
    const component = render(<LeaseStatusSummary lease={renderOptions.lease} />, {
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
  it('renders as expected when active', () => {
    const futureExpiry = moment().add(1, 'days');
    const { component } = setup({
      lease: { ...defaultLease, expiryDate: futureExpiry.format('YYYY-MM-DD') },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('renders as expected when inactive', () => {
    const pastExpiry = moment().subtract(1, 'days');
    const { component } = setup({
      lease: { ...defaultLease, expiryDate: pastExpiry.format('YYYY-MM-DD') },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('displays the lFileNo when provided', () => {
    const {
      component: { getByText },
    } = setup({ lease: { ...defaultLease, lFileNo: '111-222-333' } });
    expect(getByText('111-222-333')).toBeVisible();
  });
});
